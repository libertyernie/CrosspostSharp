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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblSource = new System.Windows.Forms.Label();
            this.ddlSource = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviantArtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.furAffinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.furryNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inkbunnyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mastodonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tumblrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weasylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviantArtModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eclipseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aPIOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiPageSize4 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiPageSize9 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.postToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblLoadStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(13, 31);
            this.lblSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(43, 15);
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
            this.ddlSource.Location = new System.Drawing.Point(64, 27);
            this.ddlSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ddlSource.Name = "ddlSource";
            this.ddlSource.Size = new System.Drawing.Size(217, 23);
            this.ddlSource.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(289, 27);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(88, 23);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 112);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(364, 260);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(390, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exportToolStripMenuItem.Text = "&Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountSetupToolStripMenuItem,
            this.deviantArtModeToolStripMenuItem,
            this.pageSizeToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // accountSetupToolStripMenuItem
            // 
            this.accountSetupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviantArtToolStripMenuItem,
            this.furAffinityToolStripMenuItem,
            this.furryNetworkToolStripMenuItem,
            this.inkbunnyToolStripMenuItem,
            this.mastodonToolStripMenuItem,
            this.twitterToolStripMenuItem,
            this.tumblrToolStripMenuItem,
            this.weasylToolStripMenuItem});
            this.accountSetupToolStripMenuItem.Name = "accountSetupToolStripMenuItem";
            this.accountSetupToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.accountSetupToolStripMenuItem.Text = "&Account Setup";
            // 
            // deviantArtToolStripMenuItem
            // 
            this.deviantArtToolStripMenuItem.Name = "deviantArtToolStripMenuItem";
            this.deviantArtToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.deviantArtToolStripMenuItem.Text = "&DeviantArt";
            this.deviantArtToolStripMenuItem.Click += new System.EventHandler(this.deviantArtToolStripMenuItem_Click);
            // 
            // furAffinityToolStripMenuItem
            // 
            this.furAffinityToolStripMenuItem.Name = "furAffinityToolStripMenuItem";
            this.furAffinityToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.furAffinityToolStripMenuItem.Text = "Fur&Affinity";
            this.furAffinityToolStripMenuItem.Click += new System.EventHandler(this.furAffinityToolStripMenuItem_Click);
            // 
            // furryNetworkToolStripMenuItem
            // 
            this.furryNetworkToolStripMenuItem.Name = "furryNetworkToolStripMenuItem";
            this.furryNetworkToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.furryNetworkToolStripMenuItem.Text = "Furry &Network";
            this.furryNetworkToolStripMenuItem.Click += new System.EventHandler(this.furryNetworkToolStripMenuItem_Click);
            // 
            // inkbunnyToolStripMenuItem
            // 
            this.inkbunnyToolStripMenuItem.Name = "inkbunnyToolStripMenuItem";
            this.inkbunnyToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.inkbunnyToolStripMenuItem.Text = "&Inkbunny";
            this.inkbunnyToolStripMenuItem.Click += new System.EventHandler(this.inkbunnyToolStripMenuItem_Click);
            // 
            // mastodonToolStripMenuItem
            // 
            this.mastodonToolStripMenuItem.Name = "mastodonToolStripMenuItem";
            this.mastodonToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.mastodonToolStripMenuItem.Text = "&Mastodon";
            this.mastodonToolStripMenuItem.Click += new System.EventHandler(this.mastodonToolStripMenuItem_Click);
            // 
            // twitterToolStripMenuItem
            // 
            this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
            this.twitterToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.twitterToolStripMenuItem.Text = "&Twitter";
            this.twitterToolStripMenuItem.Click += new System.EventHandler(this.twitterToolStripMenuItem_Click);
            // 
            // tumblrToolStripMenuItem
            // 
            this.tumblrToolStripMenuItem.Name = "tumblrToolStripMenuItem";
            this.tumblrToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.tumblrToolStripMenuItem.Text = "T&umblr";
            this.tumblrToolStripMenuItem.Click += new System.EventHandler(this.tumblrToolStripMenuItem_Click);
            // 
            // weasylToolStripMenuItem
            // 
            this.weasylToolStripMenuItem.Name = "weasylToolStripMenuItem";
            this.weasylToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.weasylToolStripMenuItem.Text = "&Weasyl";
            this.weasylToolStripMenuItem.Click += new System.EventHandler(this.weasylToolStripMenuItem_Click);
            // 
            // deviantArtModeToolStripMenuItem
            // 
            this.deviantArtModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eclipseToolStripMenuItem,
            this.aPIOnlyToolStripMenuItem});
            this.deviantArtModeToolStripMenuItem.Name = "deviantArtModeToolStripMenuItem";
            this.deviantArtModeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deviantArtModeToolStripMenuItem.Text = "&DeviantArt Mode";
            // 
            // eclipseToolStripMenuItem
            // 
            this.eclipseToolStripMenuItem.CheckOnClick = true;
            this.eclipseToolStripMenuItem.Name = "eclipseToolStripMenuItem";
            this.eclipseToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.eclipseToolStripMenuItem.Text = "&Eclipse + API";
            this.eclipseToolStripMenuItem.Click += new System.EventHandler(this.EclipseToolStripMenuItem_Click);
            // 
            // aPIOnlyToolStripMenuItem
            // 
            this.aPIOnlyToolStripMenuItem.CheckOnClick = true;
            this.aPIOnlyToolStripMenuItem.Name = "aPIOnlyToolStripMenuItem";
            this.aPIOnlyToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.aPIOnlyToolStripMenuItem.Text = "&API Only";
            this.aPIOnlyToolStripMenuItem.Click += new System.EventHandler(this.APIOnlyToolStripMenuItem_Click);
            // 
            // pageSizeToolStripMenuItem
            // 
            this.pageSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiPageSize4,
            this.tsiPageSize9});
            this.pageSizeToolStripMenuItem.Name = "pageSizeToolStripMenuItem";
            this.pageSizeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pageSizeToolStripMenuItem.Text = "&Page Size";
            // 
            // tsiPageSize4
            // 
            this.tsiPageSize4.Name = "tsiPageSize4";
            this.tsiPageSize4.Size = new System.Drawing.Size(80, 22);
            this.tsiPageSize4.Text = "4";
            this.tsiPageSize4.Click += new System.EventHandler(this.tsiPageSize4_Click);
            // 
            // tsiPageSize9
            // 
            this.tsiPageSize9.Name = "tsiPageSize9";
            this.tsiPageSize9.Size = new System.Drawing.Size(80, 22);
            this.tsiPageSize9.Text = "9";
            this.tsiPageSize9.Click += new System.EventHandler(this.tsiPageSize9_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.postToolStripMenuItem});
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.statusToolStripMenuItem.Text = "&Status";
            // 
            // postToolStripMenuItem
            // 
            this.postToolStripMenuItem.Name = "postToolStripMenuItem";
            this.postToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.postToolStripMenuItem.Text = "&Post...";
            this.postToolStripMenuItem.Click += new System.EventHandler(this.postToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Location = new System.Drawing.Point(13, 378);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(88, 27);
            this.btnPrevious.TabIndex = 5;
            this.btnPrevious.Text = "← Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(289, 378);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(88, 27);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Next →";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(13, 56);
            this.picUserIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(50, 50);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 14;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername
            // 
            this.lblUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsername.Location = new System.Drawing.Point(71, 56);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(304, 20);
            this.lblUsername.TabIndex = 11;
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.AutoEllipsis = true;
            this.lblSiteName.Location = new System.Drawing.Point(71, 76);
            this.lblSiteName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(295, 15);
            this.lblSiteName.TabIndex = 12;
            this.lblSiteName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoadStatus
            // 
            this.lblLoadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadStatus.AutoEllipsis = true;
            this.lblLoadStatus.Location = new System.Drawing.Point(13, 53);
            this.lblLoadStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoadStatus.Name = "lblLoadStatus";
            this.lblLoadStatus.Size = new System.Drawing.Size(364, 53);
            this.lblLoadStatus.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 417);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "CrosspostSharp 4";
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
		private System.Windows.Forms.Label lblLoadStatus;
		private System.Windows.Forms.ToolStripMenuItem tumblrToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inkbunnyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem weasylToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem furryNetworkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pageSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tsiPageSize4;
		private System.Windows.Forms.ToolStripMenuItem tsiPageSize9;
		private System.Windows.Forms.ToolStripMenuItem mastodonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem postToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deviantArtModeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem eclipseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aPIOnlyToolStripMenuItem;
	}
}

