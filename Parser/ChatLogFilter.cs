using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parser
{
    public partial class ChatLogFilter : Form
    {
        public string ChatLog
        {
            get { return _chatLog; }
            set
            {
                _chatLog = value;
                chatLogLoaded = _chatLog != string.Empty;
                loadedFrom = chatLogLoaded ? loadedFrom : LoadedFrom.None;
                StatusLabel.Text = $"Chat log{(chatLogLoaded ? " " : " not ")}loaded{(chatLogLoaded ? $" at {DateTime.Now.ToString("HH:mm:ss")}" : "")}";
                StatusLabel.ForeColor = chatLogLoaded ? Color.Green : Color.Red;
            }
        }

        enum LoadedFrom { None, Unparsed, Parsed };
        private LoadedFrom loadedFrom = LoadedFrom.None;

        private string _chatLog;
        private bool chatLogLoaded;

        public ChatLogFilter()
        {
            InitializeComponent();

            TimeLabel.Text = "Current time: " + DateTime.Now.ToString("HH:mm:ss");

            Words.Text = Properties.Settings.Default.FilterNames;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestampsFromFilter;
        }

        public void Initialize()
        {
            ActiveControl = LoadUnparsed;
            Filtered.Text = ChatLog = string.Empty;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = "Current time: " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void LoadUnparsed_Click(object sender, EventArgs e)
        {
            ChatLog = Main.ParseChatLog(Properties.Settings.Default.FolderPath, false, showError: true);

            loadedFrom = ChatLog == string.Empty ? LoadedFrom.None : LoadedFrom.Unparsed;

            if (chatLogLoaded)
            {
                if (GetWordsToFilterIn().Count > 0)
                    TryToFilter(fastFilter: true);
                else
                {
                    string chatLog = previousLog = ChatLog;

                    if (RemoveTimestamps.Checked)
                        chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                    Filtered.Text = chatLog;
                }
            }
        }

        private void BrowseForParsed_Click(object sender, EventArgs e)
        {
            try
            {
                ChatLog = Filtered.Text = string.Empty;

                OpenFileDialog.InitialDirectory = string.IsNullOrWhiteSpace(Properties.Settings.Default.BackupPath) ? Path.GetPathRoot(Environment.SystemDirectory) : Properties.Settings.Default.BackupPath;
                OpenFileDialog.Filter = "Text File | *.txt";

                DialogResult result = OpenFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(OpenFileDialog.FileName))
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

                        if (RemoveTimestamps.Checked)
                            chatLog = Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                        Filtered.Text = chatLog;
                    }
                }
            }
            catch
            {
                ChatLog = Filtered.Text = string.Empty;

                MessageBox.Show("An error occured while reading the selected file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string previousLog = string.Empty;
        private void RemoveTimestamps_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Filtered.Text))
                return;

            if (RemoveTimestamps.Checked)
            {
                previousLog = Filtered.Text;
                Filtered.Text = Regex.Replace(previousLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);
            }
            else if (!string.IsNullOrWhiteSpace(previousLog))
                Filtered.Text = previousLog;
            else
                TryToFilter(fastFilter: true);
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            TryToFilter();
        }

        private void TryToFilter(bool fastFilter = false)
        {
            if (!chatLogLoaded)
            {
                MessageBox.Show("You haven't loaded a chat log yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> wordsToCheck = GetWordsToFilterIn();
            if (wordsToCheck.Count == 0 && !string.IsNullOrWhiteSpace(Words.Text))
            {
                MessageBox.Show("You can only have one word, number, or valid name pair on each line if you want to filter your chat log.\nExample: Boat, $500, John, John Doe or John_Doe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (RemoveTimestamps.Checked)
                    filtered = Regex.Replace(filtered, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                Filtered.Text = filtered;
            }
            else
            {
                previousLog = logToCheck;

                if (RemoveTimestamps.Checked)
                    logToCheck = Regex.Replace(logToCheck, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                Filtered.Text = logToCheck;

                if (!fastFilter)
                    MessageBox.Show("No matches found.\n\nMake sure you only have one word, number, or valid name pair on each line.\nExample: Boat, $500, John, John Doe or John_Doe", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (skippedWord)
                MessageBox.Show("One or more words were skipped during the filtering operation because they are not in a valid format.\n\nMake sure you only have one word, number, or valid name pair on each line.\nExample: Boat, $500, John, John Doe or John_Doe", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void SaveFiltered_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Filtered.Text))
                {
                    MessageBox.Show("You haven't filtered anything yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog.FileName = "filtered_chatlog.txt";
                SaveFileDialog.Filter = "Text File | *.txt";

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(SaveFileDialog.OpenFile()))
                    {
                        sw.Write(Filtered.Text.Replace("\n", Environment.NewLine));
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occured while trying to save the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFilteredToClipboard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Filtered.Text))
                MessageBox.Show("You haven't filtered anything yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Clipboard.SetText(Filtered.Text.Replace("\n", Environment.NewLine));
        }

        private void ChatLogFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.FilterNames = Words.Text;
            Properties.Settings.Default.RemoveTimestampsFromFilter = RemoveTimestamps.Checked;

            Properties.Settings.Default.Save();
        }
    }
}
