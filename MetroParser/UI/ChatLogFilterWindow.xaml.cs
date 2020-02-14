using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MetroParser.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Input;

namespace MetroParser.UI
{
    /// <summary>
    /// Interaction logic for ChatLogFilterWindow.xaml
    /// </summary>
    public partial class ChatLogFilterWindow
    {
        private readonly MainWindow _mainWindow;
        private readonly System.Windows.Threading.DispatcherTimer Timer;
        private bool usingAdvancedFilter = false;
        private bool isLoading = true;

        private readonly Dictionary<string, Tuple<string, bool>> filterCriteria = new Dictionary<string, Tuple<string, bool>>
        {
            // Filter, regex pattern, isEnabled (false = remove from log)
            { "OOC", Tuple.Create(@"^\(\( \(\d*\) [A-Za-z]+( [A-Za-z]+){0,1}:.*?\)\)$", Properties.Settings.Default.OOCCriterionEnabled) },
            { "IC", Tuple.Create(@"^[A-Za-z]+( [A-Za-z]+){0,1} (says|shouts)( \[low\]){0,1}:.*$", Properties.Settings.Default.ICCriterionEnabled) }
        };

        private bool OtherEnabled
        {
            get { return Other.IsChecked == true; }
        }

        private string ChatLog
        {
            get { return _chatLog; }
            set
            {
                _chatLog = value;
                chatLogLoaded = !string.IsNullOrEmpty(_chatLog);
                StatusLabel.Content = string.Format(Strings.FilterLogStatus, chatLogLoaded ? string.Empty : Strings.Negation, chatLogLoaded ? string.Format(Strings.LoadedAt, DateTime.Now.ToString("HH:mm:ss")) : string.Empty);
                StatusLabel.Foreground = chatLogLoaded ? Brushes.Green: Brushes.Red;
            }
        }

        private string _chatLog;
        private bool chatLogLoaded;

        private void GainFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Focus();
        }

        public ChatLogFilterWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _mainWindow.GotKeyboardFocus += GainFocus;
            InitializeComponent();

            Left = _mainWindow.Left + (_mainWindow.Width / 2 - Width / 2);
            Top = _mainWindow.Top + (_mainWindow.Height / 2 - Height / 2);

