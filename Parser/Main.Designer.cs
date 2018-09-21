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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Logo = new System.Windows.Forms.PictureBox();
            this.Path = new System.Windows.Forms.RichTextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Parsed = new System.Windows.Forms.RichTextBox();
            this.CopyParsedToClipboard = new System.Windows.Forms.Button();
            this.CheckForUpdatesOnStartup = new System.Windows.Forms.CheckBox();
            this.SaveParsed = new System.Windows.Forms.Button();
            this.Parse = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.CheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PathLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Logo
            // 
            this.Logo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(12, 33);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(292, 143);
            this.Logo.TabIndex = 0;
            this.Logo.TabStop = false;
            // 
            // Path
            // 
            this.Path.Location = new System.Drawing.Point(310, 43);
            this.Path.MaxLength = 1024;
            this.Path.Multiline = false;
            this.Path.Name = "Path";
            this.Path.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.Path.Size = new System.Drawing.Size(221, 22);
            this.Path.TabIndex = 5;
            this.Path.Text = "";
            this.Path.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Path_MouseClick);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(537, 42);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 4;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Parsed
            // 
            this.Parsed.Location = new System.Drawing.Point(310, 71);
            this.Parsed.Name = "Parsed";
            this.Parsed.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Parsed.Size = new System.Drawing.Size(302, 99);
            this.Parsed.TabIndex = 3;
            this.Parsed.Text = "";
            // 
            // CopyParsedToClipboard
            // 
            this.CopyParsedToClipboard.Location = new System.Drawing.Point(472, 176);
            this.CopyParsedToClipboard.Name = "CopyParsedToClipboard";
            this.CopyParsedToClipboard.Size = new System.Drawing.Size(140, 23);
            this.CopyParsedToClipboard.TabIndex = 2;
            this.CopyParsedToClipboard.Text = "Copy To Clipboard";
            this.CopyParsedToClipboard.UseVisualStyleBackColor = true;
            this.CopyParsedToClipboard.Click += new System.EventHandler(this.CopyParsedToClipboard_Click);
            // 
            // CheckForUpdatesOnStartup
            // 
            this.CheckForUpdatesOnStartup.AutoSize = true;
            this.CheckForUpdatesOnStartup.Location = new System.Drawing.Point(12, 182);
            this.CheckForUpdatesOnStartup.Name = "CheckForUpdatesOnStartup";
            this.CheckForUpdatesOnStartup.Size = new System.Drawing.Size(183, 17);
            this.CheckForUpdatesOnStartup.TabIndex = 6;
            this.CheckForUpdatesOnStartup.Text = "Automatically Check For Updates";
            this.CheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.CheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.CheckForUpdatesOnStartup_CheckedChanged);
            // 
            // SaveParsed
            // 
            this.SaveParsed.Location = new System.Drawing.Point(391, 176);
            this.SaveParsed.Name = "SaveParsed";
            this.SaveParsed.Size = new System.Drawing.Size(75, 23);
            this.SaveParsed.TabIndex = 1;
            this.SaveParsed.Text = "Save As";
            this.SaveParsed.UseVisualStyleBackColor = true;
            this.SaveParsed.Click += new System.EventHandler(this.SaveParsed_Click);
            // 
            // Parse
            // 
            this.Parse.Location = new System.Drawing.Point(310, 176);
            this.Parse.Name = "Parse";
            this.Parse.Size = new System.Drawing.Size(75, 23);
            this.Parse.TabIndex = 0;
            this.Parse.Text = "Parse";
            this.Parse.UseVisualStyleBackColor = true;
            this.Parse.Click += new System.EventHandler(this.Parse_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckForUpdatesToolStripMenuItem,
            this.AboutToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(624, 24);
            this.MenuStrip.TabIndex = 7;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            this.CheckForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
            this.CheckForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
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
            this.PathLabel.Location = new System.Drawing.Point(307, 27);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(88, 13);
            this.PathLabel.TabIndex = 8;
            this.PathLabel.Text = "RAGEMP Folder:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 201);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.Parse);
            this.Controls.Add(this.SaveParsed);
            this.Controls.Add(this.CheckForUpdatesOnStartup);
            this.Controls.Add(this.CopyParsedToClipboard);
            this.Controls.Add(this.Parsed);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Path);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 240);
            this.MinimumSize = new System.Drawing.Size(640, 240);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GTAW Log Parser";
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.RichTextBox Path;
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
    }
}

