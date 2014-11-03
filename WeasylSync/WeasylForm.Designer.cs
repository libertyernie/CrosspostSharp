﻿namespace WeasylSync {
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
			this.chkTitle = new System.Windows.Forms.CheckBox();
			this.chkDescription = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtTags1 = new System.Windows.Forms.TextBox();
			this.chkTags1 = new System.Windows.Forms.CheckBox();
			this.chkLink = new System.Windows.Forms.CheckBox();
			this.txtLink = new System.Windows.Forms.TextBox();
			this.chkTitleBold = new System.Windows.Forms.CheckBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.pickDate = new System.Windows.Forms.DateTimePicker();
			this.pickTime = new System.Windows.Forms.DateTimePicker();
			this.chkNow = new System.Windows.Forms.CheckBox();
			this.chkWeasylSubmitIdTag = new System.Windows.Forms.CheckBox();
			this.lblLink = new System.Windows.Forms.Label();
			this.lProgressBar1 = new WeasylSync.LProgressBar();
			this.thumbnail4 = new WeasylSync.WeasylThumbnail();
			this.thumbnail3 = new WeasylSync.WeasylThumbnail();
			this.thumbnail2 = new WeasylSync.WeasylThumbnail();
			this.thumbnail1 = new WeasylSync.WeasylThumbnail();
			this.txtTags2 = new System.Windows.Forms.TextBox();
			this.chkTags2 = new System.Windows.Forms.CheckBox();
			this.btnPost = new System.Windows.Forms.Button();
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
			this.txtDescription.Size = new System.Drawing.Size(376, 73);
			this.txtDescription.TabIndex = 9;
			this.txtDescription.Text = "L1\r\nL2\r\nL3\r\nL4\r\nL5\r\nL6";
			// 
			// txtTags1
			// 
			this.txtTags1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags1.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTags1.Location = new System.Drawing.Point(162, 418);
			this.txtTags1.Name = "txtTags1";
			this.txtTags1.Size = new System.Drawing.Size(376, 28);
			this.txtTags1.TabIndex = 11;
			// 
			// chkTags1
			// 
			this.chkTags1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTags1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTags1.Checked = true;
			this.chkTags1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags1.Location = new System.Drawing.Point(138, 418);
			this.chkTags1.Name = "chkTags1";
			this.chkTags1.Size = new System.Drawing.Size(18, 28);
			this.chkTags1.TabIndex = 12;
			this.chkTags1.UseVisualStyleBackColor = true;
			// 
			// chkLink
			// 
			this.chkLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkLink.Checked = true;
			this.chkLink.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLink.Location = new System.Drawing.Point(138, 384);
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
			this.txtLink.Location = new System.Drawing.Point(162, 384);
			this.txtLink.Name = "txtLink";
			this.txtLink.Size = new System.Drawing.Size(376, 28);
			this.txtLink.TabIndex = 15;
			this.txtLink.Text = "View on Weasyl";
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
			this.lblLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblLink.Location = new System.Drawing.Point(138, 487);
			this.lblLink.Name = "lblLink";
			this.lblLink.Size = new System.Drawing.Size(277, 23);
			this.lblLink.TabIndex = 22;
			this.lblLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lProgressBar1
			// 
			this.lProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lProgressBar1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lProgressBar1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lProgressBar1.Location = new System.Drawing.Point(138, 487);
			this.lProgressBar1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.lProgressBar1.Maximum = 128;
			this.lProgressBar1.Minimum = 0;
			this.lProgressBar1.Name = "lProgressBar1";
			this.lProgressBar1.Size = new System.Drawing.Size(277, 23);
			this.lProgressBar1.TabIndex = 30;
			this.lProgressBar1.Value = 0;
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
			// txtTags2
			// 
			this.txtTags2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags2.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTags2.Location = new System.Drawing.Point(162, 452);
			this.txtTags2.Name = "txtTags2";
			this.txtTags2.Size = new System.Drawing.Size(253, 28);
			this.txtTags2.TabIndex = 31;
			// 
			// chkTags2
			// 
			this.chkTags2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTags2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTags2.Checked = true;
			this.chkTags2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags2.Location = new System.Drawing.Point(138, 452);
			this.chkTags2.Name = "chkTags2";
			this.chkTags2.Size = new System.Drawing.Size(18, 28);
			this.chkTags2.TabIndex = 32;
			this.chkTags2.UseVisualStyleBackColor = true;
			// 
			// btnPost
			// 
			this.btnPost.Location = new System.Drawing.Point(421, 486);
			this.btnPost.Name = "btnPost";
			this.btnPost.Size = new System.Drawing.Size(117, 24);
			this.btnPost.TabIndex = 33;
			this.btnPost.Text = "Post to Tumblr";
			this.btnPost.UseVisualStyleBackColor = true;
			this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
			// 
			// WeasylForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(550, 522);
			this.Controls.Add(this.btnPost);
			this.Controls.Add(this.chkTags2);
			this.Controls.Add(this.txtTags2);
			this.Controls.Add(this.lProgressBar1);
			this.Controls.Add(this.chkWeasylSubmitIdTag);
			this.Controls.Add(this.chkNow);
			this.Controls.Add(this.pickTime);
			this.Controls.Add(this.pickDate);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.chkTitleBold);
			this.Controls.Add(this.chkLink);
			this.Controls.Add(this.txtLink);
			this.Controls.Add(this.chkTags1);
			this.Controls.Add(this.txtTags1);
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
			this.Name = "WeasylForm";
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
		private System.Windows.Forms.TextBox txtTags1;
		private System.Windows.Forms.CheckBox chkTags1;
		private System.Windows.Forms.CheckBox chkLink;
		private System.Windows.Forms.TextBox txtLink;
		private System.Windows.Forms.CheckBox chkTitleBold;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.DateTimePicker pickDate;
		private System.Windows.Forms.DateTimePicker pickTime;
		private System.Windows.Forms.CheckBox chkNow;
		private System.Windows.Forms.CheckBox chkWeasylSubmitIdTag;
		private System.Windows.Forms.Label lblLink;
		private WeasylSync.LProgressBar lProgressBar1;
		private System.Windows.Forms.TextBox txtTags2;
		private System.Windows.Forms.CheckBox chkTags2;
		private System.Windows.Forms.Button btnPost;
	}
}
