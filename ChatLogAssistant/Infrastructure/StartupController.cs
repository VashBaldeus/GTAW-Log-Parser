using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Collections.Generic;
using System.Windows;
using GTAWAssistant.Utilities;

namespace GTAWAssistant.Infrastructure
{
    public static class StartupController
    {
        public static readonly string startUpFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.Startup)}\\";
        public static readonly string shortcutName = "gtaw-parser.lnk";

        public static void Initialize()
        {
            if (IsAddedToStartup())
            {
                if (!Properties.Settings.Default.BackupChatLogAutomatically)
                {
                    Properties.Settings.Default.StartWithWindows = false;
                    Properties.Settings.Default.Save();

                    TryRemovingFromStartup();
                }
                else if (!Properties.Settings.Default.StartWithWindows)
                {
                    TryRemovingFromStartup();
                }

                if (IsAddedToStartup())
                    CheckIfLegitimate();
            }
            else
            {
                if (Properties.Settings.Default.StartWithWindows)
                {
                    TryAddingToStartup();
                }
            }
        }

        public static void ToggleStartup(bool toggle)
        {
            if (toggle)
                TryAddingToStartup();
            else
                TryRemovingFromStartup();
        }

        private static void CheckIfLegitimate()
        {
            try
            {
                bool legit = true;
                List<FileInfo> parserShortcuts = GetParserShortcuts();

                if (parserShortcuts.Count > 0)
                {
                    foreach (FileInfo file in parserShortcuts)
                    {
                        if (legit)
                        {
                            WshShell wshShell = new WshShell();
                            IWshShortcut shortcut = wshShell.CreateShortcut(file.FullName) as IWshShortcut;

                            if (shortcut.TargetPath != Data.ExecutablePath)
                                shortcut.TargetPath = Data.ExecutablePath;
                            if (!shortcut.Arguments.ToLower().Contains($"{Data.ParameterPrefix}minimized"))
                                shortcut.Arguments = $"{Data.ParameterPrefix}minimized";
                            if (shortcut.WorkingDirectory != Data.StartupPath)
                                shortcut.WorkingDirectory = Data.StartupPath;

                            shortcut.Save();
                            legit = false;
                        }
                        else
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch
            {
                // Silent exception
            }
        }

        private static void TryAddingToStartup(bool showError = true)
        {
            try
            {
                if (IsAddedToStartup())
                    return;

                WshShell wshShell = new WshShell();
                IWshShortcut shortcut = wshShell.CreateShortcut(startUpFolder + shortcutName) as IWshShortcut;
                shortcut.TargetPath = Data.ExecutablePath;
                shortcut.Arguments = $"{Data.ParameterPrefix}minimized";
                shortcut.WorkingDirectory = Data.StartupPath;
                shortcut.Save();
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Localization.Strings.AutoStartEnableError, Localization.Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                Properties.Settings.Default.StartWithWindows = false;
                Properties.Settings.Default.Save();
            }
        }

        private static void TryRemovingFromStartup(bool showError = true)
        {
            try
            {
                if (!IsAddedToStartup())
                    return;

                List<FileInfo> parserShortcuts = GetParserShortcuts();

                if (parserShortcuts.Count > 0)
                {
                    foreach (FileInfo file in parserShortcuts)
                    {
                        file.Delete();
                    }
                }
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Localization.Strings.AutoStartDisableError, Localization.Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                if (IsAddedToStartup())
                {
                    Properties.Settings.Default.StartWithWindows = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static bool IsAddedToStartup()
        {
            return GetParserShortcuts().Count > 0;
        }

        private static List<FileInfo> GetParserShortcuts()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(startUpFolder);
                FileInfo[] allShortcuts = directory.GetFiles("*.lnk");

                List<FileInfo> parserShortcuts = new List<FileInfo>();

                foreach (FileInfo file in allShortcuts)
                {
                    if (file.Name.ToLower().Contains(shortcutName.ToLower()))
                        parserShortcuts.Add(file);
                }

                return parserShortcuts;
            }
            catch
            {
                return new List<FileInfo>();
            }
        }
    }
}
