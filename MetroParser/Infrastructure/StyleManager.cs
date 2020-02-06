using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroParser.Infrastructure
{
    public static class StyleManager
    {
        public const string DefaultLightStyle = "Amber";
        public const string DefaultDarkStyle = "Amber";
        public static bool DarkMode
        {
            get { return Properties.Settings.Default.DarkMode; }
            set
            {
                Properties.Settings.Default.DarkMode = value;
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

        public static string GetValidStyle(string style)
        {
            if (style.ToLower() == "default")
                return DarkMode ? DefaultDarkStyle : DefaultLightStyle;

            return ValidStyles.Contains(style) ? style : (DarkMode ? DefaultDarkStyle : DefaultLightStyle);
        }

        public static void UpdateTheme()
        {
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent(GetValidStyle(Properties.Settings.Default.Theme)),
                                        ThemeManager.GetAppTheme(DarkMode ? "BaseDark" : "BaseLight"));
        }
    }
}
