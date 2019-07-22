using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for BackupSettings.xaml
    /// </summary>
    public partial class BackupSettings : Window
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
            Properties.Settings.Default.SuppressNotifications = SuppressNotifications.Checked;

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            ActiveControl = Browse;

            BackupPath.Text = Properties.Settings.Default.BackupPath;

            BackUpChatLogAutomatically.Checked = Properties.Settings.Default.BackupChatLogAutomatically;
            EnableIntervalBackup.Checked = Properties.Settings.Default.EnableIntervalBackup;
            Interval.Value = Properties.Settings.Default.IntervalTime;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestampsFromBackup;
            StartWithWindows.Checked = Properties.Settings.Default.StartWithWindows;
            SuppressNotifications.Checked = Properties.Settings.Default.SuppressNotifications;
        }

        public static void ResetSettings()
        {
            Properties.Settings.Default.BackupPath = string.Empty;

            Properties.Settings.Default.BackupChatLogAutomatically = false;
            Properties.Settings.Default.EnableIntervalBackup = false;
            Properties.Settings.Default.IntervalTime = 10;
            Properties.Settings.Default.RemoveTimestampsFromBackup = false;
            Properties.Settings.Default.StartWithWindows = false;
            Properties.Settings.Default.SuppressNotifications = false;

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
                DirectoryInfo[] directories = new DirectoryInfo(Properties.Settings.Default.BackupPath).GetDirectories();

                List<DirectoryInfo> finalDirectories = new List<DirectoryInfo>();

                foreach (DirectoryInfo directory in directories)
                {
                    if (Regex.IsMatch(directory.Name, @"20\d{2}"))
                        finalDirectories.Add(directory);
                }

                if (finalDirectories.Count > 0)
                {
                    if (MessageBox.Show(Strings.MoveBackups, Strings.Question, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<string> moved = new List<string>();
                        List<string> notMoved = new List<string>();

                        foreach (DirectoryInfo directory in finalDirectories)
                        {
                            if (!Directory.Exists(BackupPath.Text + directory.Name))
                            {
                                Directory.Move(directory.FullName, BackupPath.Text + directory.Name);
                                moved.Add(directory.Name);
                            }
                            else
                                notMoved.Add(directory.Name);
                        }

                        Properties.Settings.Default.BackupPath = BackupPath.Text;
                        Properties.Settings.Default.Save();

                        if (notMoved.Count > 0)
                            MessageBox.Show((moved.Count > 0 ? string.Format(Strings.PartialMoveWarning, string.Join(", ", moved)) : Strings.NothingMovedWarning) + string.Format(Strings.AlreadyExistingFoldersWarning, string.Join(", ", notMoved)), Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.BackupMoveError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show(Strings.BadBackupPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SuppressNotifications.Enabled = BackUpChatLogAutomatically.Checked;

            if (!BackUpChatLogAutomatically.Checked)
            {
                StartWithWindows.Checked = false;
                RemoveTimestamps.Checked = false;
                EnableIntervalBackup.Checked = false;
                SuppressNotifications.Checked = false;
            }
        }

        private void EnableIntervalBackup_CheckedChanged(object sender, EventArgs e)
        {
            Interval.Enabled = EnableIntervalBackup.Checked;
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            IntervalLabel2.Text = string.Format(Strings.Recommended, Interval.Value > 1 ? Strings.MinutePlural : Strings.MinuteSingular);
            EnableIntervalBackup.Text = string.Format(Strings.IntervalHint, Interval.Value, Interval.Value > 1 ? Strings.MinutePlural : Strings.MinutePlural);
        }

        private void StartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (StartWithWindows.Checked && !StartupHandler.IsAddedToStartup())
                MessageBox.Show(Strings.AutoStartWarning, Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(Strings.BadBackupPathSave, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if ((StartWithWindows.Checked && !StartupHandler.IsAddedToStartup()) || (!StartWithWindows.Checked && StartupHandler.IsAddedToStartup()))
                StartupHandler.ToggleStartup(StartWithWindows.Checked);

            SaveSettings();
        }
    }
}
}
