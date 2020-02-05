using System;
using System.Text;
using System.Security.Cryptography;

namespace MetroParser.Utils
{
    public static class Crypto
    {
        public static void SaveParsedHash(string log)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMD5Hash(md5Hash, log);
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
