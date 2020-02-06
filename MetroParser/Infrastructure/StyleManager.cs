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
        public const string DefaultStyle = "Amber";
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
                return DefaultStyle;

            return ValidStyles.Contains(style) ? style : DefaultStyle;
        }

        public static void UpdateTheme()
        {
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent(GetValidStyle(Properties.Settings.Default.Theme)),
                                        ThemeManager.GetAppTheme(Properties.Settings.Default.DarkMode ? "BaseDark" : "BaseLight"));
        }
    }
}
