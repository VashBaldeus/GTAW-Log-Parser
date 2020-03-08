using System;
using System.IO;

namespace Parser.Controllers
{
    public static class ContinuityController
    {
        public const string Version = "1.0.2";
        public const bool IsBetaVersion = false;
        public const string ParameterPrefix = "--";
        
        public static readonly string[] ServerIPs = { string.Empty, string.Empty, "multi" };
        public static string LogLocation = $"client_resources\\{ServerIPs[0]}\\.storage";

        /// <summary>
        /// Initializes the server IPs matching with the
        /// current server depending on the chosen locale
        /// and determines the newest log file if multiple
        /// server IPs are used to connect to the server
        /// </summary>
        public static void InitializeMemory()
        {
            // Initialize the server IPs even if
            // no folder path has been chosen yet
            ServerIPs[0] = Localization.Strings.MainIP;
            ServerIPs[1] = Localization.Strings.SecondaryIP;

            // Return if the user has not picked a
            // RAGEMP directory path yet
            var directoryPath = Properties.Settings.Default.DirectoryPath;
            if (string.IsNullOrWhiteSpace(directoryPath)) return;
            
            var mainStorage = $"{directoryPath}client_resources\\{ServerIPs[0]}\\.storage";
            var secondaryStorage = $"{directoryPath}client_resources\\{ServerIPs[1]}\\.storage";
            
            // Store the server IP used to connect to the server
            // in this variable, or "e" if both IPs are being used
            var serverIp = ServerIPs[File.Exists(mainStorage) ? !File.Exists(secondaryStorage) ? 0 : 2 : File.Exists(secondaryStorage) ? 1 : 0 /* neither file exists in this case but we'll go with 0 */];

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
    }
}
