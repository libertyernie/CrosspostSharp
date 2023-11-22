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
			lblSource = new System.Windows.Forms.Label();
			ddlSource = new System.Windows.Forms.ComboBox();
			btnLoad = new System.Windows.Forms.Button();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			accountSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			deviantArtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			furAffinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			furryNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			inkbunnyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			mastodonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			pixelfedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			tumblrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			weasylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			postToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			btnPrevious = new System.Windows.Forms.Button();
			btnNext = new System.Windows.Forms.Button();
			picUserIcon = new System.Windows.Forms.PictureBox();
			lblUsername = new System.Windows.Forms.Label();
			lblSiteName = new System.Windows.Forms.Label();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)picUserIcon).BeginInit();
			SuspendLayout();
			// 
			// lblSource
			// 
			lblSource.AutoSize = true;
			lblSource.Location = new System.Drawing.Point(13, 31);
			lblSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblSource.Name = "lblSource";
			lblSource.Size = new System.Drawing.Size(43, 15);
			lblSource.TabIndex = 1;
			lblSource.Text = "Source";
			// 
			// ddlSource
			// 
			ddlSource.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			ddlSource.DisplayMember = "WrapperName";
			ddlSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ddlSource.FormattingEnabled = true;
			ddlSource.Location = new System.Drawing.Point(64, 27);
			ddlSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			ddlSource.Name = "ddlSource";
			ddlSource.Size = new System.Drawing.Size(217, 23);
			ddlSource.TabIndex = 2;
			ddlSource.DropDown += ddlSource_DropDown;
			// 
			// btnLoad
			// 
			btnLoad.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnLoad.Enabled = false;
			btnLoad.Location = new System.Drawing.Point(289, 27);
			btnLoad.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnLoad.Name = "btnLoad";
			btnLoad.Size = new System.Drawing.Size(88, 23);
			btnLoad.TabIndex = 3;
			btnLoad.Text = "Load";
			btnLoad.UseVisualStyleBackColor = true;
			btnLoad.Click += btnLoad_Click;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			tableLayoutPanel1.Location = new System.Drawing.Point(13, 112);
			tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			tableLayoutPanel1.Size = new System.Drawing.Size(364, 260);
			tableLayoutPanel1.TabIndex = 4;
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, statusToolStripMenuItem, helpToolStripMenuItem });
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			menuStrip1.Size = new System.Drawing.Size(390, 24);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, exportToolStripMenuItem, exitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			openToolStripMenuItem.Text = "&Open...";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// exportToolStripMenuItem
			// 
			exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			exportToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			exportToolStripMenuItem.Text = "&Export...";
			exportToolStripMenuItem.Click += exportToolStripMenuItem_Click_1;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			exitToolStripMenuItem.Text = "E&xit";
			exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
			// 
			// toolsToolStripMenuItem
			// 
			toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { accountSetupToolStripMenuItem });
			toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			toolsToolStripMenuItem.Text = "&Tools";
			// 
			// accountSetupToolStripMenuItem
			// 
			accountSetupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { deviantArtToolStripMenuItem, furAffinityToolStripMenuItem, furryNetworkToolStripMenuItem, inkbunnyToolStripMenuItem, mastodonToolStripMenuItem, pixelfedToolStripMenuItem, tumblrToolStripMenuItem, weasylToolStripMenuItem });
			accountSetupToolStripMenuItem.Name = "accountSetupToolStripMenuItem";
			accountSetupToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			accountSetupToolStripMenuItem.Text = "&Account Setup";
			// 
			// deviantArtToolStripMenuItem
			// 
			deviantArtToolStripMenuItem.Name = "deviantArtToolStripMenuItem";
			deviantArtToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			deviantArtToolStripMenuItem.Text = "&DeviantArt";
			deviantArtToolStripMenuItem.Click += deviantArtToolStripMenuItem_Click;
			// 
			// furAffinityToolStripMenuItem
			// 
			furAffinityToolStripMenuItem.Name = "furAffinityToolStripMenuItem";
			furAffinityToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			furAffinityToolStripMenuItem.Text = "Fur&Affinity";
			furAffinityToolStripMenuItem.Click += furAffinityToolStripMenuItem_Click;
			// 
			// furryNetworkToolStripMenuItem
			// 
			furryNetworkToolStripMenuItem.Name = "furryNetworkToolStripMenuItem";
			furryNetworkToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			furryNetworkToolStripMenuItem.Text = "Furry &Network";
			furryNetworkToolStripMenuItem.Click += furryNetworkToolStripMenuItem_Click;
			// 
			// inkbunnyToolStripMenuItem
			// 
			inkbunnyToolStripMenuItem.Name = "inkbunnyToolStripMenuItem";
			inkbunnyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			inkbunnyToolStripMenuItem.Text = "&Inkbunny";
			inkbunnyToolStripMenuItem.Click += inkbunnyToolStripMenuItem_Click;
			// 
			// mastodonToolStripMenuItem
			// 
			mastodonToolStripMenuItem.Name = "mastodonToolStripMenuItem";
			mastodonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			mastodonToolStripMenuItem.Text = "&Mastodon";
			mastodonToolStripMenuItem.Click += mastodonToolStripMenuItem_Click;
			// 
			// pixelfedToolStripMenuItem
			// 
			pixelfedToolStripMenuItem.Name = "pixelfedToolStripMenuItem";
			pixelfedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			pixelfedToolStripMenuItem.Text = "&Pixelfed";
			pixelfedToolStripMenuItem.Click += pixelfedToolStripMenuItem_Click;
			// 
			// tumblrToolStripMenuItem
			// 
			tumblrToolStripMenuItem.Name = "tumblrToolStripMenuItem";
			tumblrToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			tumblrToolStripMenuItem.Text = "T&umblr";
			tumblrToolStripMenuItem.Click += tumblrToolStripMenuItem_Click;
			// 
			// weasylToolStripMenuItem
			// 
			weasylToolStripMenuItem.Name = "weasylToolStripMenuItem";
			weasylToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			weasylToolStripMenuItem.Text = "&Weasyl";
			weasylToolStripMenuItem.Click += weasylToolStripMenuItem_Click;
			// 
			// statusToolStripMenuItem
			// 
			statusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { postToolStripMenuItem });
			statusToolStripMenuItem.Name = "statusToolStripMenuItem";
			statusToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
			statusToolStripMenuItem.Text = "&Status";
			// 
			// postToolStripMenuItem
			// 
			postToolStripMenuItem.Name = "postToolStripMenuItem";
			postToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
			postToolStripMenuItem.Text = "&Post...";
			postToolStripMenuItem.Click += postToolStripMenuItem_Click;
			// 
			// helpToolStripMenuItem
			// 
			helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
			helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			aboutToolStripMenuItem.Text = "&About";
			aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
			// 
			// btnPrevious
			// 
			btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnPrevious.Location = new System.Drawing.Point(13, 378);
			btnPrevious.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnPrevious.Name = "btnPrevious";
			btnPrevious.Size = new System.Drawing.Size(88, 27);
			btnPrevious.TabIndex = 5;
			btnPrevious.Text = "← Previous";
			btnPrevious.UseVisualStyleBackColor = true;
			btnPrevious.Click += btnPrevious_Click;
			// 
			// btnNext
			// 
			btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnNext.Location = new System.Drawing.Point(289, 378);
			btnNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnNext.Name = "btnNext";
			btnNext.Size = new System.Drawing.Size(88, 27);
			btnNext.TabIndex = 6;
			btnNext.Text = "Next →";
			btnNext.UseVisualStyleBackColor = true;
			btnNext.Click += btnNext_Click;
			// 
			// picUserIcon
			// 
			picUserIcon.Location = new System.Drawing.Point(13, 56);
			picUserIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			picUserIcon.Name = "picUserIcon";
			picUserIcon.Size = new System.Drawing.Size(50, 50);
			picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			picUserIcon.TabIndex = 14;
			picUserIcon.TabStop = false;
			// 
			// lblUsername
			// 
			lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			lblUsername.Location = new System.Drawing.Point(71, 56);
			lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblUsername.Name = "lblUsername";
			lblUsername.Size = new System.Drawing.Size(304, 20);
			lblUsername.TabIndex = 11;
			lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSiteName
			// 
			lblSiteName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblSiteName.AutoEllipsis = true;
			lblSiteName.Location = new System.Drawing.Point(71, 76);
			lblSiteName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblSiteName.Name = "lblSiteName";
			lblSiteName.Size = new System.Drawing.Size(295, 15);
			lblSiteName.TabIndex = 12;
			lblSiteName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(390, 417);
			Controls.Add(picUserIcon);
			Controls.Add(lblUsername);
			Controls.Add(lblSiteName);
			Controls.Add(btnNext);
			Controls.Add(btnPrevious);
			Controls.Add(tableLayoutPanel1);
			Controls.Add(btnLoad);
			Controls.Add(ddlSource);
			Controls.Add(lblSource);
			Controls.Add(menuStrip1);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "MainForm";
			Text = "CrosspostSharp 5";
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)picUserIcon).EndInit();
			ResumeLayout(false);
			PerformLayout();
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
		private System.Windows.Forms.ToolStripMenuItem tumblrToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inkbunnyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem weasylToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem furryNetworkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mastodonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem postToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pixelfedToolStripMenuItem;
	}
}

