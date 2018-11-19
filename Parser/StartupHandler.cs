using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Parser
{
    public static class StartupHandler
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

                            if (shortcut.TargetPath != Application.ExecutablePath)
                                shortcut.TargetPath = Application.ExecutablePath;
                            if (!shortcut.Arguments.ToLower().Contains($"{Data.parameterPrefix}minimized"))
                                shortcut.Arguments = $"{Data.parameterPrefix}minimized";
                            if (shortcut.WorkingDirectory != Application.StartupPath)
                                shortcut.WorkingDirectory = Application.StartupPath;

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
                // Silent Exception
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
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.Arguments = $"{Data.parameterPrefix}minimized";
                shortcut.WorkingDirectory = Application.StartupPath;
                shortcut.Save();
            }
            catch
            {
                if (showError)
                    MessageBox.Show("An error occured while trying to enable the automatic startup function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                    MessageBox.Show("An error occured while trying to disable the automatic startup function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
