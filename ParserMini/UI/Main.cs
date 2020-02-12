using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ParserMini.Utilities;
using ParserMini.Infrastructure;
using System.Diagnostics;
using System.Globalization;
using ParserMini.Localization;

namespace ParserMini.UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            LoadSettings();
            SetupServerList();
        }

        private void SetupServerList()
        {
            string currentLanguage = LocalizationController.GetLanguageFromCode(LocalizationController.GetLanguage());
            for (int i = 0; i < ((LocalizationController.Language[])Enum.GetValues(typeof(LocalizationController.Language))).Length; ++i)
            {
                LocalizationController.Language language = (LocalizationController.Language)i;
                ToolStripItem newLanguage = ServerToolStripMenuItem.DropDownItems.Add(language.ToString());
                newLanguage.Click += (s, e) =>
                {
                    if (((ToolStripMenuItem)newLanguage).Checked == true)
                        return;

                    CultureInfo cultureInfo = new CultureInfo(LocalizationController.GetCodeFromLanguage(language));

                    if (MessageBox.Show(Strings.SwitchServer, Strings.Restart, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        LocalizationController.SetLanguage(language);

                        ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
                        startInfo.FileName = Application.ExecutablePath;
                        startInfo.Arguments = $"{Data.ParameterPrefix}restart";
                        var exit = typeof(Application).GetMethod("ExitInternal",
                                            System.Reflection.BindingFlags.NonPublic |
                                            System.Reflection.BindingFlags.Static);
                        exit.Invoke(null, null);
                        Process.Start(startInfo);
                    }
                };

                if (currentLanguage == language.ToString())
                    ((ToolStripMenuItem)ServerToolStripMenuItem.DropDownItems[i]).Checked = true;
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FolderPath = FolderPath.Text;
            Properties.Settings.Default.RemoveTimestamps = RemoveTimestamps.Checked;
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Version.Text = string.Format(Strings.VersionInfo, Data.Version, Data.IsBetaVersion ? Strings.BetaShort : string.Empty);
            FolderPath.Text = Properties.Settings.Default.FolderPath;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestamps;
        }

        private void FolderPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void FolderPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text))
                Browse_Click(this, EventArgs.Empty);
        }

        private void FolderPath_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            bool validLocation = false;
            while (!validLocation)
            {
                if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    if (FolderBrowserDialog.SelectedPath[FolderBrowserDialog.SelectedPath.Length - 1] != '\\')
                    {
                        FolderPath.Text = FolderBrowserDialog.SelectedPath + "\\";
                        validLocation = true;
                    }
                    else
                        MessageBox.Show(Strings.BadFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    validLocation = true;
            }
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            Data.Initialize();

            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(FolderPath.Text + Data.LogLocation))
            {
                MessageBox.Show(Strings.NoChatLog, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Parsed.Text = ParseChatLog(FolderPath.Text, RemoveTimestamps.Checked);
        }

        public static string ParseChatLog(string folderPath, bool removeTimestamps)
        {
            try
            {
                string log;
                using (StreamReader sr = new StreamReader(folderPath + Data.LogLocation))
                {
                    log = sr.ReadToEnd();
                }

                log = Regex.Match(log, "\\\"chatlog\\\":\\\".+\\\\n\\\"").Value;
                if (string.IsNullOrWhiteSpace(log))
                    throw new IndexOutOfRangeException();

                log = log.Replace("\"chatlog\":\"", string.Empty);  // Remove the chat log indicator
                log = log.Replace("\\n", "\n");                     // Change all occurrences of `\n` into new lines
                log = log.Remove(log.Length - 1, 1);                // Remove the `"` character from the end


                log = System.Net.WebUtility.HtmlDecode(log);    // Decode HTML symbols (example: `&apos;` into `'`)
                log = log.TrimEnd(new char[] { '\r', '\n' });   // Remove the `new line` characters from the end

                if (removeTimestamps)
                    log = Regex.Replace(log, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);

                return log;
            }
            catch
            {
                MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void SaveParsed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text))
                return;

            try
            {
                SaveFileDialog.FileName = "chatlog.txt";
                SaveFileDialog.Filter = "Text File | *.txt";

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(SaveFileDialog.OpenFile()))
                    {
                        sw.Write(Parsed.Text.Replace("\n", Environment.NewLine));
                    }
                }
            }
            catch
            {
                MessageBox.Show(Strings.SaveError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Parsed.Text))
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(Strings.About, Data.Version, Data.IsBetaVersion ? Strings.Beta : string.Empty, LocalizationController.GetLanguageFromCode(LocalizationController.GetLanguage()), Data.ServerIPs[0], Data.ServerIPs[1]), Strings.Information, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }
    }
}
