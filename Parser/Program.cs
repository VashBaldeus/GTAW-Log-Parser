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
            Mutex mutex = new Mutex(true, "UniqueAppId", out bool isUnique);

            if (!isUnique)
            {
                MessageBox.Show("Another instance is already running, check your taskbar or task manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var args = Environment.GetCommandLineArgs();

            if (args != null && args.Any(arg => arg == $"{Data.parameterPrefix}minimized"))
            {
                startMinimized = true;

                if (args.Any(arg => arg == $"{Data.parameterPrefix}notray"))
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

                MessageBox.Show("Started the GTA World Chat Log Parser in minimized mode with no tray icon. You can only close it from the task manager.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            GC.KeepAlive(mutex);
        }
    }
}
