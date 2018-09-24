using System;
using System.Windows.Forms;

using System.Threading;

namespace Parser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex mutex = new Mutex(true, "UniqueAppId", out bool notAlreadyRunning);

            if (!notAlreadyRunning)
            {
                MessageBox.Show("Another instance is already running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

            GC.KeepAlive(mutex);
        }
    }
}
