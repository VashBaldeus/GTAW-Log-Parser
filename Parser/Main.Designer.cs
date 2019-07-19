namespace Parser
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Logo = new System.Windows.Forms.PictureBox();
            this.FolderPath = new System.Windows.Forms.RichTextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Parsed = new System.Windows.Forms.RichTextBox();
            this.CopyParsedToClipboard = new System.Windows.Forms.Button();
            this.CheckForUpdatesOnStartup = new System.Windows.Forms.CheckBox();
            this.SaveParsed = new System.Windows.Forms.Button();
            this.Parse = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.CheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutomaticBackupSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterChatLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PathLabel = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Counter = new System.Windows.Forms.Label();
            this.RemoveTimestamps = new System.Windows.Forms.CheckBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResumeTrayStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.TrayIconContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Logo
            // 
            resources.ApplyResources(this.Logo, "Logo");
            this.Logo.Name = "Logo";
            this.Logo.TabStop = false;
            this.Logo.Click += new System.EventHandler(this.Logo_Click);
            // 
            // FolderPath
            // 
            resources.ApplyResources(this.FolderPath, "FolderPath");
            this.FolderPath.DetectUrls = false;
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.ShortcutsEnabled = false;
            this.FolderPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FolderPath_MouseClick);
            this.FolderPath.TextChanged += new System.EventHandler(this.FolderPath_TextChanged);
            this.FolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FolderPath_KeyDown);
            // 
            // Browse
            // 
            resources.ApplyResources(this.Browse, "Browse");
            this.Browse.Name = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Parsed
            // 
            resources.ApplyResources(this.Parsed, "Parsed");
            this.Parsed.DetectUrls = false;
            this.Parsed.Name = "Parsed";
            this.Parsed.TextChanged += new System.EventHandler(this.Parsed_TextChanged);
            // 
            // CopyParsedToClipboard
            // 
            resources.ApplyResources(this.CopyParsedToClipboard, "CopyParsedToClipboard");
            this.CopyParsedToClipboard.Name = "CopyParsedToClipboard";
            this.CopyParsedToClipboard.UseVisualStyleBackColor = true;
            this.CopyParsedToClipboard.Click += new System.EventHandler(this.CopyParsedToClipboard_Click);
            // 
            // CheckForUpdatesOnStartup
            // 
            resources.ApplyResources(this.CheckForUpdatesOnStartup, "CheckForUpdatesOnStartup");
            this.CheckForUpdatesOnStartup.Name = "CheckForUpdatesOnStartup";
            this.CheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.CheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.CheckForUpdatesOnStartup_CheckedChanged);
            // 
            // SaveParsed
            // 
            resources.ApplyResources(this.SaveParsed, "SaveParsed");
            this.SaveParsed.Name = "SaveParsed";
            this.SaveParsed.UseVisualStyleBackColor = true;
            this.SaveParsed.Click += new System.EventHandler(this.SaveParsed_Click);
            // 
            // Parse
            // 
            resources.ApplyResources(this.Parse, "Parse");
            this.Parse.Name = "Parse";
            this.Parse.UseVisualStyleBackColor = true;
            this.Parse.Click += new System.EventHandler(this.Parse_Click);
            // 
            // SaveFileDialog
            // 
            resources.ApplyResources(this.SaveFileDialog, "SaveFileDialog");
            // 
            // MenuStrip
            // 
            resources.ApplyResources(this.MenuStrip, "MenuStrip");
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckForUpdatesToolStripMenuItem,
            this.AutomaticBackupSettingsToolStripMenuItem,
            this.FilterChatLogToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.AboutToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.MenuStrip.Name = "MenuStrip";
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            resources.ApplyResources(this.CheckForUpdatesToolStripMenuItem, "CheckForUpdatesToolStripMenuItem");
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // AutomaticBackupSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.AutomaticBackupSettingsToolStripMenuItem, "AutomaticBackupSettingsToolStripMenuItem");
            this.AutomaticBackupSettingsToolStripMenuItem.Name = "AutomaticBackupSettingsToolStripMenuItem";
            this.AutomaticBackupSettingsToolStripMenuItem.Click += new System.EventHandler(this.AutomaticBackupSettingsToolStripMenuItem_Click);
            // 
            // FilterChatLogToolStripMenuItem
            // 
            resources.ApplyResources(this.FilterChatLogToolStripMenuItem, "FilterChatLogToolStripMenuItem");
            this.FilterChatLogToolStripMenuItem.Name = "FilterChatLogToolStripMenuItem";
            this.FilterChatLogToolStripMenuItem.Click += new System.EventHandler(this.FilterChatLogToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            // 
            // AboutToolStripMenuItem
            // 
            resources.ApplyResources(this.AboutToolStripMenuItem, "AboutToolStripMenuItem");
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            resources.ApplyResources(this.ExitToolStripMenuItem, "ExitToolStripMenuItem");
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // PathLabel
            // 
            resources.ApplyResources(this.PathLabel, "PathLabel");
            this.PathLabel.Name = "PathLabel";
            // 
            // Version
            // 
            resources.ApplyResources(this.Version, "Version");
            this.Version.Name = "Version";
            // 
            // Counter
            // 
            resources.ApplyResources(this.Counter, "Counter");
            this.Counter.Name = "Counter";
            // 
            // RemoveTimestamps
            // 
            resources.ApplyResources(this.RemoveTimestamps, "RemoveTimestamps");
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            this.RemoveTimestamps.CheckedChanged += new System.EventHandler(this.RemoveTimestamps_CheckedChanged);
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel.Name = "StatusLabel";
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.TrayIcon, "TrayIcon");
            this.TrayIcon.ContextMenuStrip = this.TrayIconContextMenuStrip;
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // TrayIconContextMenuStrip
            // 
            resources.ApplyResources(this.TrayIconContextMenuStrip, "TrayIconContextMenuStrip");
            this.TrayIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResumeTrayStripMenuItem,
            this.ExitTrayToolStripMenuItem});
            this.TrayIconContextMenuStrip.Name = "TrayIconContextMenuStrip";
            // 
            // ResumeTrayStripMenuItem
            // 
            resources.ApplyResources(this.ResumeTrayStripMenuItem, "ResumeTrayStripMenuItem");
            this.ResumeTrayStripMenuItem.Name = "ResumeTrayStripMenuItem";
            this.ResumeTrayStripMenuItem.Click += new System.EventHandler(this.ResumeTrayStripMenuItem_Click);
            // 
            // ExitTrayToolStripMenuItem
            // 
            resources.ApplyResources(this.ExitTrayToolStripMenuItem, "ExitTrayToolStripMenuItem");
            this.ExitTrayToolStripMenuItem.Name = "ExitTrayToolStripMenuItem";
            this.ExitTrayToolStripMenuItem.Click += new System.EventHandler(this.ExitTrayToolStripMenuItem_Click);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.RemoveTimestamps);
            this.Controls.Add(this.Counter);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.Parse);
            this.Controls.Add(this.SaveParsed);
            this.Controls.Add(this.CheckForUpdatesOnStartup);
            this.Controls.Add(this.CopyParsedToClipboard);
            this.Controls.Add(this.Parsed);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.FolderPath);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.TrayIconContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.RichTextBox FolderPath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.RichTextBox Parsed;
        private System.Windows.Forms.Button CopyParsedToClipboard;
        private System.Windows.Forms.CheckBox CheckForUpdatesOnStartup;
        private System.Windows.Forms.Button SaveParsed;
        private System.Windows.Forms.Button Parse;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckForUpdatesToolStripMenuItem;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label Counter;
        private System.Windows.Forms.CheckBox RemoveTimestamps;
        private System.Windows.Forms.ToolStripMenuItem AutomaticBackupSettingsToolStripMenuItem;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ExitTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResumeTrayStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterChatLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
    }
}

