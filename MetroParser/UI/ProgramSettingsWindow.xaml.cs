using System;
using System.Windows;
using MetroParser.Localization;
using MetroParser.Infrastructure;
using MetroParser.Utilities;

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
            StyleController.ValidStyles.Remove("Windows");
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
            Properties.Settings.Default.FollowSystemColor = FollowSystemColor.IsChecked == true;
            Properties.Settings.Default.FollowSystemMode = FollowSystemMode.IsChecked == true;

            StyleController.DarkMode = ToggleDarkMode.IsChecked == true;
            StyleController.Style = Themes.SelectedItem.ToString();

            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
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

            FollowSystemColor.IsChecked = Properties.Settings.Default.FollowSystemColor;
            FollowSystemMode.IsChecked = Properties.Settings.Default.FollowSystemMode;
            FollowSystemColor.IsEnabled = Data.CanFollowSystemColor;
            FollowSystemMode.IsEnabled = Data.CanFollowSystemMode;

            ToggleDarkMode.IsChecked = StyleController.DarkMode;
            ToggleDarkMode.IsEnabled = !Properties.Settings.Default.FollowSystemMode;
            Timeout.Foreground = _mainWindow.UpdateCheckProgress.Foreground = ToggleDarkMode.IsChecked == true ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.Black;

            Themes.IsEnabled = !Properties.Settings.Default.FollowSystemColor;
            UpdateThemeSwitcher();
        }

        private void UpdateThemeSwitcher()
        {
            Themes.Items.Clear();
            foreach (string style in StyleController.ValidStyles)
            {
                Themes.Items.Add(style);
            }
            Themes.SelectedItem = StyleController.Style;
        }

        private void ResetSettings()
        {
            Properties.Settings.Default.DisableForumsButton = true;
            Properties.Settings.Default.DisableFacebrowserButton = true;
            Properties.Settings.Default.DisableUCPButton = true;
            Properties.Settings.Default.DisableReleasesButton= false;
            Properties.Settings.Default.DisableProjectButton = true;
            Properties.Settings.Default.DisableProfileButton = false;
            Properties.Settings.Default.UpdateCheckTimeout = 4;

            Properties.Settings.Default.DisableInformationPopups = false;
            Properties.Settings.Default.DisableWarningPopups = false;
            Properties.Settings.Default.DisableErrorPopups = false;
            Properties.Settings.Default.IgnoreBetaVersions = true;
            Properties.Settings.Default.FollowSystemColor = Data.CanFollowSystemColor;
            Properties.Settings.Default.FollowSystemMode = Data.CanFollowSystemMode;

            //StyleController.DarkMode = Data.CanFollowSystemMode ? StyleController.GetAppMode() : false;
            //StyleController.Style = Data.CanFollowSystemColor ? "Windows" : "Default";

            Properties.Settings.Default.Save();
        }

        private void Timeout_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TimeoutLabel2 == null)
                return;

            TimeoutLabel2.Content = string.Format(Strings.UpdateAbortTime, Timeout.Value > 1 ? Strings.SecondPlural : Strings.SecondSingular);
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

        private void FollowSystemColor_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FollowSystemColor = FollowSystemColor.IsChecked == true;

            Themes.IsEnabled = FollowSystemColor.IsChecked != true;
            if (FollowSystemColor.IsChecked == true)
                StyleController.ValidStyles.Add("Windows");
            else
                StyleController.ValidStyles.Remove("Windows");

            UpdateThemeSwitcher();
            Themes.SelectedItem = FollowSystemColor.IsChecked == true ? "Windows" : "Default";
        }

        private void FollowSystemMode_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.FollowSystemMode = FollowSystemMode.IsChecked == true;

            ToggleDarkMode.IsEnabled = FollowSystemMode.IsChecked != true;
            ToggleDarkMode.IsChecked = FollowSystemMode.IsChecked == true ? StyleController.GetAppMode() : false;
        }

        private void ToggleDarkMode_CheckedChanged(object sender, RoutedEventArgs e)
        {
            StyleController.DarkMode = ToggleDarkMode.IsChecked == true;
            StyleController.UpdateTheme();
            
            Timeout.Foreground = _mainWindow.UpdateCheckProgress.Foreground = ToggleDarkMode.IsChecked == true ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.Black;
        }

        private void Themes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Themes.Items.Count < StyleController.ValidStyles.Count)
                return;

            StyleController.Style = Themes.SelectedItem.ToString();
            StyleController.UpdateTheme();
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
        }
    }
}
