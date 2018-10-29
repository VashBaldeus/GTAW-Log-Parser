namespace Parser
{
    partial class BackupSettings
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
            this.CloseWindow = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.BackupPathLabel = new System.Windows.Forms.Label();
            this.BackupPath = new System.Windows.Forms.RichTextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.BackUpChatLogAutomatically = new System.Windows.Forms.CheckBox();
            this.EnableIntervalBackup = new System.Windows.Forms.CheckBox();
            this.IntervalLabel1 = new System.Windows.Forms.Label();
            this.IntervalLabel2 = new System.Windows.Forms.Label();
            this.Interval = new System.Windows.Forms.NumericUpDown();
            this.RemoveTimestamps = new System.Windows.Forms.CheckBox();
            this.StartWithWindows = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseWindow
            // 
            this.CloseWindow.Location = new System.Drawing.Point(337, 143);
            this.CloseWindow.Name = "CloseWindow";
            this.CloseWindow.Size = new System.Drawing.Size(75, 25);
            this.CloseWindow.TabIndex = 6;
            this.CloseWindow.Text = "Close";
            this.CloseWindow.UseVisualStyleBackColor = true;
            this.CloseWindow.Click += new System.EventHandler(this.CloseWindow_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(256, 143);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 25);
            this.Reset.TabIndex = 7;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // BackupPathLabel
            // 
            this.BackupPathLabel.AutoSize = true;
            this.BackupPathLabel.Location = new System.Drawing.Point(9, 13);
            this.BackupPathLabel.Name = "BackupPathLabel";
            this.BackupPathLabel.Size = new System.Drawing.Size(72, 13);
            this.BackupPathLabel.TabIndex = 0;
            this.BackupPathLabel.Text = "Backup Path:";
            // 
            // BackupPath
            // 
            this.BackupPath.DetectUrls = false;
            this.BackupPath.Location = new System.Drawing.Point(12, 29);
            this.BackupPath.MaxLength = 1024;
            this.BackupPath.Multiline = false;
            this.BackupPath.Name = "BackupPath";
            this.BackupPath.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.BackupPath.ShortcutsEnabled = false;
            this.BackupPath.Size = new System.Drawing.Size(320, 22);
            this.BackupPath.TabIndex = 1;
            this.BackupPath.Text = "";
            this.BackupPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BackupPath_MouseClick);
            this.BackupPath.TextChanged += new System.EventHandler(this.BackupPath_TextChanged);
            this.BackupPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BackupPath_KeyDown);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(338, 27);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 25);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // BackUpChatLogAutomatically
            // 
            this.BackUpChatLogAutomatically.AutoSize = true;
            this.BackUpChatLogAutomatically.Location = new System.Drawing.Point(13, 58);
            this.BackUpChatLogAutomatically.Name = "BackUpChatLogAutomatically";
            this.BackUpChatLogAutomatically.Size = new System.Drawing.Size(354, 17);
            this.BackUpChatLogAutomatically.TabIndex = 2;
            this.BackUpChatLogAutomatically.Text = "Parse and back up the chat log automatically (when the game closes)";
            this.BackUpChatLogAutomatically.UseVisualStyleBackColor = true;
            this.BackUpChatLogAutomatically.CheckedChanged += new System.EventHandler(this.BackUpChatLogAutomatically_CheckedChanged);
            // 
            // EnableIntervalBackup
            // 
            this.EnableIntervalBackup.AutoSize = true;
            this.EnableIntervalBackup.Enabled = false;
            this.EnableIntervalBackup.Location = new System.Drawing.Point(13, 81);
            this.EnableIntervalBackup.Name = "EnableIntervalBackup";
            this.EnableIntervalBackup.Size = new System.Drawing.Size(400, 17);
            this.EnableIntervalBackup.TabIndex = 3;
            this.EnableIntervalBackup.Text = "Back up the chat log automatically while the game is running (every 10 minutes)";
            this.EnableIntervalBackup.UseVisualStyleBackColor = true;
            this.EnableIntervalBackup.CheckedChanged += new System.EventHandler(this.EnableIntervalBackup_CheckedChanged);
            // 
            // IntervalLabel1
            // 
            this.IntervalLabel1.AutoSize = true;
            this.IntervalLabel1.Location = new System.Drawing.Point(9, 107);
            this.IntervalLabel1.Name = "IntervalLabel1";
            this.IntervalLabel1.Size = new System.Drawing.Size(117, 13);
            this.IntervalLabel1.TabIndex = 0;
            this.IntervalLabel1.Text = "Back up chat log every";
            // 
            // IntervalLabel2
            // 
            this.IntervalLabel2.AutoSize = true;
            this.IntervalLabel2.Location = new System.Drawing.Point(162, 107);
            this.IntervalLabel2.Name = "IntervalLabel2";
            this.IntervalLabel2.Size = new System.Drawing.Size(122, 13);
            this.IntervalLabel2.TabIndex = 0;
            this.IntervalLabel2.Text = "minutes. (recommended)";
            // 
            // Interval
            // 
            this.Interval.Enabled = false;
            this.Interval.Location = new System.Drawing.Point(126, 105);
            this.Interval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.Interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Interval.Name = "Interval";
            this.Interval.Size = new System.Drawing.Size(33, 20);
            this.Interval.TabIndex = 4;
            this.Interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Interval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Interval.ValueChanged += new System.EventHandler(this.Interval_ValueChanged);
            // 
            // RemoveTimestamps
            // 
            this.RemoveTimestamps.AutoSize = true;
            this.RemoveTimestamps.Enabled = false;
            this.RemoveTimestamps.Location = new System.Drawing.Point(12, 131);
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.Size = new System.Drawing.Size(183, 17);
            this.RemoveTimestamps.TabIndex = 5;
            this.RemoveTimestamps.Text = "Remove timestamps from backup";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            // 
            // StartWithWindows
            // 
            this.StartWithWindows.AutoSize = true;
            this.StartWithWindows.Enabled = false;
            this.StartWithWindows.Location = new System.Drawing.Point(12, 154);
            this.StartWithWindows.Name = "StartWithWindows";
            this.StartWithWindows.Size = new System.Drawing.Size(117, 17);
            this.StartWithWindows.TabIndex = 8;
            this.StartWithWindows.Text = "Start with Windows";
            this.StartWithWindows.UseVisualStyleBackColor = true;
            // 
            // BackupSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 180);
            this.Controls.Add(this.StartWithWindows);
            this.Controls.Add(this.RemoveTimestamps);
            this.Controls.Add(this.Interval);
            this.Controls.Add(this.IntervalLabel2);
            this.Controls.Add(this.IntervalLabel1);
            this.Controls.Add(this.EnableIntervalBackup);
            this.Controls.Add(this.BackUpChatLogAutomatically);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.BackupPath);
            this.Controls.Add(this.BackupPathLabel);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.CloseWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Automatic Backup Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BackupSettings_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseWindow;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Label BackupPathLabel;
        private System.Windows.Forms.RichTextBox BackupPath;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.CheckBox BackUpChatLogAutomatically;
        private System.Windows.Forms.CheckBox EnableIntervalBackup;
        private System.Windows.Forms.Label IntervalLabel1;
        private System.Windows.Forms.Label IntervalLabel2;
        private System.Windows.Forms.NumericUpDown Interval;
        private System.Windows.Forms.CheckBox RemoveTimestamps;
        private System.Windows.Forms.CheckBox StartWithWindows;
    }
}