            LoadSettings();
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Start();
            isLoading = false;
        }

        private void LoadSettings()
        {
            LoadUnparsed.Focus();
            Filtered.Text = ChatLog = string.Empty;

            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));
            Words.Text = Properties.Settings.Default.FilterNames;

            OOC.IsChecked = Properties.Settings.Default.OOCCriterionEnabled;
            IC.IsChecked = Properties.Settings.Default.ICCriterionEnabled;
            Other.IsChecked = Properties.Settings.Default.OtherCriterionEnabled;
            RemoveTimestamps.IsChecked = Properties.Settings.Default.RemoveTimestampsFromFilter;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FilterNames = Words.Text;

            Properties.Settings.Default.OOCCriterionEnabled = OOC.IsChecked == true;
            Properties.Settings.Default.ICCriterionEnabled = IC.IsChecked == true;
            Properties.Settings.Default.OtherCriterionEnabled = Other.IsChecked == true;
            Properties.Settings.Default.RemoveTimestampsFromFilter = RemoveTimestamps.IsChecked == true;

            Properties.Settings.Default.Save();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = string.Format(Strings.CurrentTime, DateTime.Now.ToString("HH:mm:ss"));
        }

        private void LoadUnparsed_Click(object sender, RoutedEventArgs e)
        {
            ChatLog = MainWindow.ParseChatLog(folderPath: Properties.Settings.Default.FolderPath, removeTimestamps: false, showError: true);
            
            if (chatLogLoaded)
                TryToFilter(fastFilter: true);
                //Filtered.Text = ChatLog;
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
                    using (StreamReader streamReader = new StreamReader(dialog.FileName))
                    {
                        ChatLog = streamReader.ReadToEnd();
                    }
                }

                if (chatLogLoaded)
                    TryToFilter(fastFilter: true);
                    //Filtered.Text = ChatLog;
            }
            catch
            {
                ChatLog = Filtered.Text = string.Empty;
                MessageBox.Show(Strings.FilterReadError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveTimestamps_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (chatLogLoaded)
                TryToFilter(fastFilter: true);
        }

        private void Criterion_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (isLoading)
                return;

            string criterionName;
            try
            {
                criterionName = ((System.Windows.Controls.CheckBox)sender).Name;
            }
            catch
            {
                criterionName = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(criterionName))
                return;

            KeyValuePair<string, Tuple<string, bool>>? entry = filterCriteria.Where(pair => pair.Key == criterionName)
                            .Select(pair => (KeyValuePair<string, Tuple<string, bool>>?)pair)
                            .FirstOrDefault();

            if (entry != null)
                filterCriteria[entry.Value.Key] = Tuple.Create(filterCriteria[entry.Value.Key].Item1, !filterCriteria[entry.Value.Key].Item2);
            
            if (chatLogLoaded)
                TryToFilter(fastFilter: true);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            TryToFilter(fastFilter: false);
        }

        private void TryToFilter(bool fastFilter = false)
        {
            if (!chatLogLoaded)
            {
                MessageBox.Show(Strings.NoChatLogLoaded, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            skippedWord = false;
            string logToCheck = ChatLog;
            string[] lines = logToCheck.Split('\n');
            string filtered = string.Empty;

            if (!usingAdvancedFilter)
            {
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    bool isCriterionEnabled = false;
                    bool matchedRegularCriterion = false;
                    foreach (KeyValuePair<string, Tuple<string, bool>> keyValuePair in filterCriteria)
                    {
                        if (string.IsNullOrWhiteSpace(keyValuePair.Key) || string.IsNullOrWhiteSpace(keyValuePair.Value.Item1))
                            continue;

                        if (Regex.IsMatch(Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty), keyValuePair.Value.Item1, RegexOptions.IgnoreCase))
                        {
                            matchedRegularCriterion = true;

                            if (keyValuePair.Value.Item2 == true)
                            {
                                isCriterionEnabled = true;
                                break;
                            }
                        }
                    }

                    if (isCriterionEnabled || (!matchedRegularCriterion && OtherEnabled))
                        filtered += line + "\n";

                    // Next line
                }
            }
            else
            {
                List<string> wordsToCheck = GetWordsToFilterIn();
                if (wordsToCheck.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(Words.Text))
                        MessageBox.Show(Strings.FilterHint, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show(Strings.NoWordsToFilter, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

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
                            // If word found on line (advanced filter)
                            // also check simple filter
                            bool isCriterionEnabled = false;
                            bool matchedRegularCriterion = false;
                            foreach (KeyValuePair<string, Tuple<string, bool>> keyValuePair in filterCriteria)
                            {
                                if (string.IsNullOrWhiteSpace(keyValuePair.Key) || string.IsNullOrWhiteSpace(keyValuePair.Value.Item1))
                                    continue;

                                if (Regex.IsMatch(Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty), keyValuePair.Value.Item1, RegexOptions.IgnoreCase))
                                {
                                    matchedRegularCriterion = true;

                                    if (keyValuePair.Value.Item2 == true)
                                    {
                                        isCriterionEnabled = true;
                                        break;
                                    }
                                }
                            }

                            if (isCriterionEnabled || (!matchedRegularCriterion && OtherEnabled))
                                filtered += line + "\n";

                            // Next line
                            break;
                        }

                        // Next word
                    }

                    // Next line
                }
            }

            if (filtered.Length > 0)
            {
                filtered = filtered.TrimEnd(new char[] { '\r', '\n' });

                if (RemoveTimestamps.IsChecked == true)
                    filtered = Regex.Replace(filtered, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                Filtered.Text = filtered;
            }
            else
            {
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

            if (usingAdvancedFilter && skippedWord && !Properties.Settings.Default.DisableInformationPopups)
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

        private void FilterModeToggle_Click(object sender, RoutedEventArgs e)
        {
            usingAdvancedFilter = !usingAdvancedFilter;

            FilterModeToggle.Content = usingAdvancedFilter ? Strings.SimpleFilter : Strings.AdvancedFilter;
            Width = usingAdvancedFilter ? 656 : 494;
        }

        private void ChatLogFilter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            _mainWindow.GotKeyboardFocus -= GainFocus;
        }

        private void Words_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!usingAdvancedFilter)
                LoadUnparsed.Focus();
        }

        private void Filter_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!usingAdvancedFilter)
                LoadUnparsed.Focus();
        }
    }
}
