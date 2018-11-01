using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Parser
{
    public static class StartupHandler
    {
        public static readonly string startUpFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\";
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

        private static void TryAddingToStartup()
        {
            try
            {
                if (IsAddedToStartup())
                    return;

                WshShell wsh = new WshShell();
                IWshShortcut shortcut = wsh.CreateShortcut(startUpFolder + shortcutName) as IWshShortcut;
                shortcut.Arguments = "--minimized";
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Application.StartupPath;
                shortcut.Save();
            }
            catch
            {
                MessageBox.Show("An error occured while trying to enable the automatic startup function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void TryRemovingFromStartup()
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
