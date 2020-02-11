using System;
using System.IO;

namespace ParserMini.Utilities
{
    public static class Data
    {
        public const string Version = "1.0";
        public const bool IsBetaVersion = false;

        public static void Initialize()
        {
            ServerIPs[0] = Localization.Strings.MainIP;
            ServerIPs[1] = Localization.Strings.SecondaryIP;

            string folderPath = Properties.Settings.Default.FolderPath;
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                
                string mainStorage = $"{folderPath}client_resources\\{ServerIPs[0]}\\.storage";
                string secondaryStorage = $"{folderPath}client_resources\\{ServerIPs[1]}\\.storage";
                string serverIP = ServerIPs[File.Exists(mainStorage) ? (!File.Exists(secondaryStorage) ? 0 : 2) : (File.Exists(secondaryStorage) ? 1 : 0 /* neither file exists in this case but we'll go with 0 */)];

                // e   => both storage files exist; check them to see which one's the latest
                if (serverIP == "e")
                {
                    try
                    {
                        serverIP = ServerIPs[DateTime.Compare(File.GetLastWriteTimeUtc(secondaryStorage), File.GetLastWriteTimeUtc(mainStorage)) > 0 ? 1 : 0];
                    }
                    catch
                    {
                        serverIP = ServerIPs[0];
                    }
                }

                LogLocation = $"client_resources\\{serverIP}\\.storage";
            }
        }

        public const string ParameterPrefix = "--";
        public static string[] ServerIPs = { string.Empty, string.Empty, "e"};
        public static string LogLocation = $"client_resources\\{ServerIPs[0]}\\.storage";
    }
}
