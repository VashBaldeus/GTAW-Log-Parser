using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Parser
{
    public partial class BackupSettings : Form
    {
        public BackupSettings()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.BackupPath = BackupPath.Text;

            Properties.Settings.Default.BackupChatLogAutomatically = BackUpChatLogAutomatically.Checked;
            Properties.Settings.Default.EnableIntervalBackup = EnableIntervalBackup.Checked;
            Properties.Settings.Default.IntervalTime = (int)Interval.Value;
            Properties.Settings.Default.RemoveTimestampsFromBackup = RemoveTimestamps.Checked;
            Properties.Settings.Default.StartWithWindows = StartWithWindows.Checked;

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            BackupPath.Text = Properties.Settings.Default.BackupPath;

            BackUpChatLogAutomatically.Checked = Properties.Settings.Default.BackupChatLogAutomatically;
            EnableIntervalBackup.Checked = Properties.Settings.Default.EnableIntervalBackup;
            Interval.Value = Properties.Settings.Default.IntervalTime;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestampsFromBackup;
            StartWithWindows.Checked = Properties.Settings.Default.StartWithWindows;
        }

        public static void ResetSettings()
        {
            Properties.Settings.Default.BackupPath = string.Empty;

            Properties.Settings.Default.BackupChatLogAutomatically = false;
            Properties.Settings.Default.EnableIntervalBackup = false;
            Properties.Settings.Default.IntervalTime = 10;
            Properties.Settings.Default.RemoveTimestampsFromBackup = false;
            Properties.Settings.Default.StartWithWindows = false;

            Properties.Settings.Default.Save();
        }

        private void BackupPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void BackupPath_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.BackupPath))
                return;

            try
            {
                DirectoryInfo directory = new DirectoryInfo(Properties.Settings.Default.BackupPath);
                FileInfo[] allTextFilesInDirectory = directory.GetFiles("*.txt");

                List<FileInfo> chatLogFiles = new List<FileInfo>();

                foreach (FileInfo file in allTextFilesInDirectory)
                {
                    if (Regex.IsMatch(file.Name, @"\d{1,2}.[A-Za-z]{3}.\d{4}-\d{1,2}.\d{1,2}.\d{1,2}"))
                        chatLogFiles.Add(file);
                }

                if (chatLogFiles.Count > 0)
                {
                    if (MessageBox.Show("Would you like to move all of your existing backups to the new folder?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        foreach (FileInfo file in chatLogFiles)
                        {
                            File.Move(file.FullName, BackupPath.Text + file.Name);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occurent while moving the chat log files to the new directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BackupPath.Text))
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
                        BackupPath.Text = dialog.FileName + "\\";
                        validLocation = true;
                    }
                    else
                        MessageBox.Show("Please pick a non-root directory for your backup folder location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    validLocation = true;
            }
        }

        private void BackUpChatLogAutomatically_CheckedChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Enabled = BackUpChatLogAutomatically.Checked;
            RemoveTimestamps.Enabled = BackUpChatLogAutomatically.Checked;
            StartWithWindows.Enabled = BackUpChatLogAutomatically.Checked;

            if (!BackUpChatLogAutomatically.Checked)
            {
                StartWithWindows.Checked = false;
                RemoveTimestamps.Checked = false;
                EnableIntervalBackup.Checked = false;
            }
        }

        private void EnableIntervalBackup_CheckedChanged(object sender, EventArgs e)
        {
            Interval.Enabled = EnableIntervalBackup.Checked;
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Text = $"Back up the chat log automatically while the game is running (every {Interval.Value} minutes)";
        }

        private void StartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (StartWithWindows.Checked && !StartupHandler.IsAddedToStartup())
                MessageBox.Show("This feature will stop working if you delete or move the parser to a different location.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetSettings();
            LoadSettings();
        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BackupSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackUpChatLogAutomatically.Checked && (string.IsNullOrWhiteSpace(BackupPath.Text) || !Directory.Exists(BackupPath.Text)))
            {
                e.Cancel = true;
                MessageBox.Show("Please choose a valid backup location or turn automatic backup off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if ((StartWithWindows.Checked && !StartupHandler.IsAddedToStartup()) || (!StartWithWindows.Checked && StartupHandler.IsAddedToStartup()))
                StartupHandler.ToggleStartup(StartWithWindows.Checked);

            SaveSettings();
        }
    }
}
