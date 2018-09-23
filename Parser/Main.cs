using System;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

using Octokit;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Parser
{
    public partial class Main : Form
    {
        private static GitHubClient client = new GitHubClient(new ProductHeaderValue("GTAW-Log-Parser"));

        //private Thread parseThread;
        //private Thread updateThread;
        //private Thread saveThread;

        public Main()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FolderPath = FolderPath.Text;
            Properties.Settings.Default.RemoveTimestamps = RemoveTimestamps.Checked;
            Properties.Settings.Default.AutomaticallyCheckForUpdates = CheckForUpdatesOnStartup.Checked;

            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Version.Text = $"Version: {Properties.Settings.Default.Version}";

            FolderPath.Text = Properties.Settings.Default.FolderPath;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestamps;

            CheckForUpdatesOnStartup.Checked = Properties.Settings.Default.AutomaticallyCheckForUpdates;
        }

        private void FolderPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (FolderPath.Text.Length == 0)
            {
                Browse_Click(this, EventArgs.Empty);
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory),
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath.Text = dialog.FileName;
                Parse.Focus();
            }
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            if (FolderPath.Text.Length == 0)
            {
                MessageBox.Show("Invalid RAGEMP folder path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(FolderPath.Text + Data.logLogation))
            {
                MessageBox.Show("Can't find the GTA World chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ParseChatLog(FolderPath.Text);
        }

        private void ParseChatLog(string folderPath)
        {
            try
            {
                string log;

                using (StreamReader sr = new StreamReader(folderPath + Data.logLogation))
                {
                    log = sr.ReadToEnd();
                }

                log = log.Remove(0, 1);

                log = log.Replace("{nl}", "\n");
                log = Regex.Replace(log, "~[a-zA-Z]~", "");

                log = log.Remove(log.Length - 2, 2);

                if (RemoveTimestamps.Checked)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", "");

                Parsed.Text = log;
            }
            catch
            {
                MessageBox.Show("An error occured while parsing the chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveTimestamps_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void CheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();

            if (CheckForUpdatesOnStartup.Checked)
                CheckForUpdates();
        }

        private void CheckForUpdates(bool checking = false)
        {
            try
            {
                string installedVersion = Properties.Settings.Default.Version;

                string currentVersion = client.Repository.Release.GetAll("MapleToo", "GTAW-Log-Parser").Result[0].TagName;

                if (string.Compare(installedVersion, currentVersion) < 0)
                {
                    if (MessageBox.Show($"A new version of the chat log parser is now available on GitHub.\n\nInstalled Version: {installedVersion}\nAvailable Version: {currentVersion}\n\nWould you like to visit the releases page now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        System.Diagnostics.Process.Start("https://github.com/MapleToo/GTAW-Log-Parser/releases");
                }
                //else if (string.Compare(installedVersion, currentVersion) > 0 && checking)
                //{
                //    if (MessageBox.Show($"You are using a newer version of the chat log parser than is recommended. You may encounter unwated errors, continue at your own discretion.\n\nInstalled Version: {installedVersion}\nRecommended Version: {currentVersion}\n\nWould you like to visit the releases page now?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                //        System.Diagnostics.Process.Start("https://github.com/MapleToo/GTAW-Log-Parser/releases");
                //}
                else if (checking)
                    MessageBox.Show($"You are running the latest version of the chat log parser.\n\nInstalled Version: {installedVersion}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                if (checking)
                    MessageBox.Show($"No updates could be found, try checking your internet connection.\n\nInstalled Version: {Properties.Settings.Default.Version}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveParsed_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length == 0)
                return;

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

        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length != 0)
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates(true);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Chat Log Parser created by Maple for GTA World.\n\nInstalled Version: {Properties.Settings.Default.Version}\n\nWould you like to visit the repository page on GitHub?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://github.com/MapleToo/GTAW-Log-Parser");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Logo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Would you like to open the documentation page for the chat log parser found on the GTA World forums?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://forum.gta.world/en/index.php?/topic/11003-chat-logs/");
        }

        private void Parsed_TextChanged(object sender, EventArgs e)
        {
            if (Parsed.Text == "")
            {
                Counter.Text = "0 characters and 0 lines";
                return;
            }

            Counter.Text = Parsed.Text.Length + " characters and " + Parsed.Text.Split('\n').Length + " lines";
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
    }
}
