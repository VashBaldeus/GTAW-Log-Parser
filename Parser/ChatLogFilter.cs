using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

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

            if (loadedFrom != LoadedFrom.None)
            {
                if (GetWordsToFilterIn().Count > 0)
                    TryToFilter(fastFilter: true);
                else
                    Filtered.Text = ChatLog;
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

                if (loadedFrom != LoadedFrom.None)
                {
                    if (GetWordsToFilterIn().Count > 0)
                        TryToFilter(fastFilter: true);
                    else
                        Filtered.Text = ChatLog;
                }
            }
            catch
            {
                ChatLog = Filtered.Text = string.Empty;

                MessageBox.Show("An error occured while reading the selected file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveTimestamps_CheckedChanged(object sender, EventArgs e)
        {
            if (loadedFrom != LoadedFrom.None && (string.IsNullOrWhiteSpace(Words.Text) || GetWordsToFilterIn().Count > 0))
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

            string chatLog = Filtered.Text = ChatLog;

            if (RemoveTimestamps.Checked)
                chatLog = System.Text.RegularExpressions.Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

            if (string.IsNullOrWhiteSpace(Words.Text))
            {
                Filtered.Text = chatLog;
                return;
            }

            List<string> wordsToCheck = GetWordsToFilterIn();
            if (wordsToCheck.Count == 0 && !string.IsNullOrWhiteSpace(Words.Text))
            {
                MessageBox.Show("Please choose at least one word, number or valid name pair PER LINE to filter into your new chat log.\n\nExample: Boat, $500, John, John Doe or John_Doe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = chatLog.Split('\n');
            string filtered = string.Empty;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                foreach (string name in wordsToCheck)
                {
                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    if (line.ToLower().Contains(name.ToLower()))
                    {
                        filtered += line + "\n";
                        break;
                    }
                }
            }

            if (filtered.Length > 0)
            {
                filtered = filtered.TrimEnd(new char[] { '\r', '\n' });
                Filtered.Text = filtered;
            }
            else
            {
                Filtered.Text = chatLog;

                if (!fastFilter)
                    MessageBox.Show("No matches found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private List<string> GetWordsToFilterIn()
        {
            string names = Words.Text;
            string[] lines = names.Split('\n');

            List<string> finalNames = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string newLine = line.Trim();

                string[] name = newLine.Split(new char[] { ' ', '_' });

                if (name.Length == 2)
                {
                    if (string.IsNullOrWhiteSpace(name[0]) || string.IsNullOrWhiteSpace(name[1]))
                        continue;

                    finalNames.Add($"{name[0]} {name[1]}");
                    finalNames.Add($"{name[0]}_{name[1]}");
                }
                else if (name.Length == 1 && !string.IsNullOrWhiteSpace(name[0]))
                    finalNames.Add(name[0]);
            }

            return finalNames;
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
