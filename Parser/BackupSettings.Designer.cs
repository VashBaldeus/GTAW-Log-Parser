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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupSettings));
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
            this.SuppressNotifications = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Interval)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseWindow
            // 
            resources.ApplyResources(this.CloseWindow, "CloseWindow");
            this.CloseWindow.Name = "CloseWindow";
            this.CloseWindow.UseVisualStyleBackColor = true;
            this.CloseWindow.Click += new System.EventHandler(this.CloseWindow_Click);
            // 
            // Reset
            // 
            resources.ApplyResources(this.Reset, "Reset");
            this.Reset.Name = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // BackupPathLabel
            // 
            resources.ApplyResources(this.BackupPathLabel, "BackupPathLabel");
            this.BackupPathLabel.Name = "BackupPathLabel";
            // 
            // BackupPath
            // 
            resources.ApplyResources(this.BackupPath, "BackupPath");
            this.BackupPath.DetectUrls = false;
            this.BackupPath.Name = "BackupPath";
            this.BackupPath.ShortcutsEnabled = false;
            this.BackupPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BackupPath_MouseClick);
            this.BackupPath.TextChanged += new System.EventHandler(this.BackupPath_TextChanged);
            this.BackupPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BackupPath_KeyDown);
            // 
            // Browse
            // 
            resources.ApplyResources(this.Browse, "Browse");
            this.Browse.Name = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // BackUpChatLogAutomatically
            // 
            resources.ApplyResources(this.BackUpChatLogAutomatically, "BackUpChatLogAutomatically");
            this.BackUpChatLogAutomatically.Name = "BackUpChatLogAutomatically";
            this.BackUpChatLogAutomatically.UseVisualStyleBackColor = true;
            this.BackUpChatLogAutomatically.CheckedChanged += new System.EventHandler(this.BackUpChatLogAutomatically_CheckedChanged);
            // 
            // EnableIntervalBackup
            // 
            resources.ApplyResources(this.EnableIntervalBackup, "EnableIntervalBackup");
            this.EnableIntervalBackup.Name = "EnableIntervalBackup";
            this.EnableIntervalBackup.UseVisualStyleBackColor = true;
            this.EnableIntervalBackup.CheckedChanged += new System.EventHandler(this.EnableIntervalBackup_CheckedChanged);
            // 
            // IntervalLabel1
            // 
            resources.ApplyResources(this.IntervalLabel1, "IntervalLabel1");
            this.IntervalLabel1.Name = "IntervalLabel1";
            // 
            // IntervalLabel2
            // 
            resources.ApplyResources(this.IntervalLabel2, "IntervalLabel2");
            this.IntervalLabel2.Name = "IntervalLabel2";
            // 
            // Interval
            // 
            resources.ApplyResources(this.Interval, "Interval");
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
            this.Interval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Interval.ValueChanged += new System.EventHandler(this.Interval_ValueChanged);
            // 
            // RemoveTimestamps
            // 
            resources.ApplyResources(this.RemoveTimestamps, "RemoveTimestamps");
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            // 
            // StartWithWindows
            // 
            resources.ApplyResources(this.StartWithWindows, "StartWithWindows");
            this.StartWithWindows.Name = "StartWithWindows";
            this.StartWithWindows.UseVisualStyleBackColor = true;
            this.StartWithWindows.CheckedChanged += new System.EventHandler(this.StartWithWindows_CheckedChanged);
            // 
            // SuppressNotifications
            // 
            resources.ApplyResources(this.SuppressNotifications, "SuppressNotifications");
            this.SuppressNotifications.Name = "SuppressNotifications";
            this.SuppressNotifications.UseVisualStyleBackColor = true;
            // 
            // BackupSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SuppressNotifications);
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
            this.TopMost = true;
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
        private System.Windows.Forms.CheckBox SuppressNotifications;
    }
}