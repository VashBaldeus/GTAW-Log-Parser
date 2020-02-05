using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool startMinimized = false;
        private static bool startMinimizedWithoutTrayIcon = false;
        private static bool isRestarted = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Any(arg => arg == $"{Data.ParameterPrefix}restart"))
                    isRestarted = true;

                if (args.Any(arg => arg == $"{Data.ParameterPrefix}minimized"))
                {
                    startMinimized = true;

                    if (args.Any(arg => arg == $"{Data.ParameterPrefix}notray"))
                        startMinimizedWithoutTrayIcon = true;
                }
            }

            Mutex mutex = new Mutex(true, "UniqueAppId", out bool isUnique);
            if (!isUnique && !isRestarted)
            {
                MessageBox.Show(Localization.Strings.OtherInstanceRunning, Localization.Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LocalizationManager.Initialize();
            Data.Initialize();

            if (startMinimizedWithoutTrayIcon)
            {
                StartupHandler.Initialize();
                BackupHandler.Initialize();
            }
            else
            {
                if (!MetroParser.Properties.Settings.Default.HasPickedLanguage)
                {
                    LanguagePickerWindow languagePicker = new LanguagePickerWindow();
                    languagePicker.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(startMinimized: startMinimized);
                    mainWindow.Show();
                }
            }

            GC.KeepAlive(mutex);
        }
    }
}
