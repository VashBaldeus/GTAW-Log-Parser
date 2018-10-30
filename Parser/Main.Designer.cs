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
            this.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(12, 27);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(288, 140);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 0;
            this.Logo.TabStop = false;
            this.Logo.Click += new System.EventHandler(this.Logo_Click);
            // 
            // FolderPath
            // 
            this.FolderPath.DetectUrls = false;
            this.FolderPath.Location = new System.Drawing.Point(309, 43);
            this.FolderPath.MaxLength = 1024;
            this.FolderPath.Multiline = false;
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.FolderPath.ShortcutsEnabled = false;
            this.FolderPath.Size = new System.Drawing.Size(221, 22);
            this.FolderPath.TabIndex = 5;
            this.FolderPath.Text = "";
            this.FolderPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FolderPath_MouseClick);
            this.FolderPath.TextChanged += new System.EventHandler(this.FolderPath_TextChanged);
            this.FolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FolderPath_KeyDown);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(536, 41);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(76, 25);
            this.Browse.TabIndex = 4;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Parsed
            // 
            this.Parsed.DetectUrls = false;
            this.Parsed.Location = new System.Drawing.Point(309, 72);
            this.Parsed.Name = "Parsed";
            this.Parsed.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Parsed.Size = new System.Drawing.Size(303, 95);
            this.Parsed.TabIndex = 3;
            this.Parsed.Text = "";
            this.Parsed.TextChanged += new System.EventHandler(this.Parsed_TextChanged);
            // 
            // CopyParsedToClipboard
            // 
            this.CopyParsedToClipboard.Location = new System.Drawing.Point(471, 186);
            this.CopyParsedToClipboard.Name = "CopyParsedToClipboard";
            this.CopyParsedToClipboard.Size = new System.Drawing.Size(141, 25);
            this.CopyParsedToClipboard.TabIndex = 2;
            this.CopyParsedToClipboard.Text = "Copy To Clipboard";
            this.CopyParsedToClipboard.UseVisualStyleBackColor = true;
            this.CopyParsedToClipboard.Click += new System.EventHandler(this.CopyParsedToClipboard_Click);
            // 
            // CheckForUpdatesOnStartup
            // 
            this.CheckForUpdatesOnStartup.AutoSize = true;
            this.CheckForUpdatesOnStartup.Location = new System.Drawing.Point(12, 178);
            this.CheckForUpdatesOnStartup.Name = "CheckForUpdatesOnStartup";
            this.CheckForUpdatesOnStartup.Size = new System.Drawing.Size(177, 17);
            this.CheckForUpdatesOnStartup.TabIndex = 6;
            this.CheckForUpdatesOnStartup.Text = "Check for updates automatically";
            this.CheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.CheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.CheckForUpdatesOnStartup_CheckedChanged);
            // 
            // SaveParsed
            // 
            this.SaveParsed.Location = new System.Drawing.Point(390, 186);
            this.SaveParsed.Name = "SaveParsed";
            this.SaveParsed.Size = new System.Drawing.Size(75, 25);
            this.SaveParsed.TabIndex = 1;
            this.SaveParsed.Text = "Save As";
            this.SaveParsed.UseVisualStyleBackColor = true;
            this.SaveParsed.Click += new System.EventHandler(this.SaveParsed_Click);
            // 
            // Parse
            // 
            this.Parse.Location = new System.Drawing.Point(309, 186);
            this.Parse.Name = "Parse";
            this.Parse.Size = new System.Drawing.Size(75, 25);
            this.Parse.TabIndex = 0;
            this.Parse.Text = "Parse";
            this.Parse.UseVisualStyleBackColor = true;
            this.Parse.Click += new System.EventHandler(this.Parse_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckForUpdatesToolStripMenuItem,
            this.AutomaticBackupSettingsToolStripMenuItem,
            this.FilterChatLogToolStripMenuItem,
            this.AboutToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(624, 24);
            this.MenuStrip.TabIndex = 8;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            this.CheckForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.CheckForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // AutomaticBackupSettingsToolStripMenuItem
            // 
            this.AutomaticBackupSettingsToolStripMenuItem.Name = "AutomaticBackupSettingsToolStripMenuItem";
            this.AutomaticBackupSettingsToolStripMenuItem.Size = new System.Drawing.Size(162, 20);
            this.AutomaticBackupSettingsToolStripMenuItem.Text = "Automatic Backup Settings";
            this.AutomaticBackupSettingsToolStripMenuItem.Click += new System.EventHandler(this.AutomaticBackupSettingsToolStripMenuItem_Click);
            // 
            // FilterChatLogToolStripMenuItem
            // 
            this.FilterChatLogToolStripMenuItem.Name = "FilterChatLogToolStripMenuItem";
            this.FilterChatLogToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.FilterChatLogToolStripMenuItem.Text = "Filter Chat Log";
            this.FilterChatLogToolStripMenuItem.Click += new System.EventHandler(this.FilterChatLogToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(306, 27);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(88, 13);
            this.PathLabel.TabIndex = 0;
            this.PathLabel.Text = "RAGEMP Folder:";
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(536, 27);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(76, 13);
            this.Version.TabIndex = 0;
            this.Version.Text = "Version: 1.0";
            this.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Counter
            // 
            this.Counter.Location = new System.Drawing.Point(436, 170);
            this.Counter.Name = "Counter";
            this.Counter.Size = new System.Drawing.Size(176, 13);
            this.Counter.TabIndex = 0;
            this.Counter.Text = "0 characters and 0 lines";
            this.Counter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RemoveTimestamps
            // 
            this.RemoveTimestamps.AutoSize = true;
            this.RemoveTimestamps.Location = new System.Drawing.Point(12, 201);
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.Size = new System.Drawing.Size(121, 17);
            this.RemoveTimestamps.TabIndex = 7;
            this.RemoveTimestamps.Text = "Remove timestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            this.RemoveTimestamps.CheckedChanged += new System.EventHandler(this.RemoveTimestamps_CheckedChanged);
            // 
            // StatusLabel
            // 
            this.StatusLabel.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel.Location = new System.Drawing.Point(308, 170);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(121, 13);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "Automatic Backup: OFF";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayIcon.BalloonTipText = "Automatic Backup: ON";
            this.TrayIcon.BalloonTipTitle = "Information";
            this.TrayIcon.ContextMenuStrip = this.TrayIconContextMenuStrip;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "GTA World Chat Log Parser";
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // TrayIconContextMenuStrip
            // 
            this.TrayIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResumeTrayStripMenuItem,
            this.ExitTrayToolStripMenuItem});
            this.TrayIconContextMenuStrip.Name = "TrayIconContextMenuStrip";
            this.TrayIconContextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // ResumeTrayStripMenuItem
            // 
            this.ResumeTrayStripMenuItem.Name = "ResumeTrayStripMenuItem";
            this.ResumeTrayStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ResumeTrayStripMenuItem.Text = "Open";
            this.ResumeTrayStripMenuItem.Click += new System.EventHandler(this.ResumeTrayStripMenuItem_Click);
            // 
            // ExitTrayToolStripMenuItem
            // 
            this.ExitTrayToolStripMenuItem.Name = "ExitTrayToolStripMenuItem";
            this.ExitTrayToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ExitTrayToolStripMenuItem.Text = "Exit";
            this.ExitTrayToolStripMenuItem.Click += new System.EventHandler(this.ExitTrayToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 221);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GTA World Chat Log Parser";
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
    }
}

