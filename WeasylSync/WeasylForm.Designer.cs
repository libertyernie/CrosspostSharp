namespace WeasylSync {
	partial class WeasylForm {
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
			this.btnUp = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.mainPictureBox = new System.Windows.Forms.PictureBox();
			this.chkHeader = new System.Windows.Forms.CheckBox();
			this.chkDescription = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtTags1 = new System.Windows.Forms.TextBox();
			this.chkTags1 = new System.Windows.Forms.CheckBox();
			this.chkFooter = new System.Windows.Forms.CheckBox();
			this.txtFooter = new System.Windows.Forms.TextBox();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.pickDate = new System.Windows.Forms.DateTimePicker();
			this.pickTime = new System.Windows.Forms.DateTimePicker();
			this.chkNow = new System.Windows.Forms.CheckBox();
			this.chkWeasylSubmitIdTag = new System.Windows.Forms.CheckBox();
			this.txtTags2 = new System.Windows.Forms.TextBox();
			this.chkTags2 = new System.Windows.Forms.CheckBox();
			this.btnPost = new System.Windows.Forms.Button();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblLinkTo = new System.Windows.Forms.Label();
			this.lblWeasylStatus1 = new System.Windows.Forms.Label();
			this.lblTumblrStatus1 = new System.Windows.Forms.Label();
			this.lblWeasylStatus2 = new System.Windows.Forms.Label();
			this.lblTumblrStatus2 = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chkHTMLPreview = new System.Windows.Forms.CheckBox();
			this.previewPanel = new System.Windows.Forms.Panel();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lProgressBar1 = new WeasylSync.LProgressBar();
			this.thumbnail4 = new WeasylSync.WeasylThumbnail();
			this.thumbnail3 = new WeasylSync.WeasylThumbnail();
			this.thumbnail2 = new WeasylSync.WeasylThumbnail();
			this.thumbnail1 = new WeasylSync.WeasylThumbnail();
			this.btnViewExistingTumblrPost = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnUp
			// 
			this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUp.Location = new System.Drawing.Point(104, 27);
			this.btnUp.Margin = new System.Windows.Forms.Padding(0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(32, 32);
			this.btnUp.TabIndex = 0;
			this.btnUp.Text = "↑";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDown.Location = new System.Drawing.Point(104, 509);
			this.btnDown.Margin = new System.Windows.Forms.Padding(0);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(32, 32);
			this.btnDown.TabIndex = 1;
			this.btnDown.Text = "↓";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(142, 27);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(400, 225);
			this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.mainPictureBox.TabIndex = 6;
			this.mainPictureBox.TabStop = false;
			// 
			// chkHeader
			// 
			this.chkHeader.Checked = true;
			this.chkHeader.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkHeader.Location = new System.Drawing.Point(142, 284);
			this.chkHeader.Name = "chkHeader";
			this.chkHeader.Size = new System.Drawing.Size(18, 19);
			this.chkHeader.TabIndex = 6;
			this.chkHeader.UseVisualStyleBackColor = true;
			this.chkHeader.CheckedChanged += new System.EventHandler(this.chkTitle_CheckedChanged);
			// 
			// chkDescription
			// 
			this.chkDescription.Checked = true;
			this.chkDescription.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDescription.Location = new System.Drawing.Point(142, 336);
			this.chkDescription.Name = "chkDescription";
			this.chkDescription.Size = new System.Drawing.Size(18, 20);
			this.chkDescription.TabIndex = 10;
			this.chkDescription.UseVisualStyleBackColor = true;
			this.chkDescription.CheckedChanged += new System.EventHandler(this.chkDescription_CheckedChanged);
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(166, 336);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(376, 73);
			this.txtDescription.TabIndex = 11;
			// 
			// txtTags1
			// 
			this.txtTags1.Location = new System.Drawing.Point(33, 19);
			this.txtTags1.Name = "txtTags1";
			this.txtTags1.Size = new System.Drawing.Size(361, 20);
			this.txtTags1.TabIndex = 1;
			// 
			// chkTags1
			// 
			this.chkTags1.Checked = true;
			this.chkTags1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags1.Location = new System.Drawing.Point(9, 19);
			this.chkTags1.Name = "chkTags1";
			this.chkTags1.Size = new System.Drawing.Size(18, 20);
			this.chkTags1.TabIndex = 0;
			this.chkTags1.UseVisualStyleBackColor = true;
			this.chkTags1.CheckedChanged += new System.EventHandler(this.chkTags1_CheckedChanged);
			// 
			// chkFooter
			// 
			this.chkFooter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkFooter.Checked = true;
			this.chkFooter.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFooter.Location = new System.Drawing.Point(142, 415);
			this.chkFooter.Name = "chkFooter";
			this.chkFooter.Size = new System.Drawing.Size(18, 20);
			this.chkFooter.TabIndex = 12;
			this.chkFooter.UseVisualStyleBackColor = true;
			this.chkFooter.CheckedChanged += new System.EventHandler(this.chkFooter_CheckedChanged);
			// 
			// txtFooter
			// 
			this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFooter.Location = new System.Drawing.Point(166, 415);
			this.txtFooter.Name = "txtFooter";
			this.txtFooter.Size = new System.Drawing.Size(376, 20);
			this.txtFooter.TabIndex = 13;
			// 
			// txtHeader
			// 
			this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeader.Location = new System.Drawing.Point(166, 284);
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtHeader.Size = new System.Drawing.Size(376, 20);
			this.txtHeader.TabIndex = 7;
			// 
			// pickDate
			// 
			this.pickDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pickDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.pickDate.Location = new System.Drawing.Point(266, 258);
			this.pickDate.Name = "pickDate";
			this.pickDate.Size = new System.Drawing.Size(100, 20);
			this.pickDate.TabIndex = 3;
			this.pickDate.Visible = false;
			// 
			// pickTime
			// 
			this.pickTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pickTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.pickTime.Location = new System.Drawing.Point(372, 258);
			this.pickTime.Name = "pickTime";
			this.pickTime.ShowUpDown = true;
			this.pickTime.Size = new System.Drawing.Size(100, 20);
			this.pickTime.TabIndex = 4;
			this.pickTime.Visible = false;
			// 
			// chkNow
			// 
			this.chkNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkNow.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkNow.Checked = true;
			this.chkNow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNow.Location = new System.Drawing.Point(478, 258);
			this.chkNow.Name = "chkNow";
			this.chkNow.Size = new System.Drawing.Size(64, 20);
			this.chkNow.TabIndex = 5;
			this.chkNow.Text = "Now";
			this.chkNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkNow.UseVisualStyleBackColor = true;
			this.chkNow.CheckedChanged += new System.EventHandler(this.chkNow_CheckedChanged);
			// 
			// chkWeasylSubmitIdTag
			// 
			this.chkWeasylSubmitIdTag.AutoEllipsis = true;
			this.chkWeasylSubmitIdTag.Checked = true;
			this.chkWeasylSubmitIdTag.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWeasylSubmitIdTag.Location = new System.Drawing.Point(292, 42);
			this.chkWeasylSubmitIdTag.Name = "chkWeasylSubmitIdTag";
			this.chkWeasylSubmitIdTag.Size = new System.Drawing.Size(102, 20);
			this.chkWeasylSubmitIdTag.TabIndex = 4;
			this.chkWeasylSubmitIdTag.Text = "#weasyl000000";
			this.chkWeasylSubmitIdTag.UseVisualStyleBackColor = true;
			// 
			// txtTags2
			// 
			this.txtTags2.Location = new System.Drawing.Point(33, 42);
			this.txtTags2.Name = "txtTags2";
			this.txtTags2.Size = new System.Drawing.Size(253, 20);
			this.txtTags2.TabIndex = 3;
			this.txtTags2.Text = " ";
			// 
			// chkTags2
			// 
			this.chkTags2.Checked = true;
			this.chkTags2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags2.Location = new System.Drawing.Point(9, 42);
			this.chkTags2.Name = "chkTags2";
			this.chkTags2.Size = new System.Drawing.Size(18, 20);
			this.chkTags2.TabIndex = 2;
			this.chkTags2.UseVisualStyleBackColor = true;
			this.chkTags2.CheckedChanged += new System.EventHandler(this.chkTags2_CheckedChanged);
			// 
			// btnPost
			// 
			this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPost.Location = new System.Drawing.Point(425, 545);
			this.btnPost.Name = "btnPost";
			this.btnPost.Size = new System.Drawing.Size(117, 24);
			this.btnPost.TabIndex = 17;
			this.btnPost.Text = "Post to Tumblr";
			this.btnPost.UseVisualStyleBackColor = true;
			this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtURL.Location = new System.Drawing.Point(217, 441);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(325, 20);
			this.txtURL.TabIndex = 15;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.chkTags1);
			this.groupBox1.Controls.Add(this.txtTags1);
			this.groupBox1.Controls.Add(this.chkWeasylSubmitIdTag);
			this.groupBox1.Controls.Add(this.txtTags2);
			this.groupBox1.Controls.Add(this.chkTags2);
			this.groupBox1.Location = new System.Drawing.Point(142, 467);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 68);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tags";
			// 
			// lblLinkTo
			// 
			this.lblLinkTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblLinkTo.AutoSize = true;
			this.lblLinkTo.Location = new System.Drawing.Point(163, 444);
			this.lblLinkTo.Margin = new System.Windows.Forms.Padding(3);
			this.lblLinkTo.Name = "lblLinkTo";
			this.lblLinkTo.Size = new System.Drawing.Size(40, 13);
			this.lblLinkTo.TabIndex = 14;
			this.lblLinkTo.Text = "{URL}:";
			this.lblLinkTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblWeasylStatus1
			// 
			this.lblWeasylStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblWeasylStatus1.Location = new System.Drawing.Point(12, 544);
			this.lblWeasylStatus1.Margin = new System.Windows.Forms.Padding(3);
			this.lblWeasylStatus1.Name = "lblWeasylStatus1";
			this.lblWeasylStatus1.Size = new System.Drawing.Size(48, 24);
			this.lblWeasylStatus1.TabIndex = 38;
			this.lblWeasylStatus1.Text = "Weasyl:";
			this.lblWeasylStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTumblrStatus1
			// 
			this.lblTumblrStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTumblrStatus1.Location = new System.Drawing.Point(168, 544);
			this.lblTumblrStatus1.Margin = new System.Windows.Forms.Padding(3);
			this.lblTumblrStatus1.Name = "lblTumblrStatus1";
			this.lblTumblrStatus1.Size = new System.Drawing.Size(48, 24);
			this.lblTumblrStatus1.TabIndex = 39;
			this.lblTumblrStatus1.Text = "Tumblr:";
			this.lblTumblrStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblWeasylStatus2
			// 
			this.lblWeasylStatus2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblWeasylStatus2.Location = new System.Drawing.Point(66, 544);
			this.lblWeasylStatus2.Margin = new System.Windows.Forms.Padding(3);
			this.lblWeasylStatus2.Name = "lblWeasylStatus2";
			this.lblWeasylStatus2.Size = new System.Drawing.Size(96, 24);
			this.lblWeasylStatus2.TabIndex = 40;
			this.lblWeasylStatus2.Text = "not logged in";
			this.lblWeasylStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTumblrStatus2
			// 
			this.lblTumblrStatus2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTumblrStatus2.Location = new System.Drawing.Point(222, 544);
			this.lblTumblrStatus2.Margin = new System.Windows.Forms.Padding(3);
			this.lblTumblrStatus2.Name = "lblTumblrStatus2";
			this.lblTumblrStatus2.Size = new System.Drawing.Size(197, 24);
			this.lblTumblrStatus2.TabIndex = 41;
			this.lblTumblrStatus2.Text = "not logged in";
			this.lblTumblrStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(554, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// chkHTMLPreview
			// 
			this.chkHTMLPreview.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkHTMLPreview.Location = new System.Drawing.Point(142, 258);
			this.chkHTMLPreview.Name = "chkHTMLPreview";
			this.chkHTMLPreview.Size = new System.Drawing.Size(61, 20);
			this.chkHTMLPreview.TabIndex = 2;
			this.chkHTMLPreview.Text = "Preview";
			this.chkHTMLPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkHTMLPreview.UseVisualStyleBackColor = true;
			this.chkHTMLPreview.CheckedChanged += new System.EventHandler(this.chkHTMLPreview_CheckedChanged);
			// 
			// previewPanel
			// 
			this.previewPanel.Location = new System.Drawing.Point(142, 284);
			this.previewPanel.Name = "previewPanel";
			this.previewPanel.Size = new System.Drawing.Size(400, 177);
			this.previewPanel.TabIndex = 43;
			this.previewPanel.Visible = false;
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtTitle.Location = new System.Drawing.Point(217, 310);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(325, 20);
			this.txtTitle.TabIndex = 9;
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(163, 313);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(3);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(48, 13);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "{TITLE}:";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lProgressBar1
			// 
			this.lProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lProgressBar1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lProgressBar1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lProgressBar1.Location = new System.Drawing.Point(12, 545);
			this.lProgressBar1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.lProgressBar1.Maximum = 128;
			this.lProgressBar1.Minimum = 0;
			this.lProgressBar1.Name = "lProgressBar1";
			this.lProgressBar1.Size = new System.Drawing.Size(321, 24);
			this.lProgressBar1.TabIndex = 30;
			this.lProgressBar1.Value = 0;
			// 
			// thumbnail4
			// 
			this.thumbnail4.Location = new System.Drawing.Point(12, 417);
			this.thumbnail4.Name = "thumbnail4";
			this.thumbnail4.Size = new System.Drawing.Size(124, 124);
			this.thumbnail4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.thumbnail4.Submission = null;
			this.thumbnail4.TabIndex = 3;
			this.thumbnail4.TabStop = false;
			// 
			// thumbnail3
			// 
			this.thumbnail3.Location = new System.Drawing.Point(12, 287);
			this.thumbnail3.Name = "thumbnail3";
			this.thumbnail3.Size = new System.Drawing.Size(124, 124);
			this.thumbnail3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.thumbnail3.Submission = null;
			this.thumbnail3.TabIndex = 2;
			this.thumbnail3.TabStop = false;
			// 
			// thumbnail2
			// 
			this.thumbnail2.Location = new System.Drawing.Point(12, 157);
			this.thumbnail2.Name = "thumbnail2";
			this.thumbnail2.Size = new System.Drawing.Size(124, 124);
			this.thumbnail2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.thumbnail2.Submission = null;
			this.thumbnail2.TabIndex = 1;
			this.thumbnail2.TabStop = false;
			// 
			// thumbnail1
			// 
			this.thumbnail1.Location = new System.Drawing.Point(12, 27);
			this.thumbnail1.Name = "thumbnail1";
			this.thumbnail1.Size = new System.Drawing.Size(124, 124);
			this.thumbnail1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.thumbnail1.Submission = null;
			this.thumbnail1.TabIndex = 0;
			this.thumbnail1.TabStop = false;
			// 
			// btnViewExistingTumblrPost
			// 
			this.btnViewExistingTumblrPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnViewExistingTumblrPost.Location = new System.Drawing.Point(339, 545);
			this.btnViewExistingTumblrPost.Name = "btnViewExistingTumblrPost";
			this.btnViewExistingTumblrPost.Size = new System.Drawing.Size(80, 24);
			this.btnViewExistingTumblrPost.TabIndex = 44;
			this.btnViewExistingTumblrPost.Text = "View Existing";
			this.btnViewExistingTumblrPost.UseVisualStyleBackColor = true;
			this.btnViewExistingTumblrPost.Click += new System.EventHandler(this.btnViewExistingTumblrPost_Click);
			// 
			// WeasylForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(554, 581);
			this.Controls.Add(this.lProgressBar1);
			this.Controls.Add(this.btnViewExistingTumblrPost);
			this.Controls.Add(this.previewPanel);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.chkHTMLPreview);
			this.Controls.Add(this.lblTumblrStatus2);
			this.Controls.Add(this.lblWeasylStatus2);
			this.Controls.Add(this.lblTumblrStatus1);
			this.Controls.Add(this.lblWeasylStatus1);
			this.Controls.Add(this.lblLinkTo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.btnPost);
			this.Controls.Add(this.chkNow);
			this.Controls.Add(this.pickTime);
			this.Controls.Add(this.pickDate);
			this.Controls.Add(this.txtHeader);
			this.Controls.Add(this.chkFooter);
			this.Controls.Add(this.txtFooter);
			this.Controls.Add(this.chkDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.chkHeader);
			this.Controls.Add(this.mainPictureBox);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.thumbnail4);
			this.Controls.Add(this.thumbnail3);
			this.Controls.Add(this.thumbnail2);
			this.Controls.Add(this.thumbnail1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "WeasylForm";
			this.Text = "WeasylSync";
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WeasylThumbnail thumbnail1;
		private WeasylThumbnail thumbnail2;
		private WeasylThumbnail thumbnail3;
		private WeasylThumbnail thumbnail4;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.PictureBox mainPictureBox;
		private System.Windows.Forms.CheckBox chkHeader;
		private System.Windows.Forms.CheckBox chkDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtTags1;
		private System.Windows.Forms.CheckBox chkTags1;
		private System.Windows.Forms.CheckBox chkFooter;
		private System.Windows.Forms.TextBox txtFooter;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.DateTimePicker pickDate;
		private System.Windows.Forms.DateTimePicker pickTime;
		private System.Windows.Forms.CheckBox chkNow;
		private System.Windows.Forms.CheckBox chkWeasylSubmitIdTag;
		private WeasylSync.LProgressBar lProgressBar1;
		private System.Windows.Forms.TextBox txtTags2;
		private System.Windows.Forms.CheckBox chkTags2;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblLinkTo;
		private System.Windows.Forms.Label lblWeasylStatus1;
		private System.Windows.Forms.Label lblTumblrStatus1;
		private System.Windows.Forms.Label lblWeasylStatus2;
		private System.Windows.Forms.Label lblTumblrStatus2;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.CheckBox chkHTMLPreview;
		private System.Windows.Forms.Panel previewPanel;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Button btnViewExistingTumblrPost;
	}
}

