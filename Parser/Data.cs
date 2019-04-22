using System;
using System.IO;

namespace Parser
{
    class Data
    {
        public static void Initialize()
        {
            string folderPath = Properties.Settings.Default.FolderPath;

            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                string mainStorage = $"{folderPath}client_resources\\{serverIPs[0]}\\.storage";
                string secondaryStorage = $"{folderPath}client_resources\\{serverIPs[1]}\\.storage";
                string serverIP = serverIPs[File.Exists(mainStorage) ? (!File.Exists(secondaryStorage) ? 0 : 2) : (File.Exists(secondaryStorage) ? 1 : 0 /* neither file exists in this case but we'll go with 0 */)];

                // e   => both storage files exist; check them to see which one's the latest
                if (serverIP == "e")
                {
                    try
                    {
                        serverIP = serverIPs[DateTime.Compare(File.GetLastWriteTimeUtc(secondaryStorage), File.GetLastWriteTimeUtc(mainStorage)) > 0 ? 1 : 0];
                    }
                    catch
                    {
                        serverIP = serverIPs[0];
                    }
                }

                logLocation = $"client_resources\\{serverIP}\\.storage";
            }
        }

        public static readonly string processName = "GTA5";
        public static readonly string productHeader = "GTAW-Log-Parser";
        public static readonly string[] serverIPs = { "164.132.206.209_22005", "play.gta.world_22005", "e"};
        public static string logLocation = $"client_resources\\{serverIPs[0]}\\.storage";
        public static readonly string parameterPrefix = "--";

        public static readonly string[] potentiallyOldFiles = { "index.js", "chatlog.js", "chat_extra.js", "chat\\js\\chat.js", "chat\\index.html", "chat\\style\\main_left.css", "chat\\style\\checkbox.css" };
    }
}
