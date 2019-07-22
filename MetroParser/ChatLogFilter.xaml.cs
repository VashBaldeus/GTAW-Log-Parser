using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MetroParser.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for ChatLogFilter.xaml
    /// </summary>
    public partial class ChatLogFilterWindow
    {
        private readonly System.Windows.Threading.DispatcherTimer Timer;

        public string ChatLog
        {
            get { return _chatLog; }
            set
            {
                _chatLog = value;
                chatLogLoaded = _chatLog != string.Empty;
                loadedFrom = chatLogLoaded ? loadedFrom : LoadedFrom.None;
                StatusLabel.Content = string.Format(Strings.FilterLogStatus, chatLogLoaded ? "" : Strings.Negation, chatLogLoaded ? string.Format(Strings.LoadedAt, DateTime.Now.ToString("HH:mm:ss")) : "");
                StatusLabel.Foreground = chatLogLoaded ? Brushes.Green: Brushes.Red;
            }
        }

        enum LoadedFrom { None, Unparsed, Parsed };
        private LoadedFrom loadedFrom = LoadedFrom.None;

        private string _chatLog;
        private bool chatLogLoaded;

        public ChatLogFilterWindow()
        {
            InitializeComponent();

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            Timer.Start();

            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));

            MainWindow.SetText(Words, Properties.Settings.Default.FilterNames);
            RemoveTimestamps.IsChecked = Properties.Settings.Default.RemoveTimestampsFromFilter;
        }

        public void Initialize()
        {
            LoadUnparsed.Focus();
            MainWindow.SetText(Filtered, string.Empty);
            ChatLog = string.Empty;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));
        }

        private void LoadUnparsed_Click(object sender, RoutedEventArgs e)
        {
            ChatLog = MainWindow.ParseChatLog(Properties.Settings.Default.FolderPath, false, showError: true);

            loadedFrom = ChatLog == string.Empty ? LoadedFrom.None : LoadedFrom.Unparsed;

            if (chatLogLoaded)
            {
                if (GetWordsToFilterIn().Count > 0)
                    TryToFilter(fastFilter: true);
                else
                {
                    string chatLog = previousLog = ChatLog;

                    if (RemoveTimestamps.IsChecked == true)
                        chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                    MainWindow.SetText(Filtered, chatLog);
                }
            }
        }

        private void BrowseForParsed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChatLog = string.Empty;
                MainWindow.SetText(Filtered, string.Empty);

                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
                {
                    InitialDirectory = string.IsNullOrWhiteSpace(Properties.Settings.Default.BackupPath) ? System.IO.Path.GetPathRoot(Environment.SystemDirectory) : Properties.Settings.Default.BackupPath,
                    Filter = "Text File | *.txt"
                };

                if (dialog.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(dialog.FileName))
                    {
                        ChatLog = sr.ReadToEnd();
                    }
                }

                loadedFrom = ChatLog == string.Empty ? LoadedFrom.None : LoadedFrom.Parsed;

                if (chatLogLoaded)
                {
                    if (GetWordsToFilterIn().Count > 0)
                        TryToFilter(fastFilter: true);
                    else
                    {
                        string chatLog = previousLog = ChatLog;

                        if (RemoveTimestamps.IsChecked == true)
                            chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                        MainWindow.SetText(Filtered, chatLog);
                    }
                }
            }
            catch
            {
                ChatLog = string.Empty;
                MainWindow.SetText(Filtered, string.Empty);

                MessageBox.Show(Strings.FilterReadError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string previousLog = string.Empty;
        private void RemoveTimestamps_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MainWindow.GetText(Filtered)))
                return;

            if (RemoveTimestamps.IsChecked == true)
            {
                previousLog = MainWindow.GetText(Filtered);
                MainWindow.SetText(Filtered, Regex.Replace(previousLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty));
            }
            else if (!string.IsNullOrWhiteSpace(previousLog))
                MainWindow.SetText(Filtered, previousLog);
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
            if (wordsToCheck.Count == 0 && !string.IsNullOrWhiteSpace(MainWindow.GetText(Words)))
            {
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

                    // ONE: Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty).ToLower().Contains(word.ToLower())
                    // TWO: Regex.IsMatch(Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty), $"\\b{word}\\b", RegexOptions.IgnoreCase)
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

                MainWindow.SetText(Filtered, filtered);
            }
            else
            {
                previousLog = logToCheck;

                if (RemoveTimestamps.IsChecked == true)
                    logToCheck = Regex.Replace(logToCheck, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                MainWindow.SetText(Filtered, logToCheck);

                if (!fastFilter)
                    MessageBox.Show(Strings.FilterHintNoMatches, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (skippedWord)
                MessageBox.Show(Strings.FilterHintSkipped, Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool skippedWord = false;
        private List<string> GetWordsToFilterIn()
        {
            skippedWord = false;
            string words = MainWindow.GetText(Words);
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
                if (string.IsNullOrWhiteSpace(MainWindow.GetText(Filtered)))
                {
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
                        sw.Write(MainWindow.GetText(Filtered).Replace("\n", Environment.NewLine));
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
            if (string.IsNullOrWhiteSpace(MainWindow.GetText(Filtered)))
                MessageBox.Show(Strings.NothingFiltered, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Clipboard.SetText(MainWindow.GetText(Filtered).Replace("\n", Environment.NewLine));
        }

        private void ChatLogFilter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.FilterNames = MainWindow.GetText(Words);
            Properties.Settings.Default.RemoveTimestampsFromFilter = RemoveTimestamps.IsChecked == true;

            Properties.Settings.Default.Save();
        }
    }
}
