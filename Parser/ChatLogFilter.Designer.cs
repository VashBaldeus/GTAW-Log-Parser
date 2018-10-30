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
            this.LoadUnparsed = new System.Windows.Forms.Button();
            this.BrowseForParsed = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Names = new System.Windows.Forms.RichTextBox();
            this.Filtered = new System.Windows.Forms.RichTextBox();
            this.Filter = new System.Windows.Forms.Button();
            this.SaveFiltered = new System.Windows.Forms.Button();
            this.CopyFilteredToClipboard = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.RemoveTimestamps = new System.Windows.Forms.CheckBox();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.NamesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadUnparsed
            // 
            this.LoadUnparsed.Location = new System.Drawing.Point(12, 25);
            this.LoadUnparsed.Name = "LoadUnparsed";
            this.LoadUnparsed.Size = new System.Drawing.Size(135, 25);
            this.LoadUnparsed.TabIndex = 0;
            this.LoadUnparsed.Text = "Load Unparsed Chat Log";
            this.LoadUnparsed.UseVisualStyleBackColor = true;
            this.LoadUnparsed.Click += new System.EventHandler(this.LoadUnparsed_Click);
            // 
            // BrowseForParsed
            // 
            this.BrowseForParsed.Location = new System.Drawing.Point(153, 25);
            this.BrowseForParsed.Name = "BrowseForParsed";
            this.BrowseForParsed.Size = new System.Drawing.Size(150, 25);
            this.BrowseForParsed.TabIndex = 1;
            this.BrowseForParsed.Text = "Browse For Parsed Chat Log";
            this.BrowseForParsed.UseVisualStyleBackColor = true;
            this.BrowseForParsed.Click += new System.EventHandler(this.BrowseForParsed_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.ForeColor = System.Drawing.Color.Red;
            this.StatusLabel.Location = new System.Drawing.Point(12, 9);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(291, 13);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "Chat log not loaded";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Names
            // 
            this.Names.DetectUrls = false;
            this.Names.Location = new System.Drawing.Point(12, 69);
            this.Names.Name = "Names";
            this.Names.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Names.Size = new System.Drawing.Size(135, 184);
            this.Names.TabIndex = 3;
            this.Names.Text = "Firstname Lastname";
            // 
            // Filtered
            // 
            this.Filtered.DetectUrls = false;
            this.Filtered.Location = new System.Drawing.Point(153, 56);
            this.Filtered.Name = "Filtered";
            this.Filtered.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Filtered.Size = new System.Drawing.Size(277, 197);
            this.Filtered.TabIndex = 4;
            this.Filtered.Text = "";
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(12, 259);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(65, 25);
            this.Filter.TabIndex = 5;
            this.Filter.Text = "Filter";
            this.Filter.UseVisualStyleBackColor = true;
            this.Filter.Click += new System.EventHandler(this.Filter_Click);
            // 
            // SaveFiltered
            // 
            this.SaveFiltered.Location = new System.Drawing.Point(82, 259);
            this.SaveFiltered.Name = "SaveFiltered";
            this.SaveFiltered.Size = new System.Drawing.Size(65, 25);
            this.SaveFiltered.TabIndex = 6;
            this.SaveFiltered.Text = "Save As";
            this.SaveFiltered.UseVisualStyleBackColor = true;
            this.SaveFiltered.Click += new System.EventHandler(this.SaveFiltered_Click);
            // 
            // CopyFilteredToClipboard
            // 
            this.CopyFilteredToClipboard.Location = new System.Drawing.Point(153, 259);
            this.CopyFilteredToClipboard.Name = "CopyFilteredToClipboard";
            this.CopyFilteredToClipboard.Size = new System.Drawing.Size(277, 25);
            this.CopyFilteredToClipboard.TabIndex = 7;
            this.CopyFilteredToClipboard.Text = "Copy To Clipboard";
            this.CopyFilteredToClipboard.UseVisualStyleBackColor = true;
            this.CopyFilteredToClipboard.Click += new System.EventHandler(this.CopyFilteredToClipboard_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "chatlog.txt";
            // 
            // RemoveTimestamps
            // 
            this.RemoveTimestamps.AutoSize = true;
            this.RemoveTimestamps.Location = new System.Drawing.Point(309, 30);
            this.RemoveTimestamps.Name = "RemoveTimestamps";
            this.RemoveTimestamps.Size = new System.Drawing.Size(121, 17);
            this.RemoveTimestamps.TabIndex = 2;
            this.RemoveTimestamps.Text = "Remove timestamps";
            this.RemoveTimestamps.UseVisualStyleBackColor = true;
            this.RemoveTimestamps.CheckedChanged += new System.EventHandler(this.RemoveTimestamps_CheckedChanged);
            // 
            // TimeLabel
            // 
            this.TimeLabel.Location = new System.Drawing.Point(309, 9);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(121, 13);
            this.TimeLabel.TabIndex = 0;
            this.TimeLabel.Text = "Current time: 23:23:23";
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // NamesLabel
            // 
            this.NamesLabel.Location = new System.Drawing.Point(10, 53);
            this.NamesLabel.Name = "NamesLabel";
            this.NamesLabel.Size = new System.Drawing.Size(135, 13);
            this.NamesLabel.TabIndex = 0;
            this.NamesLabel.Text = "Names to filter in:";
            // 
            // ChatLogFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 291);
            this.Controls.Add(this.NamesLabel);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.RemoveTimestamps);
            this.Controls.Add(this.CopyFilteredToClipboard);
            this.Controls.Add(this.SaveFiltered);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.Filtered);
            this.Controls.Add(this.Names);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.BrowseForParsed);
            this.Controls.Add(this.LoadUnparsed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChatLogFilter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Chat Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatLogFilter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadUnparsed;
        private System.Windows.Forms.Button BrowseForParsed;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.RichTextBox Names;
        private System.Windows.Forms.RichTextBox Filtered;
        private System.Windows.Forms.Button Filter;
        private System.Windows.Forms.Button SaveFiltered;
        private System.Windows.Forms.Button CopyFilteredToClipboard;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.CheckBox RemoveTimestamps;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label NamesLabel;
    }
}