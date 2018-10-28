using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

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
                chatLogLoaded = _chatLog != "";
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

            Names.Text = Properties.Settings.Default.FilterNames;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestampsFromFilter;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = "Current time: " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void LoadUnparsed_Click(object sender, EventArgs e)
        {
            ChatLog = "";

            ChatLog = Main.ParseChatLog(Properties.Settings.Default.FolderPath, false, showError: true);

            loadedFrom = ChatLog == "" ? LoadedFrom.None : LoadedFrom.Unparsed;
        }

        private void BrowseForParsed_Click(object sender, EventArgs e)
        {
            ChatLog = "";

            OpenFileDialog.InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory);
            OpenFileDialog.Filter = "Text File | *.txt";

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(OpenFileDialog.FileName))
                {
                    ChatLog = sr.ReadToEnd();
                }

                loadedFrom = LoadedFrom.Parsed;
                Filter.Focus();
            }
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            Filtered.Text = "";

            if (!chatLogLoaded)
            {
                MessageBox.Show("You haven't loaded a chat log yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Names.Text.Length == 0 || string.IsNullOrWhiteSpace(Names.Text))
            {
                string parsed = ChatLog;

                if (RemoveTimestamps.Checked && loadedFrom != LoadedFrom.Unparsed)
                {
                    parsed = System.Text.RegularExpressions.Regex.Replace(parsed, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", "");
                    Filtered.Text = parsed;
                }
                else
                    MessageBox.Show("Please choose at least one valid name to filter into your new chat log.\n\nExample: John Doe or John_Doe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            List<string> namesToCheck = GetDesiredNames();

            if (namesToCheck.Count == 0)
            {
                MessageBox.Show("Please choose at least one valid name to filter into your new chat log.\n\nExample: John Doe or John_Doe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string chatLog = ChatLog;

            if (RemoveTimestamps.Checked)
                chatLog = System.Text.RegularExpressions.Regex.Replace(chatLog, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", "");

            string[] lines = chatLog.Split('\n');
            string filtered = "";

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                foreach (string name in namesToCheck)
                {
                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    if (line.ToLower().Contains(name.ToLower()))
                        filtered += (line + "\n");
                }
            }

            if (filtered.Length > 0)
            {
                filtered = filtered.TrimEnd(new char[] { '\r', '\n' });
                Filtered.Text = filtered;
            }
            else
                MessageBox.Show("No matches found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<string> GetDesiredNames()
        {
            string names = Names.Text;
            string[] lines = names.Split('\n');

            List<string> finalNames = new List<string>();

            foreach (string line in lines)
            {
                if (line.Any(char.IsDigit) || (line.Any(char.IsSymbol) && !line.Contains("'")))
                    continue;

                string[] name = line.Split(new char[] { ' ', '_' });

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
            if (Filtered.Text.Length == 0)
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

        private void CopyFilteredToClipboard_Click(object sender, EventArgs e)
        {
            if (Filtered.Text.Length == 0)
                MessageBox.Show("You haven't filtered anything yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Clipboard.SetText(Filtered.Text.Replace("\n", Environment.NewLine));
        }

        private void ChatLogFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.FilterNames = Names.Text;
            Properties.Settings.Default.RemoveTimestampsFromFilter = RemoveTimestamps.Checked;

            Properties.Settings.Default.Save();
        }
    }
}
