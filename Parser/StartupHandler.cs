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

        public static void Initialize(bool enabled)
        {
            if (enabled)
                TryAddingToStartup();
            else
                TryRemovingFromStartup();
        }

        private static void TryAddingToStartup()
        {
            try
            {
                if (System.IO.File.Exists(startUpFolder + shortcutName))
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

            }
        }

        private static void TryRemovingFromStartup()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(startUpFolder);
                FileInfo[] allShortcuts = directory.GetFiles("*.lnk");

                List<FileInfo> parserShortcuts = new List<FileInfo>();

                foreach (FileInfo file in allShortcuts)
                {
                    if (file.Name.ToLower() == shortcutName.ToLower())
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

            }
        }

        public static bool IsAddedToStartup()
        {
            return System.IO.File.Exists(startUpFolder + shortcutName);
        }
    }
}
