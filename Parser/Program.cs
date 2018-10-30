using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parser
{
    static class Program
    {
        private static bool startMinimized = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex mutex = new Mutex(true, "UniqueAppId", out bool isUnique);

            if (!isUnique)
            {
                MessageBox.Show("Another instance is already running, check your taskbar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var args = Environment.GetCommandLineArgs();

            if (args != null && args.Any(arg => arg == "--minimized"))
                startMinimized = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(startMinimized));

            GC.KeepAlive(mutex);
        }
    }
}
