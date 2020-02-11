using MahApps.Metro;
using System;
using System.Collections.Generic;
using MetroParser.Utilities;
using System.Windows;
using System.Windows.Media;

namespace MetroParser.Infrastructure
{
    public static class StyleController
    {
        public const string DefaultLightStyle = "Amber";
        public const string DefaultDarkStyle = "Cyan";

        public static bool DarkMode
        {
            get { return Properties.Settings.Default.DarkMode; }
            set
            {
                Properties.Settings.Default.DarkMode = value;
                Properties.Settings.Default.Save();
            }
        }

        public static string Style
        {
            get { return Properties.Settings.Default.Theme; }
            set
            {
                Properties.Settings.Default.Theme = value;
                Properties.Settings.Default.Save();
            }
        }

        public static readonly List<string> ValidStyles = new List<string>
        {
            "Default",
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Orange",
            "Lime",
            "Emerald",
            "Teal",
            "Cyan",
            "Cobalt",
            "Indigo",
            "Violet",
            "Pink",
            "Magenta",
            "Crimson",
            "Amber",
            "Yellow",
            "Brown",
            "Olive",
            "Steel",
            "Mauve",
            "Taupe",
            "Sienna"
        };

        public static void InitializeFollowEligibility()
        {
            try
            {
                var keyValue = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
                Data.CanFollowSystemMode = keyValue != null;
            }
            catch
            {
                Data.CanFollowSystemMode = false;
            }

            try
            {
                var keyValue = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                Data.CanFollowSystemColor = keyValue != null && Data.CanFollowSystemMode;
            }
            catch
            {
                Data.CanFollowSystemColor = false;
            }
        }

        private static Color GetIdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }

        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }

        public static Accent GetSystemAccent()
        {
            try
            {
                Color color = SystemParameters.WindowGlassColor;
                var keyValue = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                if (keyValue != null)
                {
                    var bytes = BitConverter.GetBytes((UInt32)(int)keyValue);
                    color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                }

                #pragma warning disable IDE0028 // Simplify collection initialization
                ResourceDictionary resourceDictionary = new ResourceDictionary();
                #pragma warning restore IDE0028 // Simplify collection initialization
                resourceDictionary.Add("HighlightColor", color);
                resourceDictionary.Add("AccentBaseColor", color);
                resourceDictionary.Add("AccentColor", Color.FromArgb((byte)(204), color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor2", Color.FromArgb((byte)(153), color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor3", Color.FromArgb((byte)(102), color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor4", Color.FromArgb((byte)(51), color.R, color.G, color.B));

                resourceDictionary.Add("HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["HighlightColor"]));
                resourceDictionary.Add("AccentBaseColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentBaseColor"]));
                resourceDictionary.Add("AccentColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("AccentColorBrush2", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("AccentColorBrush3", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
                resourceDictionary.Add("AccentColorBrush4", GetSolidColorBrush((Color)resourceDictionary["AccentColor4"]));

                resourceDictionary.Add("WindowTitleColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

                resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(
                    new GradientStopCollection(new[]
                    {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                    }),
                    startPoint: new Point(1.002, 0.5), endPoint: new Point(0.001, 0.5)));

                resourceDictionary.Add("CheckmarkFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("RightArrowFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

                resourceDictionary.Add("IdealForegroundColor", GetIdealTextColor(color));
                resourceDictionary.Add("IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
                resourceDictionary.Add("IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"], 0.4));
                resourceDictionary.Add("AccentSelectedColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                resourceDictionary.Add("MetroDataGrid.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
                resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
                resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                return ThemeManager.GetAccent(resourceDictionary);
            }
            catch
            {
                Data.CanFollowSystemColor = false;
                ValidStyles.Remove("Windows");

                Properties.Settings.Default.FollowSystemColor = false;
                Properties.Settings.Default.Save();

                return ThemeManager.GetAccent(DarkMode ? DefaultDarkStyle : DefaultLightStyle);
            }
        }

        public static bool GetAppMode()
        {
            bool darkMode = false;
            try
            {
                var keyValue = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
                if (keyValue != null && (UInt32)(int)keyValue == 0)
                    darkMode = true;
            }
            catch
            {
                Data.CanFollowSystemMode = false;

                Properties.Settings.Default.FollowSystemMode = false;
                Properties.Settings.Default.Save();
            }

            return darkMode;
        }

        public static void UpdateTheme()
        {
            if (!ValidStyles.Contains(Style))
                Style = "Default";

            ThemeManager.ChangeAppStyle(Application.Current,
                                        Style == "Windows" ? GetSystemAccent() : ThemeManager.GetAccent(Style == "Default" ? (DarkMode ? DefaultDarkStyle : DefaultLightStyle) : Style),
                                        ThemeManager.GetAppTheme(DarkMode ? "BaseDark" : "BaseLight"));
        }
    }
}
