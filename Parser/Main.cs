using System;
using Octokit;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Globalization;
using Parser.Localization;

namespace Parser
{
    public partial class Main : Form
    {
        private static GitHubClient client = new GitHubClient(new ProductHeaderValue(Data.productHeader));
        private static Thread updateThread;

        private bool allowFormDisplay = false;

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowFormDisplay ? value : allowFormDisplay);
        }

        private static LanguagePicker languagePicker;

        public Main(bool startMinimized)
        {
            LocalizationManager.Initialize();

            if (Properties.Settings.Default.FirstStart)
            {
                if (languagePicker == null)
                {
                    languagePicker = new LanguagePicker();
                }
                else
                {
                    languagePicker.Initialize();

                    languagePicker.WindowState = FormWindowState.Normal;
                    languagePicker.BringToFront();
                }

                languagePicker.ShowDialog();
            }

            StartupHandler.Initialize();

            allowFormDisplay = !startMinimized;

            InitializeComponent();

            // Also checks for the RAGEMP folder on the first start
            LoadSettings();

            string currentLanguage = LocalizationManager.GetLanguageFromCode(LocalizationManager.GetLanguage());
            for (int i = 0; i < ((LocalizationManager.Language[])Enum.GetValues(typeof(LocalizationManager.Language))).Length; ++i)
            {
                LocalizationManager.Language language = (LocalizationManager.Language)i;
                ToolStripItem newLanguage = languageToolStripMenuItem.DropDownItems.Add(language.ToString());
                newLanguage.Click += (s, e) =>
                {
                    if (((ToolStripMenuItem)newLanguage).Checked == true)
                        return;

                    CultureInfo cultureInfo = new CultureInfo(LocalizationManager.GetCodeFromLanguage(language));

                    if (MessageBox.Show(Strings.ResourceManager.GetString("Restart", cultureInfo), Strings.ResourceManager.GetString("RestartTitle", cultureInfo), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        LocalizationManager.SetLanguage(language);

                        Hide();
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Forms.Application.Exit();
                    }
                };

                if (currentLanguage == language.ToString())
                    ((ToolStripMenuItem)languageToolStripMenuItem.DropDownItems[i]).Checked = true;
            }

            BackupHandler.Initialize();

            if (startMinimized)
                TrayIcon.Visible = true;

            if (Properties.Settings.Default.CheckForUpdatesAutomatically)
                TryCheckingForUpdates(manual: false);
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FolderPath = FolderPath.Text;
            Properties.Settings.Default.RemoveTimestamps = RemoveTimestamps.Checked;
            Properties.Settings.Default.CheckForUpdatesAutomatically = CheckForUpdatesOnStartup.Checked;

            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Version.Text = string.Format(Strings.VersionInfo, Properties.Settings.Default.Version);
            StatusLabel.Text = string.Format(Strings.BackupStatus, Properties.Settings.Default.BackupChatLogAutomatically ? Strings.Enabled : Strings.Disabled);
            Counter.Text = string.Format(Strings.CharacterCounter, 0, 0);

            if (Properties.Settings.Default.FirstStart)
            {
                Properties.Settings.Default.FirstStart = false;
                Properties.Settings.Default.Save();

                LookForMainFolder();

                if (LocalizationManager.GetLanguage() == LocalizationManager.GetCodeFromLanguage(LocalizationManager.Language.Español))
                {
                    MessageBox.Show("La traducción al español no está completa todavía.\n\nSi desea ayudar, visite la página de GitHub y contribuya.", Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                FolderPath.Text = Properties.Settings.Default.FolderPath;

            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestamps;
            CheckForUpdatesOnStartup.Checked = Properties.Settings.Default.CheckForUpdatesAutomatically;
        }

        private void LookForMainFolder()
        {
            try
            {
                string folderPath = string.Empty;

                foreach (var drive in DriveInfo.GetDrives())
                {
                    foreach (string possibleFolder in Data.possibleFolderLocations)
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
                    MessageBox.Show(Strings.FolderFinderNotFound, Strings.FolderFinderTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FolderPath.Text = folderPath;
                MessageBox.Show(string.Format(Strings.FolderFinder, folderPath), Strings.FolderFinderTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                FolderPath.Text = string.Empty;
                MessageBox.Show(Strings.FolderFinderError, Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FolderPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void FolderPath_TextChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.BackupChatLogAutomatically)
            {
                BackupSettings.ResetSettings();

                StatusLabel.Text = string.Format(Strings.BackupStatus, Strings.Disabled);
                MessageBox.Show(Strings.BackupTurnedOff, Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FolderPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text))
                Browse_Click(this, EventArgs.Empty);
        }

        private void Browse_Click(object sender, EventArgs e)
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
                        MessageBox.Show(Strings.BadFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    validLocation = true;
            }
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(FolderPath.Text + Data.logLocation))
            {
                MessageBox.Show(Strings.NoChatLog, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Parsed.Text = ParseChatLog(FolderPath.Text, RemoveTimestamps.Checked, showError: true);
        }

        public static string ParseChatLog(string folderPath, bool removeTimestamps, bool showError = false)
        {
            try
            {
                Data.Initialize();

                string log;
                using (StreamReader sr = new StreamReader(folderPath + Data.logLocation))
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
                            if (MessageBox.Show(Strings.OldChatLog, Strings.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                try
                                {
                                    int foundDirectories = 0;

                                    foreach (string ip in Data.serverIPs)
                                    {
                                        if (Directory.Exists($"{folderPath}client_resources\\{ip}"))
                                        {
                                            foundDirectories++;

                                            if (File.Exists($"{folderPath}client_resources\\{ip}\\.storage"))
                                                File.Delete($"{folderPath}client_resources\\{ip}\\.storage");

                                            foreach (string file in Data.potentiallyOldFiles)
                                            {
                                                if (File.Exists($"{folderPath}client_resources\\{ip}\\gtalife\\{file}"))
                                                    File.Delete($"{folderPath}client_resources\\{ip}\\gtalife\\{file}");
                                            }
                                        }
                                    }

                                    if (foundDirectories > 1)
                                        MessageBox.Show(string.Format(Strings.MultipleChatLogs, Data.serverIPs[0], Data.serverIPs[1]), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                catch
                                {
                                    MessageBox.Show(Strings.FileDeleteError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);


                return string.Empty;
            }
        }

        private void Parsed_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text))
            {
                Counter.Text = string.Format(Strings.CharacterCounter, 0, 0);
                return;
            }

            Counter.Text = string.Format(Strings.CharacterCounter, Parsed.Text.Length, Parsed.Text.Split('\n').Length);
        }

        private void SaveParsed_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Parsed.Text))
                {
                    MessageBox.Show(Strings.NothingParsed, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog.FileName = "chatlog.txt";
                SaveFileDialog.Filter = "Text File | *.txt";

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(SaveFileDialog.OpenFile()))
                    {
                        sw.Write(Parsed.Text.Replace("\n", Environment.NewLine));
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.SaveError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text))
                MessageBox.Show(Strings.NothingParsed, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        private void CheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckForUpdatesOnStartup.Checked)
                TryCheckingForUpdates();
        }

        private static string previousLog = string.Empty;
        private void RemoveTimestamps_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text) || string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\") || !File.Exists(FolderPath.Text + Data.logLocation))
                return;

            if (RemoveTimestamps.Checked)
            {
                previousLog = Parsed.Text;
                Parsed.Text = Regex.Replace(previousLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);
            }
            else if (!string.IsNullOrWhiteSpace(previousLog))
                Parsed.Text = previousLog;
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryCheckingForUpdates(true);
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
                    if (!allowFormDisplay)
                        ResumeTrayStripMenuItem_Click(this, EventArgs.Empty);

                    if (MessageBox.Show(string.Format(Strings.UpdateAvailable, installedVersion, currentVersion), Strings.UpdateAvailableTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        System.Diagnostics.Process.Start("https://github.com/MapleToo/GTAW-Log-Parser/releases");
                }
                else if (manual)
                    MessageBox.Show(string.Format(Strings.RunningLatest, installedVersion), Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                if (manual)
                    MessageBox.Show(string.Format(Strings.NoInternet, Properties.Settings.Default.Version), Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static BackupSettings backupSettings;
        private void AutomaticBackupSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPathBackup, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Properties.Settings.Default.BackupChatLogAutomatically)
            {
                if (MessageBox.Show(Strings.BackupWillBeOff, Strings.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                StatusLabel.Text = string.Format(Strings.BackupStatus, Strings.Disabled);
            }
            else
                MessageBox.Show(Strings.SettingsAfterClose, Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);

            BackupHandler.AbortAll();
            SaveSettings();

            if (backupSettings == null)
            {
                backupSettings = new BackupSettings();
                backupSettings.FormClosed += (s, args) =>
                {
                    BackupHandler.Initialize();
                    StatusLabel.Text = string.Format(Strings.BackupStatus, Properties.Settings.Default.BackupChatLogAutomatically ? Strings.Enabled : Strings.Disabled);
                };
            }
            else
            {
                backupSettings.LoadSettings();

                backupSettings.WindowState = FormWindowState.Normal;
                backupSettings.BringToFront();
            }

            backupSettings.ShowDialog();
        }

        private static ChatLogFilter chatLogFilter;
        private void FilterChatLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPathFilter, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveSettings();

            if (chatLogFilter == null)
            {
                chatLogFilter = new ChatLogFilter();
            }
            else
            {
                chatLogFilter.Initialize();

                chatLogFilter.WindowState = FormWindowState.Normal;
                chatLogFilter.BringToFront();
            }

            chatLogFilter.ShowDialog();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format(Strings.About, Properties.Settings.Default.Version), Strings.Information, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://github.com/MapleToo/GTAW-Log-Parser");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Logo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.OpenDocumentation, Strings.Information, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://forum.gta.world/en/index.php?/topic/7690-chat-logs/");
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.BackupChatLogAutomatically && TrayIcon.Visible == false)
            {
                DialogResult result = MessageBox.Show(Strings.MinimizeInsteadOfClose, Strings.Warning, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    e.Cancel = true;

                    Hide();
                    TrayIcon.Visible = true;

                    return;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            BackupHandler.quitting = true;
            SaveSettings();
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResumeTrayStripMenuItem_Click(this, EventArgs.Empty);
        }

        private void ResumeTrayStripMenuItem_Click(object sender, EventArgs e)
        {
            allowFormDisplay = true;

            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            TrayIcon.Visible = false;
        }

        private void ExitTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!allowFormDisplay)
                BackupHandler.quitting = true;

            System.Windows.Forms.Application.Exit();
        }
    }
}
