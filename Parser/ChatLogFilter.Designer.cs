namespace Parser
{
    partial class ChatLogFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatLogFilter));
            this.LoadUnparsed = new System.Windows.Forms.Button();
            this.BrowseForParsed = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Words = new System.Windows.Forms.RichTextBox();
            this.Filtered = new System.Windows.Forms.RichTextBox();
            this.Filter = new System.Windows.Forms.Button();
            this.SaveFiltered = new System.Windows.Forms.Button();
            this.CopyFilteredToClipboard = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.RemoveTimestamps = new System.Windows.Forms.CheckBox();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.WordsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadUnparsed
            // 
            resources.ApplyResources(this.LoadUnparsed, "LoadUnparsed");
            this.LoadUnparsed.Name = "LoadUnparsed";
            this.LoadUnparsed.UseVisualStyleBackColor = true;
            this.LoadUnparsed.Click += new System.EventHandler(this.LoadUnparsed_Click);
            // 
            // BrowseForParsed
            // 
            resources.ApplyResources(this.BrowseForParsed, "BrowseForParsed");
            this.BrowseForParsed.Name = "BrowseForParsed";
            this.BrowseForParsed.UseVisualStyleBackColor = true;
            this.BrowseForParsed.Click += new System.EventHandler(this.BrowseForParsed_Click);
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel.Name = "StatusLabel";
            // 
            // Words
            // 
            resources.ApplyResources(this.Words, "Words");
            this.Words.DetectUrls = false;
            this.Words.Name = "Words";
            // 
            // Filtered
            // 
            resources.ApplyResources(this.Filtered, "Filtered");
            this.Filtered.DetectUrls = false;
            this.Filtered.Name = "Filtered";
            // 
            // Filter
            // 
            resources.ApplyResources(this.Filter, "Filter");
            this.Filter.Name = "Filter";
            this.Filter.UseVisualStyleBackColor = true;
            this.Filter.Click += new System.EventHandler(this.Filter_Click);
            // 
            // SaveFiltered
            // 
            resources.ApplyResources(this.SaveFiltered, "SaveFiltered");
            this.SaveFiltered.Name = "SaveFiltered";
            this.SaveFiltered.UseVisualStyleBackColor = true;
            this.SaveFiltered.Click += new System.EventHandler(this.SaveFiltered_Click);
            // 
            // CopyFilteredToClipboard
            // 
            resources.ApplyResources(this.CopyFilteredToClipboard, "CopyFilteredToClipboard");
            this.CopyFilteredToClipboard.Name = "CopyFilteredToClipboard";
            this.CopyFilteredToClipboard.UseVisualStyleBackColor = true;
            this.CopyFilteredToClipboard.Click += new System.EventHandler(this.CopyFilteredToClipboard_Click);
            // 
            // SaveFileDialog
            // 
            resources.ApplyResources(this.SaveFileDialog, "SaveFileDialog");
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "chatlog.txt";
            resources.ApplyResources(this.OpenFileDialog, "OpenFileDialog");
            // 
            // RemoveTimestamps
            // 
            resources.ApplyResources(this.RemoveTimestamps, "RemoveTimestamps");
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            this.RemoveTimestamps.CheckedChanged += new System.EventHandler(this.RemoveTimestamps_CheckedChanged);
            // 
            // TimeLabel
            // 
            resources.ApplyResources(this.TimeLabel, "TimeLabel");
            this.TimeLabel.Name = "TimeLabel";
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // WordsLabel
            // 
            resources.ApplyResources(this.WordsLabel, "WordsLabel");
            this.WordsLabel.Name = "WordsLabel";
            // 
            // ChatLogFilter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WordsLabel);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.RemoveTimestamps);
            this.Controls.Add(this.CopyFilteredToClipboard);
            this.Controls.Add(this.SaveFiltered);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.Filtered);
            this.Controls.Add(this.Words);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.BrowseForParsed);
            this.Controls.Add(this.LoadUnparsed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChatLogFilter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatLogFilter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadUnparsed;
        private System.Windows.Forms.Button BrowseForParsed;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.RichTextBox Words;
        private System.Windows.Forms.RichTextBox Filtered;
        private System.Windows.Forms.Button Filter;
        private System.Windows.Forms.Button SaveFiltered;
        private System.Windows.Forms.Button CopyFilteredToClipboard;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.CheckBox RemoveTimestamps;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label WordsLabel;
    }
}