using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.IO;

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

            Properties.Settings.Default.Save();
        }

        public static void ToggleStartup(bool toggle)
        {
            if (toggle)
                TryAddingToStartup();
            else
                TryRemovingFromStartup();
        }

        public static void TryAddingToStartup()
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

        public static void TryRemovingFromStartup()
        {
            try
            {
                if (!IsAddedToStartup())
                    return;

                DirectoryInfo directory = new DirectoryInfo(startUpFolder);
                FileInfo[] allShortcuts = directory.GetFiles("*.lnk");

                List<FileInfo> parserShortcuts = new List<FileInfo>();

                foreach (FileInfo file in allShortcuts)
                {
                    if (file.Name.ToLower().Contains(shortcutName.ToLower()))
                        parserShortcuts.Add(file);
                }

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
                MessageBox.Show("An error occured while trying to disable the automatic startup function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsAddedToStartup()
        {
            return System.IO.File.Exists(startUpFolder + shortcutName);
        }
    }
}
