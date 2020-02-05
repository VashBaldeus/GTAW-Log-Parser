using System;
using System.Diagnostics;
using System.IO;

namespace MetroParser.Utils
{
    public static class Data
    {
        public static void Initialize()
        {
            string folderPath = Properties.Settings.Default.FolderPath;

            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                ServerIPs[0] = Localization.Strings.MainIP;
                ServerIPs[1] = Localization.Strings.SecondaryIP;

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

        public static readonly string ParameterPrefix = "--";
        public static readonly string ProcessName = "GTA5";
        public static readonly string ProductHeader = "GTAW-Log-Parser";
        public static readonly string[] ServerIPs = { string.Empty, string.Empty, "e"};
        public static string LogLocation = $"client_resources\\{ServerIPs[0]}\\.storage";

        public static readonly string[] PossibleFolderLocations = { "RAGEMP\\", "\\RAGEMP\\", "Games\\RAGEMP\\", "\\Games\\RAGEMP\\" };
        public static readonly string[] PotentiallyOldFiles = { "index.js", "chatlog.js", "chat_extra.js", "chat\\js\\chat.js", "chat\\index.html", "chat\\style\\main_left.css", "chat\\style\\checkbox.css" };

        public static string ExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
        public static string StartupPath = Path.GetDirectoryName(ExecutablePath);
    }
}
