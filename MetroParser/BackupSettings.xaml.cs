﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MetroParser.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for BackupSettingsWindow.xaml
    /// </summary>
    public partial class BackupSettingsWindow
    {
        public BackupSettingsWindow()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.BackupPath = MainWindow.GetText(BackupPath);

            Properties.Settings.Default.BackupChatLogAutomatically = BackUpChatLogAutomatically.IsChecked == true;
            Properties.Settings.Default.EnableIntervalBackup = EnableIntervalBackup.IsChecked == true;
            Properties.Settings.Default.IntervalTime = (int)Interval.Value;
            Properties.Settings.Default.RemoveTimestampsFromBackup = RemoveTimestamps.IsChecked == true;
            Properties.Settings.Default.StartWithWindows = StartWithWindows.IsChecked == true;
            Properties.Settings.Default.SuppressNotifications = SuppressNotifications.IsChecked == true;

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            Browse.Focus();

            MainWindow.SetText(BackupPath, Properties.Settings.Default.BackupPath);

            BackUpChatLogAutomatically.IsChecked = Properties.Settings.Default.BackupChatLogAutomatically;
            EnableIntervalBackup.IsChecked = Properties.Settings.Default.EnableIntervalBackup;
            Interval.Value = Properties.Settings.Default.IntervalTime;
            RemoveTimestamps.IsChecked = Properties.Settings.Default.RemoveTimestampsFromBackup;
            StartWithWindows.IsChecked = Properties.Settings.Default.StartWithWindows;
            SuppressNotifications.IsChecked = Properties.Settings.Default.SuppressNotifications;
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

        private void BackupPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
                    if (MessageBox.Show(Strings.MoveBackups, Strings.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        List<string> moved = new List<string>();
                        List<string> notMoved = new List<string>();

                        foreach (DirectoryInfo directory in finalDirectories)
                        {
                            if (!Directory.Exists(MainWindow.GetText(BackupPath) + directory.Name))
                            {
                                Directory.Move(directory.FullName, MainWindow.GetText(BackupPath) + directory.Name);
                                moved.Add(directory.Name);
                            }
                            else
                                notMoved.Add(directory.Name);
                        }

                        Properties.Settings.Default.BackupPath = MainWindow.GetText(BackupPath);
                        Properties.Settings.Default.Save();

                        if (notMoved.Count > 0)
                            MessageBox.Show((moved.Count > 0 ? string.Format(Strings.PartialMoveWarning, string.Join(", ", moved)) : Strings.NothingMovedWarning) + string.Format(Strings.AlreadyExistingFoldersWarning, string.Join(", ", notMoved)), Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.BackupMoveError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackupPath_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MainWindow.GetText(BackupPath)))
                Browse_Click(this, null);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;

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
                        MainWindow.SetText(BackupPath, dialog.FileName + "\\");
                        validLocation = true;
                    }
                    else
                        MessageBox.Show(Strings.BadBackupPath, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    validLocation = true;
            }

            Topmost = true;
            Activate();
        }

        private void BackUpChatLogAutomatically_CheckedChanged(object sender, RoutedEventArgs e)
        {
            EnableIntervalBackup.IsEnabled = BackUpChatLogAutomatically.IsChecked == true;
            RemoveTimestamps.IsEnabled = BackUpChatLogAutomatically.IsChecked == true;
            StartWithWindows.IsEnabled = BackUpChatLogAutomatically.IsChecked == true;
            SuppressNotifications.IsEnabled = BackUpChatLogAutomatically.IsChecked == true;

            if (BackUpChatLogAutomatically.IsChecked != true)
            {
                StartWithWindows.IsChecked = false;
                RemoveTimestamps.IsChecked = false;
                EnableIntervalBackup.IsChecked = false;
                SuppressNotifications.IsChecked = false;
            }
        }

        private void EnableIntervalBackup_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Interval.IsEnabled = EnableIntervalBackup.IsChecked == true;
        }

        private void Interval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (IntervalLabel2 == null)
                return;

            IntervalLabel2.Content = string.Format(Strings.Recommended, Interval.Value > 1 ? Strings.MinutePlural : Strings.MinuteSingular);
            EnableIntervalBackup.Content= string.Format(Strings.IntervalHint, Interval.Value, Interval.Value > 1 ? Strings.MinutePlural : Strings.MinutePlural);
        }

        private void StartWithWindows_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (StartWithWindows.IsChecked == true && !StartupHandler.IsAddedToStartup())
                MessageBox.Show(Strings.AutoStartWarning, Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetSettings();
            LoadSettings();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BackupSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BackUpChatLogAutomatically.IsChecked == true && (string.IsNullOrWhiteSpace(MainWindow.GetText(BackupPath)) || !Directory.Exists(MainWindow.GetText(BackupPath))))
            {
                e.Cancel = true;
                MessageBox.Show(Strings.BadBackupPathSave, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if ((StartWithWindows.IsChecked == true && !StartupHandler.IsAddedToStartup()) || (!StartWithWindows.IsChecked == true && StartupHandler.IsAddedToStartup()))
                StartupHandler.ToggleStartup(StartWithWindows.IsChecked == true);

            SaveSettings();

            Hide();

            e.Cancel = true;
        }
    }
}
