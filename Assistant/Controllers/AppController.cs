using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using Assistant.Localization;

namespace Assistant.Controllers
{
    public static class AppController
    {
        public const string AssemblyVersion = "4.0.3";
        public static readonly string Version = $"v{AssemblyVersion}";
        public const bool IsBetaVersion = false;
        public static bool CanFollowSystemColor = false;
        public static bool CanFollowSystemMode = false;

        public const string ParameterPrefix = "--";
        public const string ProcessName = "GTA5";
        public const string ProductHeader = "GTAW-Log-Parser";
        public static readonly string[] ServerIPs = { string.Empty, string.Empty, "multi" };
        public static string LogLocation = $"client_resources\\{ServerIPs[0]}\\.storage";

        public static readonly string[] PossibleDirectoryLocations = { "RAGEMP\\", "\\RAGEMP\\", "Games\\RAGEMP\\", "\\Games\\RAGEMP\\" };
        public static readonly string ExecutablePath = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string StartupPath = Path.GetDirectoryName(ExecutablePath);
        public static string PreviousLog = string.Empty;

        /// <summary>
        /// Initializes the server IPs matching with the
        /// current server depending on the chosen locale
        /// and determines the newest log file if multiple
        /// server IPs are used to connect to the server
        /// </summary>
        public static void InitializeServerIp()
        {
            // Initialize the server IPs even if
            // no directory path has been chosen yet
            ServerIPs[0] = Strings.MainIP;
            ServerIPs[1] = Strings.SecondaryIP;

            // Return if the user has not picked
            // a RAGEMP directory path yet
            string directoryPath = Properties.Settings.Default.DirectoryPath;
            if (string.IsNullOrWhiteSpace(directoryPath)) return;

            string mainStorage = $"{directoryPath}client_resources\\{ServerIPs[0]}\\.storage";
            string secondaryStorage = $"{directoryPath}client_resources\\{ServerIPs[1]}\\.storage";

            // Store the server IP used to connect to the server in
            // this variable, or "multi" if both IPs are being used
            string serverIp = ServerIPs[File.Exists(mainStorage) ? !File.Exists(secondaryStorage) ? 0 : 2 : File.Exists(secondaryStorage) ? 1 : 0];

            // multi -> both storage files exist; check them to see which one's the latest
            if (serverIp == "multi")
            {
                try
                {
                    serverIp = ServerIPs[DateTime.Compare(File.GetLastWriteTimeUtc(secondaryStorage), File.GetLastWriteTimeUtc(mainStorage)) > 0 ? 1 : 0];
                }
                catch
                {
                    serverIp = ServerIPs[0];
                }
            }

            // Finally, set the log location
            LogLocation = $"client_resources\\{serverIp}\\.storage";
        }

        /// <summary>
        /// Parses the most recent chat log found at the
        /// selected RAGEMP directory path and returns it.
        /// Displays an error if a chat log does not
        /// exist or if it has an incorrect format
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="removeTimestamps"></param>
        /// <param name="showError"></param>
        /// <returns></returns>
        public static string ParseChatLog(string directoryPath, bool removeTimestamps, bool showError = false)
        {
            InitializeServerIp();

            try
            {
                // Read the chat log
                string log;
                using (StreamReader sr = new StreamReader(directoryPath + AppController.LogLocation))
                {
                    log = sr.ReadToEnd();
                }

                // Grab the chat log part from the .storage file
                log = Regex.Match(log, "\\\"chatlog\\\":\\\".+\\\\n\\\"").Value;
                if (string.IsNullOrWhiteSpace(log))
                    throw new IndexOutOfRangeException();

                // Comments to the right of these lines
                log = log.Replace("\"chatlog\":\"", string.Empty);  // Remove the chat log indicator
                log = log.Replace("\\n", "\n");                     // Change all occurrences of `\n` into new lines
                log = log.Remove(log.Length - 1, 1);                // Remove the `"` character from the end

                log = System.Net.WebUtility.HtmlDecode(log);    // Decode HTML symbols (example: `&apos;` into `'`)
                log = log.TrimEnd('\r', '\n');                  // Remove the `new line` characters from the end

                PreviousLog = log;
                if (removeTimestamps)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                return log;
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                return string.Empty;
            }
        }
    }
}
