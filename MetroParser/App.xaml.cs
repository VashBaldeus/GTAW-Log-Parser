using MahApps.Metro;
using MetroParser.Infrastructure;
using MetroParser.UI;
using MetroParser.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using MetroParser.Properties;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            StyleController.InitializeFollowEligibility();

            if (Settings.Default.FollowSystemMode)
            {
                if (Data.CanFollowSystemMode)
                    StyleController.DarkMode = StyleController.GetAppMode();
                else
                    Settings.Default.FollowSystemMode = false;
            }
            if (Settings.Default.FollowSystemColor)
            {
                if (Data.CanFollowSystemColor)
                {
                    StyleController.ValidStyles.Add("Windows");
                    StyleController.Style = "Windows";
                }
                else
                    Settings.Default.FollowSystemColor = false;
            }
            Settings.Default.Save();

            StyleController.UpdateTheme();
            base.OnStartup(e);
        }

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

            LocalizationController.Initialize();
            Data.Initialize();

            if (startMinimizedWithoutTrayIcon)
            {
                StartupController.Initialize();
                BackupController.Initialize();
            }
            else
            {
                if (!Settings.Default.HasPickedLanguage)
                {
                    LanguagePickerWindow languagePicker = new LanguagePickerWindow();
                    languagePicker.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(startMinimized: startMinimized);
                    if (!startMinimized)
                        mainWindow.Show();
                }
            }

            GC.KeepAlive(mutex);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            StyleController.StopWatchers();
            BackupController.quitting = true;
        }
    }
}
