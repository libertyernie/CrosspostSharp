namespace CrosspostSharp3 {
	partial class ArtworkForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArtworkForm));
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			wbrDescription = new System.Windows.Forms.WebBrowser();
			chkAdult = new System.Windows.Forms.CheckBox();
			chkMature = new System.Windows.Forms.CheckBox();
			txtTags = new System.Windows.Forms.TextBox();
			lblTags = new System.Windows.Forms.Label();
			lblDescription = new System.Windows.Forms.Label();
			txtTitle = new System.Windows.Forms.TextBox();
			lblTitle = new System.Windows.Forms.Label();
			btnPost = new System.Windows.Forms.Button();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exportAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			mainWindowAccountSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			panel1 = new System.Windows.Forms.Panel();
			listBox1 = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			menuStrip1.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(0, 24);
			splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(pictureBox1);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(wbrDescription);
			splitContainer1.Panel2.Controls.Add(chkAdult);
			splitContainer1.Panel2.Controls.Add(chkMature);
			splitContainer1.Panel2.Controls.Add(txtTags);
			splitContainer1.Panel2.Controls.Add(lblTags);
			splitContainer1.Panel2.Controls.Add(lblDescription);
			splitContainer1.Panel2.Controls.Add(txtTitle);
			splitContainer1.Panel2.Controls.Add(lblTitle);
			splitContainer1.Size = new System.Drawing.Size(518, 450);
			splitContainer1.SplitterDistance = 203;
			splitContainer1.SplitterWidth = 5;
			splitContainer1.TabIndex = 0;
			// 
			// pictureBox1
			// 
			pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			pictureBox1.Location = new System.Drawing.Point(0, 0);
			pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(518, 203);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// wbrDescription
			// 
			wbrDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			wbrDescription.Location = new System.Drawing.Point(143, 32);
			wbrDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			wbrDescription.MinimumSize = new System.Drawing.Size(23, 23);
			wbrDescription.Name = "wbrDescription";
			wbrDescription.Size = new System.Drawing.Size(367, 144);
			wbrDescription.TabIndex = 12;
			// 
			// chkAdult
			// 
			chkAdult.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkAdult.AutoSize = true;
			chkAdult.Location = new System.Drawing.Point(219, 211);
			chkAdult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkAdult.Name = "chkAdult";
			chkAdult.Size = new System.Drawing.Size(55, 19);
			chkAdult.TabIndex = 9;
			chkAdult.Text = "Adult";
			chkAdult.UseVisualStyleBackColor = true;
			// 
			// chkMature
			// 
			chkMature.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkMature.AutoSize = true;
			chkMature.Location = new System.Drawing.Point(143, 211);
			chkMature.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkMature.Name = "chkMature";
			chkMature.Size = new System.Drawing.Size(64, 19);
			chkMature.TabIndex = 8;
			chkMature.Text = "Mature";
			chkMature.UseVisualStyleBackColor = true;
			// 
			// txtTags
			// 
			txtTags.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTags.Location = new System.Drawing.Point(143, 182);
			txtTags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtTags.Name = "txtTags";
			txtTags.Size = new System.Drawing.Size(367, 23);
			txtTags.TabIndex = 7;
			// 
			// lblTags
			// 
			lblTags.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblTags.AutoSize = true;
			lblTags.Location = new System.Drawing.Point(105, 185);
			lblTags.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblTags.Name = "lblTags";
			lblTags.Size = new System.Drawing.Size(30, 15);
			lblTags.TabIndex = 6;
			lblTags.Text = "Tags";
			// 
			// lblDescription
			// 
			lblDescription.AutoSize = true;
			lblDescription.Location = new System.Drawing.Point(68, 32);
			lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new System.Drawing.Size(67, 15);
			lblDescription.TabIndex = 2;
			lblDescription.Text = "Description";
			// 
			// txtTitle
			// 
			txtTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTitle.Location = new System.Drawing.Point(143, 3);
			txtTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtTitle.Name = "txtTitle";
			txtTitle.Size = new System.Drawing.Size(367, 23);
			txtTitle.TabIndex = 1;
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Location = new System.Drawing.Point(106, 6);
			lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(29, 15);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Title";
			// 
			// btnPost
			// 
			btnPost.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			btnPost.Location = new System.Drawing.Point(41, 409);
			btnPost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnPost.Name = "btnPost";
			btnPost.Size = new System.Drawing.Size(82, 27);
			btnPost.TabIndex = 9;
			btnPost.Text = "Post";
			btnPost.UseVisualStyleBackColor = true;
			btnPost.Click += btnPost_Click;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			menuStrip1.Size = new System.Drawing.Size(681, 24);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, exportAsToolStripMenuItem, closeToolStripMenuItem, exitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			openToolStripMenuItem.Text = "&Open...";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// exportAsToolStripMenuItem
			// 
			exportAsToolStripMenuItem.Name = "exportAsToolStripMenuItem";
			exportAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			exportAsToolStripMenuItem.Text = "Export As...";
			exportAsToolStripMenuItem.Click += exportAsToolStripMenuItem_Click;
			// 
			// closeToolStripMenuItem
			// 
			closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			closeToolStripMenuItem.Text = "&Close";
			closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			exitToolStripMenuItem.Text = "E&xit";
			exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
			// 
			// toolsToolStripMenuItem
			// 
			toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mainWindowAccountSetupToolStripMenuItem });
			toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			toolsToolStripMenuItem.Text = "&Tools";
			// 
			// mainWindowAccountSetupToolStripMenuItem
			// 
			mainWindowAccountSetupToolStripMenuItem.Name = "mainWindowAccountSetupToolStripMenuItem";
			mainWindowAccountSetupToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			mainWindowAccountSetupToolStripMenuItem.Text = "&Main Window / Account Setup";
			mainWindowAccountSetupToolStripMenuItem.Click += mainWindowAccountSetupToolStripMenuItem_Click;
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
			// panel1
			// 
			panel1.Controls.Add(btnPost);
			panel1.Controls.Add(listBox1);
			panel1.Dock = System.Windows.Forms.DockStyle.Right;
			panel1.Location = new System.Drawing.Point(518, 24);
			panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(163, 450);
			panel1.TabIndex = 10;
			// 
			// listBox1
			// 
			listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			listBox1.FormattingEnabled = true;
			listBox1.IntegralHeight = false;
			listBox1.ItemHeight = 15;
			listBox1.Location = new System.Drawing.Point(0, 0);
			listBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			listBox1.Name = "listBox1";
			listBox1.Size = new System.Drawing.Size(163, 395);
			listBox1.TabIndex = 1;
			listBox1.DoubleClick += listBox1_DoubleClick;
			// 
			// ArtworkForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(681, 474);
			Controls.Add(splitContainer1);
			Controls.Add(panel1);
			Controls.Add(menuStrip1);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "ArtworkForm";
			Text = "Edit Submission Details";
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			panel1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.CheckBox chkAdult;
		private System.Windows.Forms.CheckBox chkMature;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mainWindowAccountSetupToolStripMenuItem;
		private System.Windows.Forms.WebBrowser wbrDescription;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}