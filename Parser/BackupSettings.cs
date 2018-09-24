﻿using System;
using System.Windows.Forms;

using System.IO;

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

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            BackupPath.Text = Properties.Settings.Default.BackupPath;

            BackUpChatLogAutomatically.Checked = Properties.Settings.Default.BackupChatLogAutomatically;
            EnableIntervalBackup.Checked = Properties.Settings.Default.EnableIntervalBackup;
            Interval.Value = Properties.Settings.Default.IntervalTime;
        }

        public static void ResetSettings()
        {
            Properties.Settings.Default.BackupPath = "";

            Properties.Settings.Default.BackupChatLogAutomatically = false;
            Properties.Settings.Default.EnableIntervalBackup = false;
            Properties.Settings.Default.IntervalTime = 10;

            Properties.Settings.Default.Save();
        }

        private void BackupPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void BackupPath_TextChanged(object sender, EventArgs e)
        {
            //BackupHandler.AbortAll();

            // Ask to move current backups, if any, to new location

            //BackupHandler.Initialize();
        }

        private void BackupPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (BackupPath.Text.Length == 0)
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
                if (dialog.FileName[dialog.FileName.Length - 1] != '\\')
                    BackupPath.Text = dialog.FileName + "\\";
                else
                    BackupPath.Text = dialog.FileName;
                Browse.Focus();
            }
        }

        private void BackUpChatLogAutomatically_CheckedChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Enabled = BackUpChatLogAutomatically.Checked;
            if (!BackUpChatLogAutomatically.Checked)
            {
                BackupHandler.AbortAutomaticBackup();
                EnableIntervalBackup.Checked = false;
            }
        }

        private void EnableIntervalBackup_CheckedChanged(object sender, EventArgs e)
        {
            Interval.Enabled = EnableIntervalBackup.Checked;

            if (!EnableIntervalBackup.Checked)
                BackupHandler.AbortIntervalBackup();
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Text = $"Automatically back up the chat log while the game is running (every {Interval.Value} minutes)";
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
            if (BackUpChatLogAutomatically.Checked && (BackupPath.Text == "" || !Directory.Exists(BackupPath.Text)))
            {
                e.Cancel = true;

                MessageBox.Show("Please choose a valid backup location or turn automatic backup off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SaveSettings();
        }
    }
}
