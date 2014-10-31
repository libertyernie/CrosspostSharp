namespace WeasylSync {
	partial class Form1 {
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
			this.chkTitle = new System.Windows.Forms.CheckBox();
			this.chkDescription = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtTags = new System.Windows.Forms.TextBox();
			this.chkTags = new System.Windows.Forms.CheckBox();
			this.chkWeasylTag = new System.Windows.Forms.CheckBox();
			this.chkLink = new System.Windows.Forms.CheckBox();
			this.txtLink = new System.Windows.Forms.TextBox();
			this.btnEmail = new System.Windows.Forms.Button();
			this.chkTitleBold = new System.Windows.Forms.CheckBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.pickDate = new System.Windows.Forms.DateTimePicker();
			this.pickTime = new System.Windows.Forms.DateTimePicker();
			this.chkNow = new System.Windows.Forms.CheckBox();
			this.chkWeasylSubmitIdTag = new System.Windows.Forms.CheckBox();
			this.lblLink = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblDiagnostic = new System.Windows.Forms.Label();
			this.thumbnail4 = new WeasylSync.WeasylThumbnail();
			this.thumbnail3 = new WeasylSync.WeasylThumbnail();
			this.thumbnail2 = new WeasylSync.WeasylThumbnail();
			this.thumbnail1 = new WeasylSync.WeasylThumbnail();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnUp
			// 
			this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUp.Location = new System.Drawing.Point(100, 12);
			this.btnUp.Margin = new System.Windows.Forms.Padding(0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(32, 32);
			this.btnUp.TabIndex = 4;
			this.btnUp.Text = "↑";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDown.Location = new System.Drawing.Point(100, 478);
			this.btnDown.Margin = new System.Windows.Forms.Padding(0);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(32, 32);
			this.btnDown.TabIndex = 5;
			this.btnDown.Text = "↓";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(138, 12);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(400, 225);
			this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.mainPictureBox.TabIndex = 6;
			this.mainPictureBox.TabStop = false;
			// 
			// chkTitle
			// 
			this.chkTitle.Checked = true;
			this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTitle.Location = new System.Drawing.Point(138, 243);
			this.chkTitle.Name = "chkTitle";
			this.chkTitle.Size = new System.Drawing.Size(18, 28);
			this.chkTitle.TabIndex = 8;
			this.chkTitle.UseVisualStyleBackColor = true;
			// 
			// chkDescription
			// 
			this.chkDescription.Checked = true;
			this.chkDescription.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDescription.Location = new System.Drawing.Point(138, 305);
			this.chkDescription.Name = "chkDescription";
			this.chkDescription.Size = new System.Drawing.Size(18, 28);
			this.chkDescription.TabIndex = 10;
			this.chkDescription.UseVisualStyleBackColor = true;
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDescription.Location = new System.Drawing.Point(162, 305);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(376, 108);
			this.txtDescription.TabIndex = 9;
			this.txtDescription.Text = "L1\r\nL2\r\nL3\r\nL4\r\nL5\r\nL6";
			// 
			// txtTags
			// 
			this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTags.Location = new System.Drawing.Point(162, 453);
			this.txtTags.Name = "txtTags";
			this.txtTags.Size = new System.Drawing.Size(190, 28);
			this.txtTags.TabIndex = 11;
			// 
			// chkTags
			// 
			this.chkTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTags.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTags.Checked = true;
			this.chkTags.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags.Location = new System.Drawing.Point(138, 453);
			this.chkTags.Name = "chkTags";
			this.chkTags.Size = new System.Drawing.Size(18, 28);
			this.chkTags.TabIndex = 12;
			this.chkTags.UseVisualStyleBackColor = true;
			// 
			// chkWeasylTag
			// 
			this.chkWeasylTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkWeasylTag.AutoEllipsis = true;
			this.chkWeasylTag.Checked = true;
			this.chkWeasylTag.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWeasylTag.Location = new System.Drawing.Point(358, 453);
			this.chkWeasylTag.Name = "chkWeasylTag";
			this.chkWeasylTag.Size = new System.Drawing.Size(66, 28);
			this.chkWeasylTag.TabIndex = 13;
			this.chkWeasylTag.Text = "#weasyl";
			this.chkWeasylTag.UseVisualStyleBackColor = true;
			// 
			// chkLink
			// 
			this.chkLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkLink.Checked = true;
			this.chkLink.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLink.Location = new System.Drawing.Point(138, 419);
			this.chkLink.Name = "chkLink";
			this.chkLink.Size = new System.Drawing.Size(18, 28);
			this.chkLink.TabIndex = 16;
			this.chkLink.UseVisualStyleBackColor = true;
			// 
			// txtLink
			// 
			this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLink.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLink.ForeColor = System.Drawing.Color.Blue;
			this.txtLink.Location = new System.Drawing.Point(162, 419);
			this.txtLink.Name = "txtLink";
			this.txtLink.Size = new System.Drawing.Size(376, 28);
			this.txtLink.TabIndex = 15;
			this.txtLink.Text = "View on Weasyl";
			// 
			// btnEmail
			// 
			this.btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEmail.Location = new System.Drawing.Point(421, 487);
			this.btnEmail.Name = "btnEmail";
			this.btnEmail.Size = new System.Drawing.Size(117, 23);
			this.btnEmail.TabIndex = 17;
			this.btnEmail.Text = "Email to Tumblr";
			this.btnEmail.UseVisualStyleBackColor = true;
			this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
			// 
			// chkTitleBold
			// 
			this.chkTitleBold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkTitleBold.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkTitleBold.Location = new System.Drawing.Point(510, 243);
			this.chkTitleBold.Name = "chkTitleBold";
			this.chkTitleBold.Size = new System.Drawing.Size(28, 28);
			this.chkTitleBold.TabIndex = 18;
			this.chkTitleBold.Text = "B";
			this.chkTitleBold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTitleBold.UseVisualStyleBackColor = true;
			this.chkTitleBold.CheckedChanged += new System.EventHandler(this.chkTitleBold_CheckedChanged);
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTitle.Location = new System.Drawing.Point(162, 243);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTitle.Size = new System.Drawing.Size(342, 28);
			this.txtTitle.TabIndex = 7;
			// 
			// pickDate
			// 
			this.pickDate.Enabled = false;
			this.pickDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.pickDate.Location = new System.Drawing.Point(162, 277);
			this.pickDate.Name = "pickDate";
			this.pickDate.Size = new System.Drawing.Size(110, 22);
			this.pickDate.TabIndex = 23;
			// 
			// pickTime
			// 
			this.pickTime.Enabled = false;
			this.pickTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.pickTime.Location = new System.Drawing.Point(278, 277);
			this.pickTime.Name = "pickTime";
			this.pickTime.ShowUpDown = true;
			this.pickTime.Size = new System.Drawing.Size(110, 22);
			this.pickTime.TabIndex = 24;
			// 
			// chkNow
			// 
			this.chkNow.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkNow.Checked = true;
			this.chkNow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNow.Location = new System.Drawing.Point(394, 277);
			this.chkNow.Name = "chkNow";
			this.chkNow.Size = new System.Drawing.Size(64, 22);
			this.chkNow.TabIndex = 26;
			this.chkNow.Text = "Now";
			this.chkNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkNow.UseVisualStyleBackColor = true;
			this.chkNow.CheckedChanged += new System.EventHandler(this.chkNow_CheckedChanged);
			// 
			// chkWeasylSubmitIdTag
			// 
			this.chkWeasylSubmitIdTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkWeasylSubmitIdTag.AutoEllipsis = true;
			this.chkWeasylSubmitIdTag.Checked = true;
			this.chkWeasylSubmitIdTag.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWeasylSubmitIdTag.Location = new System.Drawing.Point(430, 453);
			this.chkWeasylSubmitIdTag.Name = "chkWeasylSubmitIdTag";
			this.chkWeasylSubmitIdTag.Size = new System.Drawing.Size(108, 28);
			this.chkWeasylSubmitIdTag.TabIndex = 27;
			this.chkWeasylSubmitIdTag.Text = "#weasyl000000";
			this.chkWeasylSubmitIdTag.UseVisualStyleBackColor = true;
			// 
			// lblLink
			// 
			this.lblLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblLink.Location = new System.Drawing.Point(138, 487);
			this.lblLink.Name = "lblLink";
			this.lblLink.Size = new System.Drawing.Size(277, 23);
			this.lblLink.TabIndex = 22;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar1.Location = new System.Drawing.Point(141, 487);
			this.progressBar1.Maximum = 128;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(274, 23);
			this.progressBar1.Step = 16;
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 28;
			this.progressBar1.Visible = false;
			// 
			// lblDiagnostic
			// 
			this.lblDiagnostic.AutoSize = true;
			this.lblDiagnostic.Location = new System.Drawing.Point(465, 278);
			this.lblDiagnostic.Name = "lblDiagnostic";
			this.lblDiagnostic.Size = new System.Drawing.Size(88, 17);
			this.lblDiagnostic.TabIndex = 29;
			this.lblDiagnostic.Text = "lblDiagnostic";
			// 
			// thumbnail4
			// 
			this.thumbnail4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail4.Location = new System.Drawing.Point(12, 390);
			this.thumbnail4.Name = "thumbnail4";
			this.thumbnail4.Size = new System.Drawing.Size(120, 120);
			this.thumbnail4.Submission = null;
			this.thumbnail4.TabIndex = 3;
			this.thumbnail4.TabStop = false;
			// 
			// thumbnail3
			// 
			this.thumbnail3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail3.Location = new System.Drawing.Point(12, 264);
			this.thumbnail3.Name = "thumbnail3";
			this.thumbnail3.Size = new System.Drawing.Size(120, 120);
			this.thumbnail3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail3.Submission = null;
			this.thumbnail3.TabIndex = 2;
			this.thumbnail3.TabStop = false;
			// 
			// thumbnail2
			// 
			this.thumbnail2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail2.Location = new System.Drawing.Point(12, 138);
			this.thumbnail2.Name = "thumbnail2";
			this.thumbnail2.Size = new System.Drawing.Size(120, 120);
			this.thumbnail2.Submission = null;
			this.thumbnail2.TabIndex = 1;
			this.thumbnail2.TabStop = false;
			// 
			// thumbnail1
			// 
			this.thumbnail1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail1.Location = new System.Drawing.Point(12, 12);
			this.thumbnail1.Name = "thumbnail1";
			this.thumbnail1.Size = new System.Drawing.Size(120, 120);
			this.thumbnail1.Submission = null;
			this.thumbnail1.TabIndex = 0;
			this.thumbnail1.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(550, 522);
			this.Controls.Add(this.lblDiagnostic);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.chkWeasylSubmitIdTag);
			this.Controls.Add(this.chkNow);
			this.Controls.Add(this.pickTime);
			this.Controls.Add(this.pickDate);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.chkTitleBold);
			this.Controls.Add(this.btnEmail);
			this.Controls.Add(this.chkLink);
			this.Controls.Add(this.txtLink);
			this.Controls.Add(this.chkWeasylTag);
			this.Controls.Add(this.chkTags);
			this.Controls.Add(this.txtTags);
			this.Controls.Add(this.chkDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.chkTitle);
			this.Controls.Add(this.mainPictureBox);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.thumbnail4);
			this.Controls.Add(this.thumbnail3);
			this.Controls.Add(this.thumbnail2);
			this.Controls.Add(this.thumbnail1);
			this.Controls.Add(this.lblLink);
			this.Name = "Form1";
			this.Text = "WeasylSync";
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
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
		private System.Windows.Forms.CheckBox chkTitle;
		private System.Windows.Forms.CheckBox chkDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.CheckBox chkTags;
		private System.Windows.Forms.CheckBox chkWeasylTag;
		private System.Windows.Forms.CheckBox chkLink;
		private System.Windows.Forms.TextBox txtLink;
		private System.Windows.Forms.Button btnEmail;
		private System.Windows.Forms.CheckBox chkTitleBold;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.DateTimePicker pickDate;
		private System.Windows.Forms.DateTimePicker pickTime;
		private System.Windows.Forms.CheckBox chkNow;
		private System.Windows.Forms.CheckBox chkWeasylSubmitIdTag;
		private System.Windows.Forms.Label lblLink;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblDiagnostic;
	}
}

