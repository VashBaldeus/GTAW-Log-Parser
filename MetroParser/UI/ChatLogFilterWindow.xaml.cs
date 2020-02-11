using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MetroParser.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MetroParser.UI
{
    /// <summary>
    /// Interaction logic for ChatLogFilterWindow.xaml
    /// </summary>
    public partial class ChatLogFilterWindow
    {
        private readonly MainWindow _mainWindow;
        private readonly System.Windows.Threading.DispatcherTimer Timer;
        private bool advancedFilter = true;

        public string ChatLog
        {
            get { return _chatLog; }
            set
            {
                _chatLog = value;
                chatLogLoaded = !string.IsNullOrEmpty(_chatLog);
                loadedFrom = chatLogLoaded ? loadedFrom : LoadedFrom.None;
                StatusLabel.Content = string.Format(Strings.FilterLogStatus, chatLogLoaded ? "" : Strings.Negation, chatLogLoaded ? string.Format(Strings.LoadedAt, DateTime.Now.ToString("HH:mm:ss")) : "");
                StatusLabel.Foreground = chatLogLoaded ? Brushes.Green: Brushes.Red;
            }
        }

        enum LoadedFrom { None, Unparsed, Parsed };
        private LoadedFrom loadedFrom = LoadedFrom.None;

        private string _chatLog;
        private bool chatLogLoaded;

        public ChatLogFilterWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            InitializeComponent();

            Left = _mainWindow.Left + (_mainWindow.Width / 2 - Width / 2);
            Top = _mainWindow.Top + (_mainWindow.Height / 2 - Height / 2);

            Initialize();

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Start();

            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));

            Words.Text = Properties.Settings.Default.FilterNames;
            RemoveTimestamps.IsChecked = Properties.Settings.Default.RemoveTimestampsFromFilter;
        }

        private void Initialize()
        {
            LoadUnparsed.Focus();
            Filtered.Text = ChatLog = string.Empty;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));
        }

        private void LoadUnparsed_Click(object sender, RoutedEventArgs e)
        {
            ChatLog = MainWindow.ParseChatLog(folderPath: Properties.Settings.Default.FolderPath, removeTimestamps: false, showError: true);
            loadedFrom = string.IsNullOrEmpty(ChatLog) ? LoadedFrom.None : LoadedFrom.Unparsed;

            if (chatLogLoaded)
            {
                if (GetWordsToFilterIn().Count > 0)
                    TryToFilter(fastFilter: true);
                else
                {
                    string chatLog = previousLog = ChatLog;

                    if (RemoveTimestamps.IsChecked == true)
                        chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                    Filtered.Text = chatLog;
                }
            }
        }

        private void BrowseForParsed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChatLog = Filtered.Text = string.Empty;

                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
                {
                    InitialDirectory = string.IsNullOrWhiteSpace(Properties.Settings.Default.BackupPath) ? Path.GetPathRoot(Environment.SystemDirectory) : Properties.Settings.Default.BackupPath,
                    Filter = "Text File | *.txt"
                };

                if (dialog.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(dialog.FileName))
                    {
                        ChatLog = sr.ReadToEnd();
                    }
                }

                loadedFrom = string.IsNullOrEmpty(ChatLog) ? LoadedFrom.None : LoadedFrom.Parsed;

                if (chatLogLoaded)
                {
                    if (GetWordsToFilterIn().Count > 0)
                        TryToFilter(fastFilter: true);
                    else
                    {
                        string chatLog = previousLog = ChatLog;

                        if (RemoveTimestamps.IsChecked == true)
                            chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                        Filtered.Text = chatLog;
                    }
                }
            }
            catch
            {
                ChatLog = Filtered.Text = string.Empty;

                MessageBox.Show(Strings.FilterReadError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string previousLog = string.Empty;
        private void RemoveTimestamps_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Filtered.Text))
                return;

            if (RemoveTimestamps.IsChecked == true)
            {
                previousLog = Filtered.Text;
                Filtered.Text = Regex.Replace(previousLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);
            }
            else if (!string.IsNullOrWhiteSpace(previousLog))
                Filtered.Text = previousLog;
            else
                TryToFilter(fastFilter: true);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            TryToFilter();
        }

        private void TryToFilter(bool fastFilter = false)
        {
            if (!chatLogLoaded)
            {
                MessageBox.Show(Strings.NoChatLogLoaded, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> wordsToCheck = GetWordsToFilterIn();
            if (wordsToCheck.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(Words.Text))
                    MessageBox.Show(Strings.FilterHint, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            string logToCheck = ChatLog;

            string[] lines = logToCheck.Split('\n');
            string filtered = string.Empty;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                foreach (string word in wordsToCheck)
                {
                    if (string.IsNullOrWhiteSpace(word))
                        continue;

                    // ONE (substring): Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty).ToLower().Contains(word.ToLower())
                    // TWO (string): Regex.IsMatch(Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty), $"\\b{word}\\b", RegexOptions.IgnoreCase)
                    if (Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty).ToLower().Contains(word.ToLower()))
                    {
                        filtered += line + "\n";
                        break;
                    }
                }
            }

            if (filtered.Length > 0)
            {
                filtered = filtered.TrimEnd(new char[] { '\r', '\n' });
                previousLog = filtered;

                if (RemoveTimestamps.IsChecked == true)
                    filtered = Regex.Replace(filtered, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                Filtered.Text = filtered;
            }
            else
            {
                previousLog = logToCheck;

                if (RemoveTimestamps.IsChecked == true)
                    logToCheck = Regex.Replace(logToCheck, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                Filtered.Text = logToCheck;

                if (!fastFilter)
                {
                    if (!Properties.Settings.Default.DisableInformationPopups)
                        MessageBox.Show(Strings.FilterHintNoMatches, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        Filtered.Text = string.Empty;
                }
            }

            if (skippedWord && !Properties.Settings.Default.DisableInformationPopups)
                MessageBox.Show(Strings.FilterHintSkipped, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool skippedWord = false;
        private List<string> GetWordsToFilterIn()
        {
            skippedWord = false;
            string words = Words.Text;
            string[] lines = words.Split('\n');

            List<string> finalWords = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string newLine = line.Trim();

                string[] splitWord = newLine.Split(new char[] { ' ', '_' });

                if (splitWord.Length == 2)
                {
                    if (string.IsNullOrWhiteSpace(splitWord[0]) || string.IsNullOrWhiteSpace(splitWord[1]))
                        continue;

                    finalWords.Add($"{splitWord[0]} {splitWord[1]}");
                    finalWords.Add($"{splitWord[0]}_{splitWord[1]}");
                }
                else if (splitWord.Length == 1 && !string.IsNullOrWhiteSpace(splitWord[0]))
                    finalWords.Add(splitWord[0]);
                else
                    skippedWord = true;
            }

            return finalWords;
        }

        private void SaveFiltered_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Filtered.Text))
                {
                    if (!Properties.Settings.Default.DisableErrorPopups)
                        MessageBox.Show(Strings.NothingFiltered, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    return;
                }

                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "filtered_chatlog.txt",
                    Filter = "Text File | *.txt"
                };

                if (dialog.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(dialog.OpenFile()))
                    {
                        sw.Write(Filtered.Text.Replace("\n", Environment.NewLine));
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.SaveError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyFilteredToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Filtered.Text) && !Properties.Settings.Default.DisableErrorPopups)
                MessageBox.Show(Strings.NothingFiltered, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Clipboard.SetText(Filtered.Text.Replace("\n", Environment.NewLine));
        }

        private void AdvancedFilter_Click(object sender, RoutedEventArgs e)
        {
            advancedFilter = !advancedFilter;

            AdvancedFilter.Content = advancedFilter ? "Simple Filter" : "Advanced Filter";
            Width = advancedFilter ? 656 : 494;
        }

        private void ChatLogFilter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.FilterNames = Words.Text;
            Properties.Settings.Default.RemoveTimestampsFromFilter = RemoveTimestamps.IsChecked == true;

            Properties.Settings.Default.Save();
            Hide();
        }
    }
}
