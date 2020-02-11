using ParserMini.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ParserMini.Localization;

namespace ParserMini
{
    static class Program
    {
        private static bool isRestarted = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Any(arg => arg == $"{Data.ParameterPrefix}restart"))
                    isRestarted = true;
            }

            Mutex mutex = new Mutex(true, "UniqueAppId", out bool isUnique);
            if (!isUnique && !isRestarted)
            {
                MessageBox.Show(Strings.OtherInstanceRunning, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Infrastructure.LocalizationController.Initialize();
            Data.Initialize();
            Application.Run(new UI.Main());

            GC.KeepAlive(mutex);
        }
    }
}
