using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MetroParser.Localization;
using System.Windows;
using System.Text.RegularExpressions;
using MetroParser.Utilities;
using MetroParser.UI;

namespace MetroParser.Infrastructure
{
    public static class BackupController
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

        private static void DisplayBackupResultMessage(string text, string title, MessageBoxButton buttons, MessageBoxImage image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(text, title, buttons, image);
            });
        }

        public static void Initialize()
        {
            folderPath = Properties.Settings.Default.FolderPath;
            backupPath = Properties.Settings.Default.BackupPath;

            enableAutomaticBackup = Properties.Settings.Default.BackupChatLogAutomatically;
            enableIntervalBackup = Properties.Settings.Default.EnableIntervalBackup;

            if (string.IsNullOrWhiteSpace(backupPath) || !Directory.Exists(backupPath))
                return;
            else if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath + "\\client_resources"))
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

            SortOldBackups();
        }

        private static void SortOldBackups()
        {
            try
            {
                if (File.Exists(backupPath + ".temp"))
                    File.Delete(backupPath + ".temp");

                DirectoryInfo directory = new DirectoryInfo(backupPath);
                FileInfo[] textFilesInDirectory = directory.GetFiles("*.txt");

                foreach (FileInfo file in textFilesInDirectory)
                {
                    if (!Regex.IsMatch(file.Name, @"\d{1,2}.[A-Za-z]{3}.\d{4}-\d{1,2}.\d{1,2}.\d{1,2}"))
                        continue;

                    string year = Regex.Match(file.Name, @"\d{4}").ToString();
                    string month = Regex.Match(file.Name, @"[A-Za-z]{3}").ToString();

                    if (string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(month))
                        continue;

                    string path = $"{backupPath}{year}\\{month}\\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (!File.Exists(path + file.Name))
                        File.Move(file.FullName, path + file.Name);
                    else
                        file.Delete();
                }
            }
            catch
            {
                // Silent Exception
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

        private const int gameClosedCheckTime = 10;
        private static bool isGameRunning = false;

        private static void BackupWorker()
        {
            while (!quitting && runBackgroundBackup)
            {
                Process[] processes = Process.GetProcessesByName(Data.ProcessName);

                if (!isGameRunning && processes.Length != 0)
                    isGameRunning = true;
                else if (isGameRunning && processes.Length == 0)
                {
                    isGameRunning = false;
                    ParseThenSaveToFile(true);
                }

                Thread.Sleep(gameClosedCheckTime * 1000);
            }
        }

        private static void IntervalWorker()
        {
            while (!quitting && runBackgroundInterval)
            {
                int intervalTime = Properties.Settings.Default.IntervalTime;

                if (isGameRunning && File.Exists(folderPath + Data.LogLocation))
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
                string parsed = MainWindow.ParseChatLog(folderPath: folderPath, removeTimestamps: Properties.Settings.Default.RemoveTimestampsFromBackup, showError: gameClosed);
                Cryptography.SaveParsedHash(parsed);

                if (string.IsNullOrWhiteSpace(parsed))
                    return;

                string fileName = parsed.Substring(0, parsed.IndexOf("\n"));

                string fileNameDate = Regex.Match(fileName, @"\d{1,2}\/[A-Za-z]{3}\/\d{4}").ToString();
                fileNameDate = fileNameDate.Replace("/", ".");

                string year = Regex.Match(fileNameDate, @"\d{4}").ToString();
                string month = Regex.Match(fileNameDate, @"[A-Za-z]{3}").ToString();

                string fileNameTime = Regex.Match(fileName, @"\d{1,2}:\d{1,2}:\d{1,2}").ToString();
                fileNameTime = fileNameTime.Replace(":", ".");

                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileNameDate) || string.IsNullOrWhiteSpace(fileNameTime) || string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(month))
                    return;

                fileName = fileNameDate + "-" + fileNameTime + ".txt";

                string path = $"{backupPath}{year}\\{month}\\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(path + fileName))
                {
                    using (StreamWriter sw = new StreamWriter(path + fileName))
                    {
                        sw.Write(parsed.Replace("\n", Environment.NewLine));
                    }

                    if (gameClosed && !Properties.Settings.Default.SuppressNotifications)
                        DisplayBackupResultMessage(string.Format(Strings.SuccessfulBackup, path + fileName), Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (File.Exists(path + ".temp"))
                        File.Delete(path + ".temp");

                    using (StreamWriter sw = new StreamWriter(path + ".temp"))
                    {
                        sw.Write(parsed.Replace("\n", Environment.NewLine));
                    }

                    FileInfo oldFile = new FileInfo(path + fileName);
                    FileInfo newFile = new FileInfo(path + ".temp");

                    if (oldFile.Length < newFile.Length)
                    {
                        File.Delete(path + fileName);
                        File.Move(path + ".temp", path + fileName);
                    }
                    else
                        File.Delete(path + ".temp");

                    if (gameClosed && !Properties.Settings.Default.SuppressNotifications)
                        DisplayBackupResultMessage(string.Format(Strings.SuccessfulBackup, path + fileName), Strings.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                if (gameClosed)
                    DisplayBackupResultMessage(Strings.BackupError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
