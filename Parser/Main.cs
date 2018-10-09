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
        private Thread updateThread;

        public Main()
        {
            InitializeComponent();

            LoadSettings();

            BackupHandler.Initialize();
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
            Version.Text = $"Version: {Properties.Settings.Default.Version}";
            StatusLabel.Text = $"Automatic Backup: {(Properties.Settings.Default.BackupChatLogAutomatically ? "ON" : "OFF")}";

            FolderPath.Text = Properties.Settings.Default.FolderPath;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestamps;
            CheckForUpdatesOnStartup.Checked = Properties.Settings.Default.CheckForUpdatesAutomatically;
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

                StatusLabel.Text = "Automatic Backup: OFF";
                MessageBox.Show("Automatic backup has been turned OFF, please set it up again if you wish to use it.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FolderPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (FolderPath.Text.Length == 0)
                Browse_Click(this, EventArgs.Empty);
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
                if (dialog.FileName[dialog.FileName.Length - 1] != '\\')
                    FolderPath.Text = dialog.FileName + "\\";
                else
                    FolderPath.Text = dialog.FileName;

                Parse.Focus();
            }
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            if (FolderPath.Text.Length == 0 || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show("Invalid RAGEMP folder path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(FolderPath.Text + Data.logLogation))
            {
                MessageBox.Show("Can't find the GTA World chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Parsed.Text = ParseChatLog(FolderPath.Text, RemoveTimestamps.Checked);
        }

        public static string ParseChatLog(string folderPath, bool removeTimestamps)
        {
            try
            {
                string log;

                using (StreamReader sr = new StreamReader(folderPath + Data.logLogation))
                {
                    log = sr.ReadToEnd();
                }

                log = log.Remove(0, 1);

                log = log.Replace("\\n", "\n");
                log = Regex.Replace(log, "~[a-zA-Z]~", "");

                log = log.Remove(log.Length - 2, 2);

                if (removeTimestamps)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", "");

                return log;
            }
            catch
            {
                MessageBox.Show("An error occured while parsing the chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
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

        private void SaveParsed_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length == 0)
            {
                MessageBox.Show("You haven't parsed anything yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length == 0)
                MessageBox.Show("You haven't parsed anything yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        private void CheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckForUpdatesOnStartup.Checked)
                TryCheckingForUpdates();
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
            else
                MessageBox.Show("Currently checking for updates, please wait for the process to finish to check again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CheckForUpdates(bool manual = false)
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
                else if (manual)
                    MessageBox.Show($"You are running the latest version of the chat log parser.\n\nInstalled Version: {installedVersion}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                if (manual)
                    MessageBox.Show($"No updates could be found, try checking your internet connection.\n\nInstalled Version: {Properties.Settings.Default.Version}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static BackupSettings backupSettings;
        private void AutomaticBackupSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderPath.Text.Length == 0 || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show("Please choose a valid RAGEMP folder location before trying to enable automatic backup.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Properties.Settings.Default.BackupChatLogAutomatically)
            {
                if (MessageBox.Show("The automatic backup function will be turned off while the settings dialog is open.\n\nWould you like to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                StatusLabel.Text = "Automatic Backup: OFF";
            }
            else
                MessageBox.Show("Settings will only be applied once you close the settings dialog.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BackupHandler.AbortAll();
            SaveSettings();

            if (backupSettings == null)
            {
                backupSettings = new BackupSettings();
                backupSettings.FormClosed += (s, args) =>
                {
                    BackupHandler.Initialize();
                    StatusLabel.Text = $"Automatic Backup: {(Properties.Settings.Default.BackupChatLogAutomatically ? "ON" : "OFF")}";
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
            if (MessageBox.Show("Would you like to open the documentation page for the chat log parser found on the GTA World forums?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://forum.gta.world/en/index.php?/topic/7690-chat-logs/");
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.BackupChatLogAutomatically && TrayIcon.Visible == false)
            {
                if (MessageBox.Show("Closing the parser will prevent the automatic backups from happening.\n\nWould you like to minimize the parser to the system tray instead?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    e.Cancel = true;

                    Hide();
                    TrayIcon.Visible = true;

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
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            TrayIcon.Visible = false;
        }

        private void ExitTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
