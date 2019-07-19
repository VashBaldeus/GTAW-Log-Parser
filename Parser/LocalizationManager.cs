using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parser
{
    class LocalizationManager
    {
        public enum Language {English, Español};

        private static readonly Dictionary<Language, string> languages = new Dictionary<Language, string>
        {
            { Language.English, "en_US" },
            { Language.Español, "es_ES" }
        };

        private static string currentLanguage = string.Empty;

        public static void Initialize(bool save = false)
        {
            if (string.IsNullOrWhiteSpace(currentLanguage))
                currentLanguage = Properties.Settings.Default.LanguageCode;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(currentLanguage);

            if (save)
            {
                Properties.Settings.Default.LanguageCode = currentLanguage;
                Properties.Settings.Default.Save();
            }
        }

        public static string GetLanguage()
        {
            return currentLanguage;
        }

        public static void SetLanguage(Language language, bool save = true)
        {
            if (!languages.ContainsKey(language))
                language = Language.English;

            currentLanguage = languages[language];
            Initialize(save);
        }

        public static string GetLanguageFromCode(string code)
        {
            return languages.FirstOrDefault(x => x.Value == code).Key.ToString();
        }

        public static string GetCodeFromLanguage(Language language)
        {
            if (!languages.ContainsKey(language))
                language = Language.English;

            return languages[language];
        }
    }
}
