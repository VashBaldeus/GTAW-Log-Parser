using System;
using System.Windows.Forms;

using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Parser
{
    public partial class Main : Form
    {
        private static readonly string path = Data.logLogation;

        public Main()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Path = Path.Text;
            Properties.Settings.Default.CheckForUpdates = CheckForUpdatesOnStartup.Checked;

            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            Path.Text = Properties.Settings.Default.Path;
            CheckForUpdatesOnStartup.Checked = Properties.Settings.Default.CheckForUpdates;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = System.IO.Path.GetPathRoot(Environment.SystemDirectory),
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Path.Text = dialog.FileName;
                Path.Focus();
                Path.SelectionStart = Path.Text.Length;
                SaveSettings();
            }
        }

        private void Path_MouseClick(object sender, MouseEventArgs e)
        {
            if (Path.Text.Length == 0)
            {
                Browse_Click(this, EventArgs.Empty);
            }
        }

        private void TryToParseLog(string folder)
        {
            new Thread(() =>
            {
                try
                {
                    StreamReader sr = new StreamReader(folder + path);
                    string log = sr.ReadToEnd();
                    sr.Close();

                    log = log.Remove(0, 2);
                    log = log.Remove(log.Length - 2, 2);

                    log = Regex.Replace(log, "{endl}\",\"", Environment.NewLine);
                    log = Regex.Replace(log, "~[a-zA-Z]~", "");

                    log = log.Remove(log.Length - 6, 6);

                    SetText(log);
                }
                catch
                {
                    MessageBox.Show("An error occured while parsing the chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }).Start();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (Parsed.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                Parsed.Text = text;
            }
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            if (Path.Text.Length == 0)
            {
                MessageBox.Show("Invalid RAGEMP folder path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!File.Exists(Path.Text + path))
            {
                MessageBox.Show("Can't find the GTA World chat log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TryToParseLog(Path.Text);
        }

        private void CheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();

            if (CheckForUpdatesOnStartup.Checked)
                CheckForUpdates();
        }

        private void CheckForUpdates(bool checking = false)
        {
            new Thread(() =>
            {
                try
                {
                    float installedVersion = Properties.Settings.Default.Version;

                    string unparsedWebVersion = "";

                    string url = Data.versionLocation;
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);

                    var metaTags = doc.DocumentNode.SelectNodes("//meta");
                    foreach (var tag in metaTags)
                    {
                        var tagName = tag.Attributes["name"];
                        var tagContent = tag.Attributes["content"];
                        var tagProperty = tag.Attributes["property"];
                        if (tagProperty != null && tagContent != null)
                        {
                            switch (tagProperty.Value.ToLower())
                            {
                                case "og:description":
                                    unparsedWebVersion = tagContent.Value;
                                    break;
                            }
                        }
                    }

                    var match = Regex.Match(unparsedWebVersion, @"([-+]?[0-9]*\.?[0-9]+)");
                    float currentVersion = Convert.ToSingle(match.Groups[1].Value);

                    if (currentVersion > installedVersion)
                        MessageBox.Show($"A new version of the chat log parser is now available on the GTA World forums.\n\nCurrent Version: {installedVersion}\nNew Version: {currentVersion}", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (checking)
                        MessageBox.Show("You are running the latest version of the parser.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch
                {
                    if (checking)
                        MessageBox.Show("You are running the latest version of the parser.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }).Start();
        }

        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length != 0)
                Clipboard.SetText(Parsed.Text);
        }

        private void SaveParsed_Click(object sender, EventArgs e)
        {
            if (Parsed.Text.Length == 0)
                return;

            SaveFileDialog.FileName = "chatlog.txt";
            SaveFileDialog.Filter = "Text File | *.txt";

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(SaveFileDialog.OpenFile());
                sw.Write(Parsed.Text);

                sw.Dispose();
                sw.Close();
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"This chat log parser was created by Maple.\n\nInstalled Version: {Properties.Settings.Default.Version}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates(true);
        }
    }
}
