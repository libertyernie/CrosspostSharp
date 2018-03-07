namespace CrosspostSharp3 {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.lblSource = new System.Windows.Forms.Label();
            this.ddlSource = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviantArtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.furAffinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inkbunnyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaRSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tumblrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weasylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblLoadStatus = new System.Windows.Forms.Label();
            this.furryNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(16, 41);
            this.lblSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(53, 17);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Source";
            // 
            // ddlSource
            // 
            this.ddlSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSource.DisplayMember = "WrapperName";
            this.ddlSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSource.FormattingEnabled = true;
            this.ddlSource.Location = new System.Drawing.Point(79, 36);
            this.ddlSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ddlSource.Name = "ddlSource";
            this.ddlSource.Size = new System.Drawing.Size(241, 24);
            this.ddlSource.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(329, 34);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 28);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 135);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(409, 258);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(445, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(129, 26);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(129, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountSetupToolStripMenuItem,
            this.refreshAllToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // accountSetupToolStripMenuItem
            // 
            this.accountSetupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviantArtToolStripMenuItem,
            this.furAffinityToolStripMenuItem,
            this.furryNetworkToolStripMenuItem,
            this.inkbunnyToolStripMenuItem,
            this.mediaRSSToolStripMenuItem,
            this.twitterToolStripMenuItem,
            this.tumblrToolStripMenuItem,
            this.weasylToolStripMenuItem});
            this.accountSetupToolStripMenuItem.Name = "accountSetupToolStripMenuItem";
            this.accountSetupToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.accountSetupToolStripMenuItem.Text = "&Account Setup";
            // 
            // deviantArtToolStripMenuItem
            // 
            this.deviantArtToolStripMenuItem.Name = "deviantArtToolStripMenuItem";
            this.deviantArtToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.deviantArtToolStripMenuItem.Text = "&DeviantArt";
            this.deviantArtToolStripMenuItem.Click += new System.EventHandler(this.deviantArtToolStripMenuItem_Click);
            // 
            // furAffinityToolStripMenuItem
            // 
            this.furAffinityToolStripMenuItem.Name = "furAffinityToolStripMenuItem";
            this.furAffinityToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.furAffinityToolStripMenuItem.Text = "&FurAffinity";
            this.furAffinityToolStripMenuItem.Click += new System.EventHandler(this.furAffinityToolStripMenuItem_Click);
            // 
            // inkbunnyToolStripMenuItem
            // 
            this.inkbunnyToolStripMenuItem.Name = "inkbunnyToolStripMenuItem";
            this.inkbunnyToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.inkbunnyToolStripMenuItem.Text = "&Inkbunny";
            this.inkbunnyToolStripMenuItem.Click += new System.EventHandler(this.inkbunnyToolStripMenuItem_Click);
            // 
            // mediaRSSToolStripMenuItem
            // 
            this.mediaRSSToolStripMenuItem.Name = "mediaRSSToolStripMenuItem";
            this.mediaRSSToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.mediaRSSToolStripMenuItem.Text = "&Media RSS";
            this.mediaRSSToolStripMenuItem.Click += new System.EventHandler(this.mediaRSSToolStripMenuItem_Click);
            // 
            // twitterToolStripMenuItem
            // 
            this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
            this.twitterToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.twitterToolStripMenuItem.Text = "&Twitter";
            this.twitterToolStripMenuItem.Click += new System.EventHandler(this.twitterToolStripMenuItem_Click);
            // 
            // tumblrToolStripMenuItem
            // 
            this.tumblrToolStripMenuItem.Name = "tumblrToolStripMenuItem";
            this.tumblrToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.tumblrToolStripMenuItem.Text = "T&umblr";
            this.tumblrToolStripMenuItem.Click += new System.EventHandler(this.tumblrToolStripMenuItem_Click);
            // 
            // weasylToolStripMenuItem
            // 
            this.weasylToolStripMenuItem.Name = "weasylToolStripMenuItem";
            this.weasylToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.weasylToolStripMenuItem.Text = "&Weasyl";
            this.weasylToolStripMenuItem.Click += new System.EventHandler(this.weasylToolStripMenuItem_Click);
            // 
            // refreshAllToolStripMenuItem
            // 
            this.refreshAllToolStripMenuItem.Name = "refreshAllToolStripMenuItem";
            this.refreshAllToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.refreshAllToolStripMenuItem.Text = "&Refresh All";
            this.refreshAllToolStripMenuItem.Click += new System.EventHandler(this.refreshAllToolStripMenuItem_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Location = new System.Drawing.Point(20, 401);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(100, 28);
            this.btnPrevious.TabIndex = 5;
            this.btnPrevious.Text = "← Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(329, 401);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 28);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Next →";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(20, 69);
            this.picUserIcon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(64, 59);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 14;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername
            // 
            this.lblUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(92, 69);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(337, 21);
            this.lblUsername.TabIndex = 11;
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.AutoEllipsis = true;
            this.lblSiteName.Location = new System.Drawing.Point(92, 90);
            this.lblSiteName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(337, 16);
            this.lblSiteName.TabIndex = 12;
            this.lblSiteName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoadStatus
            // 
            this.lblLoadStatus.AutoEllipsis = true;
            this.lblLoadStatus.Location = new System.Drawing.Point(16, 65);
            this.lblLoadStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoadStatus.Name = "lblLoadStatus";
            this.lblLoadStatus.Size = new System.Drawing.Size(413, 16);
            this.lblLoadStatus.TabIndex = 0;
            // 
            // furryNetworkToolStripMenuItem
            // 
            this.furryNetworkToolStripMenuItem.Name = "furryNetworkToolStripMenuItem";
            this.furryNetworkToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.furryNetworkToolStripMenuItem.Text = "Furry&Network";
            this.furryNetworkToolStripMenuItem.Click += new System.EventHandler(this.furryNetworkToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 444);
            this.Controls.Add(this.lblLoadStatus);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.ddlSource);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "CrosspostSharp 3";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.ComboBox ddlSource;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem accountSetupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem furAffinityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deviantArtToolStripMenuItem;
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.Label lblSiteName;
		private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mediaRSSToolStripMenuItem;
		private System.Windows.Forms.Label lblLoadStatus;
		private System.Windows.Forms.ToolStripMenuItem tumblrToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inkbunnyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem weasylToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem furryNetworkToolStripMenuItem;
	}
}

