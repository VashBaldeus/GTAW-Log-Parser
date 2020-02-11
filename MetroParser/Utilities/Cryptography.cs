using System;
using System.Text;
using System.Security.Cryptography;

namespace MetroParser.Utilities
{
    public static class Cryptography
    {
        public static void SaveParsedHash(string log, bool isManual)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMD5Hash(md5Hash, log);

                string lastHash = Properties.Settings.Default.LastParsedHash;
                string lastManualHash = Properties.Settings.Default.LastParsedManualHash;
                string lastAutoHash = Properties.Settings.Default.LastParsedAutoHash;

                if (isManual)
                {
                    Properties.Settings.Default.SameHashManualCount = lastManualHash == hash ? Properties.Settings.Default.SameHashManualCount + 1 : 1;
                    Properties.Settings.Default.LastParsedManualHash = hash;
                }
                else
                {
                    Properties.Settings.Default.SameHashAutoCount = lastAutoHash == hash ? Properties.Settings.Default.SameHashAutoCount + 1 : 1;
                    Properties.Settings.Default.LastParsedAutoHash = hash;
                }

                Properties.Settings.Default.SameHashCount = lastHash == hash ? Properties.Settings.Default.SameHashCount + 1 : 1;
                Properties.Settings.Default.LastParsedHash = hash;
                Properties.Settings.Default.Save();
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
