using System;
using System.Windows;
using MetroParser.Localization;

namespace MetroParser.UI
{
    /// <summary>
    /// Interaction logic for ProgramSettingsWindow.xaml
    /// </summary>
    public partial class ProgramSettingsWindow
    {
        private readonly MainWindow _mainWindow;

        public ProgramSettingsWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            Left = _mainWindow.Left + (_mainWindow.Width / 2 - Width / 2);
            Top = _mainWindow.Top + (_mainWindow.Height / 2 - Height / 2) + 55;

            CloseWindow.Focus();
            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.DisableForumsButton = DisableForumsButton.IsChecked == true;
            Properties.Settings.Default.DisableFacebrowserButton = DisableFacebrowserButton.IsChecked == true;
            Properties.Settings.Default.DisableUCPButton = DisableUCPButton.IsChecked == true;
            Properties.Settings.Default.DisableReleasesButton = DisableReleasesButton.IsChecked == true;
            Properties.Settings.Default.DisableProjectButton = DisableProjectButton.IsChecked == true;
            Properties.Settings.Default.DisableProfileButton = DisableProfileButton.IsChecked == true;
            Properties.Settings.Default.UpdateCheckTimeout = (int)Timeout.Value;

            Properties.Settings.Default.DisableInformationPopups = DisableInformationPopups.IsChecked == true;
            Properties.Settings.Default.DisableWarningPopups = DisableWarningPopups.IsChecked == true;
            Properties.Settings.Default.DisableErrorPopups = DisableErrorPopups.IsChecked == true;
            Properties.Settings.Default.IgnoreBetaVersions = IgnoreBetaVersions.IsChecked == true;

            Infrastructure.StyleController.DarkMode = ToggleDarkMode.IsChecked == true;
            Properties.Settings.Default.Theme = Themes.SelectedItem.ToString();

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            DisableForumsButton.IsChecked = Properties.Settings.Default.DisableForumsButton;
            DisableFacebrowserButton.IsChecked = Properties.Settings.Default.DisableFacebrowserButton;
            DisableUCPButton.IsChecked = Properties.Settings.Default.DisableUCPButton;
            DisableReleasesButton.IsChecked = Properties.Settings.Default.DisableReleasesButton;
            DisableProjectButton.IsChecked = Properties.Settings.Default.DisableProjectButton;
            DisableProfileButton.IsChecked = Properties.Settings.Default.DisableProfileButton;
            Timeout.Value = Properties.Settings.Default.UpdateCheckTimeout;

            DisableInformationPopups.IsChecked = Properties.Settings.Default.DisableInformationPopups;
            DisableWarningPopups.IsChecked = Properties.Settings.Default.DisableWarningPopups;
            DisableErrorPopups.IsChecked = Properties.Settings.Default.DisableErrorPopups;
            IgnoreBetaVersions.IsChecked = Properties.Settings.Default.IgnoreBetaVersions;

            ToggleDarkMode.IsChecked = Infrastructure.StyleController.DarkMode;
            Timeout.Foreground = _mainWindow.UpdateCheckProgress.Foreground = ToggleDarkMode.IsChecked == true ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.Black;

            Themes.Items.Clear();
            foreach (string style in Infrastructure.StyleController.ValidStyles)
            {
                Themes.Items.Add(style);
            }
            Themes.SelectedItem = Infrastructure.StyleController.GetValidStyle(Properties.Settings.Default.Theme);
        }

        public static void ResetSettings()
        {
            Properties.Settings.Default.DisableForumsButton = false;
            Properties.Settings.Default.DisableFacebrowserButton = false;
            Properties.Settings.Default.DisableUCPButton = false;
            Properties.Settings.Default.DisableReleasesButton= false;
            Properties.Settings.Default.DisableProjectButton = false;
            Properties.Settings.Default.DisableProfileButton = false;
            Properties.Settings.Default.UpdateCheckTimeout = 4;

            Properties.Settings.Default.DisableInformationPopups = false;
            Properties.Settings.Default.DisableWarningPopups = false;
            Properties.Settings.Default.DisableErrorPopups = false;
            Properties.Settings.Default.IgnoreBetaVersions = true;

            Infrastructure.StyleController.DarkMode = false;
            Properties.Settings.Default.Theme = Infrastructure.StyleController.DefaultLightStyle;

            Properties.Settings.Default.Save();
        }

        private void Timeout_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TimeoutLabel2 == null)
                return;

            TimeoutLabel2.Content = string.Format("{0}.", Timeout.Value > 1 ? Strings.SecondPlural : Strings.SecondSingular);
        }

        private void DisableForumsButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenForums.Visibility = DisableForumsButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableFacebrowserButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenFacebrowser.Visibility = DisableFacebrowserButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableUCPButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenUCP.Visibility = DisableUCPButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableReleasesButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenGithubReleases.Visibility = DisableReleasesButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableProjectButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenGithubProject.Visibility = DisableProjectButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableProfileButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenProfilePage.Visibility = DisableProfileButton.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void DisableInformationPopups_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DisableInformationPopups = DisableInformationPopups.IsChecked == true;
        }

        private void DisableWarningPopups_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DisableWarningPopups = DisableWarningPopups.IsChecked == true;
        }

        private void DisableErrorPopups_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DisableErrorPopups = DisableErrorPopups.IsChecked == true;
        }

        private void IgnoreBetaVersions_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IgnoreBetaVersions = IgnoreBetaVersions.IsChecked == true;
        }

        private void ToggleDarkMode_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Infrastructure.StyleController.DarkMode = ToggleDarkMode.IsChecked == true;
            Properties.Settings.Default.Save();

            Infrastructure.StyleController.UpdateTheme();
            Timeout.Foreground = _mainWindow.UpdateCheckProgress.Foreground = ToggleDarkMode.IsChecked == true ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.Black;
        }

        private void Themes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Themes.Items.Count < Infrastructure.StyleController.ValidStyles.Count)
                return;

            Properties.Settings.Default.Theme = Themes.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            Infrastructure.StyleController.UpdateTheme();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetSettings();
            LoadSettings();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProgramSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
            Hide();
        }
    }
}
