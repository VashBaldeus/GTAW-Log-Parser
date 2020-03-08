using System;
using System.IO;
using System.Diagnostics;
using Parser.Controllers;
using Parser.Localization;
using System.Windows.Forms;

namespace Parser.UI
{
    public partial class Main : Form
    {
        /// <summary>
        /// Constructor for the main user form
        /// </summary>
        public Main()
        {
            InitializeComponent();

            LoadSettings();
            SetupServerList();
        }

        /// <summary>
        /// Adds menu options under "Server" on the menu
        /// strip for each Language in @LocalizationController
        /// </summary>
        private void SetupServerList()
        {
            // Get the current Language to add a check on
            // the option and loop through the Languages enum
            var currentLanguage = LocalizationController.GetLanguageFromCode(LocalizationController.GetLanguage());
            for (var i = 0; i < ((LocalizationController.Language[])Enum.GetValues(typeof(LocalizationController.Language))).Length; ++i)
            {
                // Add the menu option and the click event
                var language = (LocalizationController.Language)i;
                var newLanguage = ServerToolStripMenuItem.DropDownItems.Add(language.ToString());
                newLanguage.Click += (s, e) =>
                {
                    // No need to do anything if the current language
                    // is clicked on since that won't change anything
                    if (((ToolStripMenuItem)newLanguage).Checked)
                        return;

                    // Make sure the user wants to switch
                    if (MessageBox.Show(Strings.SwitchServer, Strings.Restart, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes) return;
                    LocalizationController.SetLanguage(language);

                    // Restart the program
                    var startInfo = Process.GetCurrentProcess().StartInfo;
                    startInfo.FileName = Application.ExecutablePath;
                    startInfo.Arguments = $"{ContinuityController.ParameterPrefix}restart";
                    var exit = typeof(Application).GetMethod("ExitInternal",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Static);
                    exit?.Invoke(null, null);
                    Process.Start(startInfo);
                };

                // Check the current Language
                if (currentLanguage == language.ToString())
                    ((ToolStripMenuItem)ServerToolStripMenuItem.DropDownItems[i]).Checked = true;
            }
        }

        /// <summary>
        /// Saves the dynamic information
        /// on the main form to the settings
        /// </summary>
        private void SaveSettings()
        {
            Properties.Settings.Default.DirectoryPath = FolderPath.Text;
            Properties.Settings.Default.RemoveTimestamps = RemoveTimestamps.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads the settings into the
        /// dynamic controls on the main form
        /// </summary>
        private void LoadSettings()
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once UnreachableCode
#pragma warning disable 162
            Version.Text = string.Format(Strings.VersionInfo, ContinuityController.Version, ContinuityController.IsBetaVersion ? Strings.BetaShort : string.Empty);
#pragma warning restore 162
            FolderPath.Text = Properties.Settings.Default.DirectoryPath;
            RemoveTimestamps.Checked = Properties.Settings.Default.RemoveTimestamps;
        }

        /// <summary>
        /// Doesn't allow input in the
        /// directory path text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Opens the directory picker
        /// when the text box is clicked on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text))
                Browse_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Saves the settings when the
        /// value of the text box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderPath_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Displays a directory picker until
        /// a non-root directory is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = Path.GetPathRoot(Environment.SystemDirectory);

            var validLocation = false;
            while (!validLocation)
            {
                if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    if (FolderBrowserDialog.SelectedPath[FolderBrowserDialog.SelectedPath.Length - 1] != '\\')
                    {
                        FolderPath.Text = FolderBrowserDialog.SelectedPath + @"\";
                        validLocation = true;
                    }
                    else
                        MessageBox.Show(Strings.BadFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    validLocation = true;
            }
        }

        /// <summary>
        /// Attempts to parse the
        /// current chat log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parse_Click(object sender, EventArgs e)
        {
            // The paths may have changed since the program has
            // started, we need to initialize the locations again
            ContinuityController.InitializeMemory();

            if (string.IsNullOrWhiteSpace(FolderPath.Text) || !Directory.Exists(FolderPath.Text + "client_resources\\"))
            {
                MessageBox.Show(Strings.InvalidFolderPath, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(FolderPath.Text + ContinuityController.LogLocation))
            {
                MessageBox.Show(Strings.NoChatLog, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Parsed.Text = ProgramController.ParseChatLog(FolderPath.Text, RemoveTimestamps.Checked);
        }

        /// <summary>
        /// Displays a save file dialog to save the
        /// contents of the main text box to the disk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParsed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Parsed.Text))
                return;

            try
            {
                SaveFileDialog.FileName = "chatlog.txt";
                SaveFileDialog.Filter = @"Text File | *.txt";

                if (SaveFileDialog.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(SaveFileDialog.OpenFile()))
                {
                    sw.Write(Parsed.Text.Replace("\n", Environment.NewLine));
                }
            }
            catch
            {
                MessageBox.Show(Strings.SaveError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copies the contents of the
        /// main text box to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyParsedToClipboard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Parsed.Text))
                Clipboard.SetText(Parsed.Text.Replace("\n", Environment.NewLine));
        }

        /// <summary>
        /// Saves the settings before
        /// the main form closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Displays some information about the program
        /// when the About menu strip option is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ReSharper disable once UnreachableCode
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
#pragma warning disable 162
            MessageBox.Show(string.Format(Strings.About, ContinuityController.Version, ContinuityController.IsBetaVersion ? Strings.Beta : string.Empty, LocalizationController.GetLanguageFromCode(LocalizationController.GetLanguage()), ContinuityController.ServerIPs[0], ContinuityController.ServerIPs[1]), Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
#pragma warning restore 162
        }
    }
}
