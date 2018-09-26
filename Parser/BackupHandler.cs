using System;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Parser
{
    public static class BackupHandler
    {
        private static Thread backupThread;
        private static Thread intervalThread;

        private static string folderPath;
        private static string backupPath;

        private static bool enableAutomaticBackup;
        private static bool enableIntervalBackup;

        private static bool runBackgroundBackup = false;
        private static bool runBackgroundInterval = false;
        public static bool quitting = false;

        public static void Initialize()
        {
            folderPath = Properties.Settings.Default.FolderPath;
            backupPath = Properties.Settings.Default.BackupPath;

            enableAutomaticBackup = Properties.Settings.Default.BackupChatLogAutomatically;
            enableIntervalBackup = Properties.Settings.Default.EnableIntervalBackup;

            if (backupPath == "" || !Directory.Exists(backupPath))
                return;
            else if (folderPath == "" || !Directory.Exists(folderPath + "\\client_resources"))
                return;

            ResumeIfQueuedToStop();

            if (enableAutomaticBackup && (backupThread == null || !backupThread.IsAlive))
            {
                runBackgroundBackup = true;

                backupThread = new Thread(() => BackupWorker());
                backupThread.Start();
            }
            if (enableIntervalBackup && (intervalThread == null || !intervalThread.IsAlive))
            {
                runBackgroundInterval = true;

                intervalThread = new Thread(() => IntervalWorker());
                intervalThread.Start();
            }
        }

        public static void AbortAutomaticBackup()
        {
            if (backupThread != null && backupThread.IsAlive)
                runBackgroundBackup = false;
        }

        public static void AbortIntervalBackup()
        {
            if (intervalThread != null && intervalThread.IsAlive)
                runBackgroundInterval = false;
        }

        public static bool IsAnyRunning()
        {
            return runBackgroundBackup || runBackgroundInterval;
        }

        public static void ResumeIfQueuedToStop()
        {
            if (backupThread != null && backupThread.IsAlive && !runBackgroundBackup && !quitting)
                runBackgroundBackup = true;

            if (intervalThread != null && intervalThread.IsAlive && !runBackgroundInterval && !quitting)
                runBackgroundInterval = true;
        }

        public static void AbortAll()
        {
            AbortIntervalBackup();
            AbortAutomaticBackup();
        }

        private static readonly int exitDelay = 10;
        private static bool isGameRunning = false;

        private static void BackupWorker()
        {
            while (!quitting && runBackgroundBackup)
            {
                Process[] processes = Process.GetProcessesByName("GTA5");

                if (!isGameRunning && processes.Length != 0)
                    isGameRunning = true;
                else if (isGameRunning && processes.Length == 0)
                {
                    isGameRunning = false;
                    ParseThenSaveToFile(true);
                }

                Thread.Sleep(exitDelay * 1000);
            }
        }

        private static void IntervalWorker()
        {
            while (!quitting && runBackgroundInterval)
            {
                int intervalTime = Properties.Settings.Default.IntervalTime;

                if (isGameRunning)
                    ParseThenSaveToFile();

                for (int i = 0; i < intervalTime * 6; i++)
                {
                    if (quitting || !runBackgroundInterval)
                        break;

                    Thread.Sleep(10 * 1000);
                }
            }
        }

        private static void ParseThenSaveToFile(bool gameClosed = false)
        {
            try
            {
                string parsed = Main.ParseChatLog(folderPath, Properties.Settings.Default.RemoveTimestampsFromBackup);
                if (parsed.Length == 0)
                    return;

                string fileName = parsed.Substring(0, parsed.IndexOf("\n"));

                string fileNameDate = Regex.Match(fileName, @"\d{1,2}\/[A-Za-z]{3}\/\d{4}").ToString();
                fileNameDate = fileNameDate.Replace("/", ".");

                string fileNameTime = Regex.Match(fileName, @"\d{1,2}:\d{1,2}:\d{1,2}").ToString();
                fileNameTime = fileNameTime.Replace(":", ".");

                fileName = fileNameDate + "-" + fileNameTime + ".txt";

                if (!File.Exists(backupPath + fileName))
                {
                    using (StreamWriter sw = new StreamWriter(backupPath + fileName))
                    {
                        sw.Write(parsed.Replace("\n", Environment.NewLine));
                    }

                    if (gameClosed)
                        MessageBox.Show($"Successfully parsed and backed up chat log to {backupPath + fileName}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(backupPath + ".temp"))
                    {
                        sw.Write(parsed.Replace("\n", Environment.NewLine));
                    }

                    FileInfo oldFile = new FileInfo(backupPath + fileName);
                    FileInfo newFile = new FileInfo(backupPath + ".temp");

                    if (oldFile.Length < newFile.Length)
                    {
                        File.Delete(backupPath + fileName);
                        File.Move(backupPath + ".temp", backupPath + fileName);
                    }
                    else
                        File.Delete(backupPath + ".temp");

                    if (gameClosed)
                        MessageBox.Show($"Successfully parsed and backed up chat log to {backupPath + fileName}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("An error occured while trying to automatically save the chat log.\n\nMake sure you picked a non-system directory for your backup path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
