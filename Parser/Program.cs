using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parser
{
    static class Program
    {
        private static bool startMinimized = false;
        private static bool startMinimizedWithoutTrayIcon = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.Sleep(1000);

            Mutex mutex = new Mutex(true, "UniqueAppId", out bool isUnique);

            if (!isUnique)
            {
                MessageBox.Show(Localization.Strings.OtherInstanceRunning, Localization.Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var args = Environment.GetCommandLineArgs();

            if (args != null && args.Any(arg => arg == $"{Data.ParameterPrefix}minimized"))
            {
                startMinimized = true;

                if (args.Any(arg => arg == $"{Data.ParameterPrefix}notray"))
                    startMinimizedWithoutTrayIcon = true;
            }

            Data.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!startMinimizedWithoutTrayIcon)
                Application.Run(new Main(startMinimized));
            else
            {
                StartupHandler.Initialize();
                BackupHandler.Initialize();
            }

            GC.KeepAlive(mutex);
        }
    }
}
