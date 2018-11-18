using System;
using System.IO;

namespace Parser
{
    class Data
    {
        public static void Initialize()
        {
            string folderPath = Properties.Settings.Default.FolderPath;

            if (string.IsNullOrWhiteSpace(folderPath))
                return;
            else
            {
                if (!Directory.Exists(folderPath + $"client_resources\\{primaryServerIP}") && Directory.Exists(folderPath + $"client_resources\\{secondaryServerIP}"))
                    logLocation = $"client_resources\\{secondaryServerIP}\\.storage";
            }
        }

        public static readonly string processName = "GTA5";
        public static readonly string productHeader = "GTAW-Log-Parser";
        public static readonly string primaryServerIP = "164.132.206.209_22005";
        public static readonly string secondaryServerIP = "play.gta.world_22005";
        public static string logLocation = $"client_resources\\{primaryServerIP}\\.storage";
        public static readonly string parameterPrefix = "--";
    }
}
