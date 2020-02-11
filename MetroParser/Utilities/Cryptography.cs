using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using MetroParser.Localization;

namespace MetroParser.Utilities
{
    public static class Cryptography
    {
        public static void SaveParsedHash(string log)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMD5Hash(md5Hash, log);

                string lastAutoHash = Properties.Settings.Default.LastParsedAutoHash;

                Properties.Settings.Default.SameHashAutoCount = lastAutoHash == hash ? Properties.Settings.Default.SameHashAutoCount + 1 : 1;
                Properties.Settings.Default.LastParsedAutoHash = hash;
                Properties.Settings.Default.Save();

                if (Properties.Settings.Default.SameHashAutoCount > 2)
                    MessageBox.Show(Strings.SameHashWarning, Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static string GetMD5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static bool IsSameMD5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMD5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
