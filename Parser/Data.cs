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
                if (!File.Exists($"{folderPath}client_resources\\{serverIPs[0]}\\.storage") && File.Exists($"{folderPath}client_resources\\{serverIPs[1]}\\.storage"))
                {
                    logLocation = $"client_resources\\{serverIPs[1]}\\.storage";
                    return;
                }

                if (!Directory.Exists($"{folderPath}client_resources\\{serverIPs[0]}") && Directory.Exists($"{folderPath}client_resources\\{serverIPs[1]}"))
                    logLocation = $"client_resources\\{serverIPs[1]}\\.storage";
            }
        }

        public static readonly string processName = "GTA5";
        public static readonly string productHeader = "GTAW-Log-Parser";
        public static readonly string[] serverIPs = { "164.132.206.209_22005", "play.gta.world_22005" };
        public static string logLocation = $"client_resources\\{serverIPs[0]}\\.storage";
        public static readonly string parameterPrefix = "--";

        public static readonly string[] potentiallyOldFiles = { "index.js", "chatlog.js", "chat_extra.js", "chat\\js\\chat.js", "chat\\index.html", "chat\\style\\main_left.css", "chat\\style\\checkbox.css" };
    }
}
