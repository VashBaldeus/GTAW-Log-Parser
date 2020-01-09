using System;
using Octokit;
using System.IO;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Controls;
using MetroParser.Localization;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static System.Windows.Forms.NotifyIcon TrayIcon;

        private static readonly GitHubClient client = new GitHubClient(new ProductHeaderValue(Data.ProductHeader));
        private static Thread updateThread;
        private readonly bool loading = true;

        private bool isRestarting = false;

        public MainWindow(bool startMinimized)
        {
            StartupHandler.Initialize();

            InitializeTrayIcon();

            if (startMinimized)
            {
                TrayIcon.Visible = true;
                Visibility = Visibility.Hidden;
            }

            InitializeComponent();

            // Also checks for the RAGEMP folder on the first start
            LoadSettings();

            string currentLanguage = LocalizationManager.GetLanguageFromCode(LocalizationManager.GetLanguage());
            for (int i = 0; i < ((LocalizationManager.Language[])Enum.GetValues(typeof(LocalizationManager.Language))).Length; ++i)
            {
                LocalizationManager.Language language = (LocalizationManager.Language)i;

                MenuItem menuItem = new MenuItem
                {
                    Header = language.ToString()
                };

                LanguageToolStripMenuItem.Items.Add(menuItem);
                menuItem.Click += (s, e) =>
                {
                    if (menuItem.IsChecked == true)
                        return;

                    CultureInfo cultureInfo = new CultureInfo(LocalizationManager.GetCodeFromLanguage(language));

                    if (MessageBox.Show(Strings.ResourceManager.GetString("Restart", cultureInfo), Strings.ResourceManager.GetString("RestartTitle", cultureInfo), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        LocalizationManager.SetLanguage(language);

                        Hide();
                        isRestarting = true;

                        ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
                        startInfo.FileName = Data.ExecutablePath;
                        startInfo.Arguments = $"{Data.ParameterPrefix}restart";
                        Process.Start(startInfo);

                        System.Windows.Application.Current.Shutdown();
                    }
                };

                if (currentLanguage == language.ToString())
                    menuItem.IsChecked = true;
            }

            BackupHandler.Initialize();

            if (Properties.Settings.Default.CheckForUpdatesAutomatically)
                TryCheckingForUpdates(manual: false);

            loading = false;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FolderPath = FolderPath.Text;
            Properties.Settings.Default.RemoveTimestamps = RemoveTimestamps.IsChecked == true;
            Properties.Settings.Default.CheckForUpdatesAutomatically = CheckForUpdatesOnStartup.IsChecked == true;

            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Version.Content = string.Format(Strings.VersionInfo, Properties.Settings.Default.Version);
            StatusLabel.Content = string.Format(Strings.BackupStatus, Properties.Settings.Default.BackupChatLogAutomatically ? Strings.Enabled : Strings.Disabled);
            Counter.Content = string.Format(Strings.CharacterCounter, 0, 0);

            if (Properties.Settings.Default.FirstStart)
            {
                Properties.Settings.Default.FirstStart = false;
                Properties.Settings.Default.Save();

                LookForMainFolder();

                // Warning
                //MessageBox.Show(Strings.LanguageInfo, Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                FolderPath.Text = Properties.Settings.Default.FolderPath;

            RemoveTimestamps.IsChecked = Properties.Settings.Default.RemoveTimestamps;
            CheckForUpdatesOnStartup.IsChecked = Properties.Settings.Default.CheckForUpdatesAutomatically;
        }

        private void LookForMainFolder()
        {
            try
            {
                string folderPath = string.Empty;

                foreach (var drive in DriveInfo.GetDrives())
                {
                    foreach (string possibleFolder in Data.PossibleFolderLocations)
                    {
                        if (Directory.Exists(drive.Name + possibleFolder))
                        {
                            folderPath = drive.Name + possibleFolder;
                            break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(folderPath))
                        break;
                }

                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    MessageBox.Show(Strings.FolderFinderNotFound, Strings.FolderFinderTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                FolderPath.Text = folderPath;
                MessageBox.Show(string.Format(Strings.FolderFinder, folderPath), Strings.FolderFinderTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                FolderPath.Text = string.Empty;
                MessageBox.Show(Strings.FolderFinderError, Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FolderPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loading)
                return;

            if (Properties.Settings.Default.BackupChatLogAutomatically)
            {
                BackupSettingsWindow.ResetSettings();

                StatusLabel.Content = string.Format(Strings.BackupStatus, Strings.Disabled);
                MessageBox.Show(Strings.BackupTurnedOff, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FolderPath_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text))
                Browse_Click(this, null);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory),
                IsFolderPicker = true
            };

            bool validLocation = false;

            while (!validLocation)
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (dialog.FileName[dialog.FileName.Length - 1] != '\\')
                    {
                        FolderPath.Text = dialog.FileName + "\\";
                        validLocation = true;
                    }
                    else
                        MessageBox.Show(Strings.BadFolderPath, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    validLocation = true;
            }
        }

        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPath, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!File.Exists(FolderPath.Text + Data.LogLocation))
            {
                MessageBox.Show(Strings.NoChatLog, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Parsed.Text = ParseChatLog(FolderPath.Text, RemoveTimestamps.IsChecked == true, showError: true);
        }

        public static string ParseChatLog(string folderPath, bool removeTimestamps, bool showError = false)
        {
            try
            {
                Data.Initialize();

                string log;
                using (StreamReader sr = new StreamReader(folderPath + Data.LogLocation))
                {
                    log = sr.ReadToEnd();
                }

                bool oldLog = false;
                string tempLog = Regex.Match(log, "\\\"chatlog\\\":\\\".+\\\\n\\\"").Value;
                if (string.IsNullOrWhiteSpace(tempLog))
                {
                    tempLog = "\"chatlog\":" + log;
                    tempLog = Regex.Match(tempLog, "\\\"chatlog\\\":\\\".+\\\\n\\\"").Value;

                    if (!string.IsNullOrWhiteSpace(tempLog))
                    {
                        oldLog = true;

                        if (showError)
                        {
                            if (MessageBox.Show(Strings.OldChatLog, Strings.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                try
                                {
                                    int foundDirectories = 0;

                                    foreach (string ip in Data.ServerIPs)
                                    {
                                        if (Directory.Exists($"{folderPath}client_resources\\{ip}"))
                                        {
                                            foundDirectories++;

                                            if (File.Exists($"{folderPath}client_resources\\{ip}\\.storage"))
                                                File.Delete($"{folderPath}client_resources\\{ip}\\.storage");

                                            foreach (string file in Data.PotentiallyOldFiles)
                                            {
                                                if (File.Exists($"{folderPath}client_resources\\{ip}\\gtalife\\{file}"))
                                                    File.Delete($"{folderPath}client_resources\\{ip}\\gtalife\\{file}");
                                            }
                                        }
                                    }

                                    if (foundDirectories > 1)
                                        MessageBox.Show(string.Format(Strings.MultipleChatLogs, Data.ServerIPs[0], Data.ServerIPs[1]), Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                catch
                                {
                                    MessageBox.Show(Strings.FileDeleteError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }

                log = tempLog;

                log = log.Replace("\"chatlog\":\"", string.Empty);  // Remove the chat log indicator
                log = log.Replace("\\n", "\n");                     // Change all occurrences of `\n` into new lines
                log = log.Remove(log.Length - 1, 1);                // Remove the `"` character from the end

                if (oldLog)
                {
                    log = Regex.Replace(log, "<[^>]*>", string.Empty);                      // Remove the HTML tags that are added for the chat (example: `If the ingame menus are out of place, use <span style=\"color: dodgerblue\">/movemenu</span>`)

                    log = Regex.Replace(log, "~[A-Za-z]~", string.Empty);                   // Remove the RAGEMP color tags (example: `~r~` for red)
                    log = Regex.Replace(log, @"!{#(?:[0-9A-Fa-f]{3}){1,2}}", string.Empty); // Remove HEX color tags (example: `!{#FFEC8B}` for the yellow color picked for radio messages)
                }

                log = System.Net.WebUtility.HtmlDecode(log);    // Decode HTML symbols (example: `&apos;` into `'`)
                log = log.TrimEnd(new char[] { '\r', '\n' });   // Remove the `new line` characters from the end

                previousLog = log;

                if (removeTimestamps)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                return log;
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);


                return string.Empty;
            }
        }

        private void Parsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Counter == null)
                return;

            if (string.IsNullOrWhiteSpace(Parsed.Text))
            {
                Counter.Content = string.Format(Strings.CharacterCounter, 0, 0);
                return;
            }

            Counter.Content= string.Format(Strings.CharacterCounter, Parsed.Text.Length, Parsed.Text.Split('\n').Length);
        }

        private void SaveParsed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Parsed.Text))
                {
                    MessageBox.Show(Strings.NothingParsed, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "chatlog.txt",
                    Filter = "Text File | *.txt"
                };

                if (dialog.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(dialog.OpenFile()))
                    {
                        sw.Write(Parsed.Text.Replace("\n", Environment.NewLine));
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.SaveError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyParsedToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text))
                MessageBox.Show(Strings.NothingParsed, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        private void CheckForUpdatesOnStartup_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (CheckForUpdatesOnStartup.IsChecked == true)
                TryCheckingForUpdates();
        }

        private static string previousLog = string.Empty;
        private void RemoveTimestamps_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text) || string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\") || !File.Exists(FolderPath.Text+ Data.LogLocation))
                return;

            if (RemoveTimestamps.IsChecked == true)
            {
                previousLog = Parsed.Text;
                Parsed.Text = Regex.Replace(previousLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);
            }
            else if (!string.IsNullOrWhiteSpace(previousLog))
                Parsed.Text = previousLog;
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TryCheckingForUpdates(manual: true);
        }

        private void TryCheckingForUpdates(bool manual = false)
        {
            if (updateThread == null || !updateThread.IsAlive)
            {
                updateThread = new Thread(() => CheckForUpdates(manual));
                updateThread.Start();
            }
        }

        private void CheckForUpdates(bool manual = false)
        {
            try
            {
                string installedVersion = Properties.Settings.Default.Version;
                string currentVersion = client.Repository.Release.GetAll("MapleToo", "GTAW-Log-Parser").Result[0].TagName;

                if (string.Compare(installedVersion, currentVersion) < 0)
                {
                    if (Visibility != Visibility.Visible)
                        ResumeTrayStripMenuItem_Click(this, EventArgs.Empty);

                    if (MessageBox.Show(string.Format(Strings.UpdateAvailable, installedVersion, currentVersion), Strings.UpdateAvailableTitle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        Process.Start("https://github.com/MapleToo/GTAW-Log-Parser/releases");
                }
                else if (manual)
                    MessageBox.Show(string.Format(Strings.RunningLatest, installedVersion), Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                if (manual)
                    MessageBox.Show(string.Format(Strings.NoInternet, Properties.Settings.Default.Version), Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static BackupSettingsWindow backupSettings;
        private void AutomaticBackupSettingsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPathBackup, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Properties.Settings.Default.BackupChatLogAutomatically)
            {
                if (MessageBox.Show(Strings.BackupWillBeOff, Strings.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                StatusLabel.Content = string.Format(Strings.BackupStatus, Strings.Disabled);
            }
            else
                MessageBox.Show(Strings.SettingsAfterClose, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);

            BackupHandler.AbortAll();
            SaveSettings();

            if (backupSettings == null)
            {
                backupSettings = new BackupSettingsWindow();
                backupSettings.IsVisibleChanged += (s, args) =>
                {
                    if ((bool)args.NewValue == false)
                    {
                        BackupHandler.Initialize();
                        StatusLabel.Content = string.Format(Strings.BackupStatus, Properties.Settings.Default.BackupChatLogAutomatically ? Strings.Enabled : Strings.Disabled);
                    }
                };
            }

            backupSettings.LoadSettings();
            BringToFront(backupSettings);
        }

        private static ChatLogFilterWindow chatLogFilter;
        private void FilterChatLogToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPathFilter, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveSettings();

            if (chatLogFilter == null)
                chatLogFilter = new ChatLogFilterWindow();

            chatLogFilter.Initialize();
            BringToFront(chatLogFilter);
        }

        private void AboutToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format(Strings.About, Properties.Settings.Default.Version), Strings.Information, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                Process.Start("https://github.com/MapleToo/GTAW-Log-Parser");
        }

        private void ExitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Logo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show(Strings.OpenDocumentation, Strings.Information, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                Process.Start(Strings.ForumLink);
        }

        private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isRestarting)
            {
                if (Properties.Settings.Default.BackupChatLogAutomatically && TrayIcon.Visible == false)
                {
                    MessageBoxResult result = MessageBox.Show(Strings.MinimizeInsteadOfClose, Strings.Warning, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;

                        Hide();
                        TrayIcon.Visible = true;

                        return;
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            BackupHandler.quitting = true;
            SaveSettings();

            System.Windows.Application.Current.Shutdown();
        }

        private void TrayIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ResumeTrayStripMenuItem_Click(sender, EventArgs.Empty);
        }

        private void ResumeTrayStripMenuItem_Click(object sender, EventArgs e)
        {
            BringToFront(this, topMost: false);
            TrayIcon.Visible = false;
        }

        private void ExitTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Visibility != Visibility.Visible)
                BackupHandler.quitting = true;

            System.Windows.Application.Current.Shutdown();
        }

        public static void BringToFront(Window window, bool topMost = true)
        {
            window.Show();
            window.WindowState = WindowState.Normal;
            window.Activate();
            window.Topmost = true;
            if (!topMost)
                window.Topmost = false;
            window.Focus();
        }

        private void InitializeTrayIcon()
        {
            TrayIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = false,
                Icon = Properties.Resources.AppIcon
            };

            TrayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;

            ContextMenu menu = new ContextMenu();
            TrayIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            TrayIcon.ContextMenuStrip.Items.Add("Open", null, ResumeTrayStripMenuItem_Click);
            TrayIcon.ContextMenuStrip.Items.Add("Exit", null, ExitTrayToolStripMenuItem_Click);
        }
    }
}
