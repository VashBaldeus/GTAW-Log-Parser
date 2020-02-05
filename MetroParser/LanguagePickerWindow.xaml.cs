using System;
using System.Diagnostics;
using System.Windows;

namespace MetroParser
{
    /// <summary>
    /// Interaction logic for LanguagePickerWindow.xaml
    /// </summary>
    public partial class LanguagePickerWindow
    {
        private readonly System.Windows.Threading.DispatcherTimer Timer;

        public bool isStarting = false;
        private readonly bool handleListChange = false;

        public LanguagePickerWindow()
        {
            InitializeComponent();

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Start();

            foreach (LocalizationManager.Language language in (LocalizationManager.Language[])Enum.GetValues(typeof(LocalizationManager.Language)))
                LanguageList.Items.Add(language.ToString());

            LanguageList.SelectedIndex = 0;
            handleListChange = true;

            StartButton.Focus();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LanguageCode = LocalizationManager.GetLanguage();
            Properties.Settings.Default.HasPickedLanguage = true;
            Properties.Settings.Default.Save();

            isStarting = true;
            Close();
        }

        private void LanguageList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!handleListChange)
                return;

            LocalizationManager.SetLanguage((LocalizationManager.Language)LanguageList.SelectedIndex, save: false);

            Title = Localization.Strings.Language;
            StartButton.Content = Localization.Strings.Start;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Thickness margin = WelcomeLabel.Margin;

            if (margin.Left < -1201 && margin.Right > -549)
            {
                margin.Left = 0;
                margin.Right = -1750;
            }
            else
            {
                margin.Left -= 1;
                margin.Right += 1;
            }

            WelcomeLabel.Margin = margin;
        }

        private void LanguagePicker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isStarting)
                Application.Current.Shutdown();
            else
            {
                ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
                startInfo.FileName = Data.ExecutablePath;
                startInfo.Arguments = $"{Data.ParameterPrefix}restart";
                Process.Start(startInfo);

                Application.Current.Shutdown();
            }
        }
    }
}
