using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    public partial class LanguagePicker : Form
    {
        private bool isStarting = false;
        private bool handleListChange = false;

        public LanguagePicker()
        {
            InitializeComponent();

            foreach (LocalizationManager.Language language in (LocalizationManager.Language[])Enum.GetValues(typeof(LocalizationManager.Language)))
                languageList.Items.Add(language.ToString());

            languageList.SelectedIndex = 0;
            handleListChange = true;
        }

        public void Initialize()
        {
            StartButton.Focus();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LanguageCode = LocalizationManager.GetLanguage();
            Properties.Settings.Default.Save();

            isStarting = true;
            Close();
        }

        private void LanguagePicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isStarting)
                Application.Exit();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            welcomeLabel.Text = welcomeLabel.Text = welcomeLabel.Text.Substring(1, welcomeLabel.Text.Length - 1) + welcomeLabel.Text.Substring(0, 1); ;
        }

        private void LanguageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!handleListChange)
                return;

            LocalizationManager.SetLanguage((LocalizationManager.Language)languageList.SelectedIndex, save: false);

            Text = Localization.Strings.Language;
            StartButton.Text = Localization.Strings.Start;
        }
    }
}
