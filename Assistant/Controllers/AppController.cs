using System;
using System.IO;
using System.Windows;
using Assistant.Localization;
using System.Text.RegularExpressions;

namespace Assistant.Controllers
{
    public static class AppController
    {
        public static string PreviousLog = string.Empty;

        /// <summary>
        /// Parses the most recent chat log found at the
        /// selected RAGEMP directory path and returns it.
        /// Displays an error if a chat log does not
        /// exist or if it has an incorrect format
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="removeTimestamps"></param>
        /// <param name="showError"></param>
        /// <returns></returns>
        public static string ParseChatLog(string directoryPath, bool removeTimestamps, bool showError = false)
        {
            ContinuityController.InitializeServerIp();

            try
            {
                // Read the chat log
                string log;
                using (StreamReader sr = new StreamReader(directoryPath + ContinuityController.LogLocation))
                {
                    log = sr.ReadToEnd();
                }

                // Grab the chat log part from the .storage file
                log = Regex.Match(log, "\\\"chatlog\\\":\\\".+\\\\n\\\"").Value;
                if (string.IsNullOrWhiteSpace(log))
                    throw new IndexOutOfRangeException();

                // Comments to the right of these lines
                log = log.Replace("\"chatlog\":\"", string.Empty);  // Remove the chat log indicator
                log = log.Replace("\\n", "\n");                     // Change all occurrences of `\n` into new lines
                log = log.Remove(log.Length - 1, 1);                // Remove the `"` character from the end

                log = System.Net.WebUtility.HtmlDecode(log);    // Decode HTML symbols (example: `&apos;` into `'`)
                log = log.TrimEnd('\r', '\n');                  // Remove the `new line` characters from the end

                PreviousLog = log;
                if (removeTimestamps)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                return log;
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                return string.Empty;
            }
        }
    }
}
