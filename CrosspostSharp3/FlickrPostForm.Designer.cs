namespace CrosspostSharp3 {
	partial class FlickrPostForm {
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
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnPost = new System.Windows.Forms.Button();
            this.lblTags = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.lblUsername2 = new System.Windows.Forms.Label();
            this.groupFlickrContentType = new System.Windows.Forms.GroupBox();
            this.radFlickrOther = new System.Windows.Forms.RadioButton();
            this.radFlickrScreenshot = new System.Windows.Forms.RadioButton();
            this.radFlickrPhoto = new System.Windows.Forms.RadioButton();
            this.groupFlickrSafetyLevel = new System.Windows.Forms.GroupBox();
            this.radFlickrRestricted = new System.Windows.Forms.RadioButton();
            this.radFlickrModerate = new System.Windows.Forms.RadioButton();
            this.radFlickrSafe = new System.Windows.Forms.RadioButton();
            this.groupFlickrPermissions = new System.Windows.Forms.GroupBox();
            this.chkFlickrFamily = new System.Windows.Forms.CheckBox();
            this.chkFlickrFriend = new System.Windows.Forms.CheckBox();
            this.chkFlickrPublic = new System.Windows.Forms.CheckBox();
            this.ddlFlickrLicense = new System.Windows.Forms.ComboBox();
            this.chkFlickrHidden = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.groupFlickrContentType.SuspendLayout();
            this.groupFlickrSafetyLevel.SuspendLayout();
            this.groupFlickrPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(12, 118);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(360, 75);
            this.txtDescription.TabIndex = 5;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(297, 326);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 12;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // lblTags
            // 
            this.lblTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 196);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(31, 13);
            this.lblTags.TabIndex = 6;
            this.lblTags.Text = "Tags";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(12, 212);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(360, 20);
            this.txtTags.TabIndex = 7;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 63);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(12, 79);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(360, 20);
            this.txtTitle.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 102);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(119, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description (Markdown)";
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(12, 12);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(48, 48);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 20;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername1
            // 
            this.lblUsername1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername1.Location = new System.Drawing.Point(66, 12);
            this.lblUsername1.Name = "lblUsername1";
            this.lblUsername1.Size = new System.Drawing.Size(306, 17);
            this.lblUsername1.TabIndex = 0;
            this.lblUsername1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsername2
            // 
            this.lblUsername2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername2.AutoEllipsis = true;
            this.lblUsername2.Location = new System.Drawing.Point(66, 29);
            this.lblUsername2.Name = "lblUsername2";
            this.lblUsername2.Size = new System.Drawing.Size(306, 13);
            this.lblUsername2.TabIndex = 1;
            this.lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupFlickrContentType
            // 
            this.groupFlickrContentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupFlickrContentType.Controls.Add(this.radFlickrOther);
            this.groupFlickrContentType.Controls.Add(this.radFlickrScreenshot);
            this.groupFlickrContentType.Controls.Add(this.radFlickrPhoto);
            this.groupFlickrContentType.Location = new System.Drawing.Point(189, 238);
            this.groupFlickrContentType.Name = "groupFlickrContentType";
            this.groupFlickrContentType.Size = new System.Drawing.Size(85, 88);
            this.groupFlickrContentType.TabIndex = 23;
            this.groupFlickrContentType.TabStop = false;
            this.groupFlickrContentType.Text = "Content type";
            // 
            // radFlickrOther
            // 
            this.radFlickrOther.AutoSize = true;
            this.radFlickrOther.Checked = true;
            this.radFlickrOther.Location = new System.Drawing.Point(6, 65);
            this.radFlickrOther.Name = "radFlickrOther";
            this.radFlickrOther.Size = new System.Drawing.Size(51, 17);
            this.radFlickrOther.TabIndex = 2;
            this.radFlickrOther.TabStop = true;
            this.radFlickrOther.Text = "Other";
            this.radFlickrOther.UseVisualStyleBackColor = true;
            // 
            // radFlickrScreenshot
            // 
            this.radFlickrScreenshot.AutoSize = true;
            this.radFlickrScreenshot.Location = new System.Drawing.Point(6, 42);
            this.radFlickrScreenshot.Name = "radFlickrScreenshot";
            this.radFlickrScreenshot.Size = new System.Drawing.Size(79, 17);
            this.radFlickrScreenshot.TabIndex = 1;
            this.radFlickrScreenshot.Text = "Screenshot";
            this.radFlickrScreenshot.UseVisualStyleBackColor = true;
            // 
            // radFlickrPhoto
            // 
            this.radFlickrPhoto.AutoSize = true;
            this.radFlickrPhoto.Location = new System.Drawing.Point(6, 19);
            this.radFlickrPhoto.Name = "radFlickrPhoto";
            this.radFlickrPhoto.Size = new System.Drawing.Size(53, 17);
            this.radFlickrPhoto.TabIndex = 0;
            this.radFlickrPhoto.Text = "Photo";
            this.radFlickrPhoto.UseVisualStyleBackColor = true;
            // 
            // groupFlickrSafetyLevel
            // 
            this.groupFlickrSafetyLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupFlickrSafetyLevel.Controls.Add(this.radFlickrRestricted);
            this.groupFlickrSafetyLevel.Controls.Add(this.radFlickrModerate);
            this.groupFlickrSafetyLevel.Controls.Add(this.radFlickrSafe);
            this.groupFlickrSafetyLevel.Location = new System.Drawing.Point(98, 238);
            this.groupFlickrSafetyLevel.Name = "groupFlickrSafetyLevel";
            this.groupFlickrSafetyLevel.Size = new System.Drawing.Size(85, 88);
            this.groupFlickrSafetyLevel.TabIndex = 22;
            this.groupFlickrSafetyLevel.TabStop = false;
            this.groupFlickrSafetyLevel.Text = "Safety level";
            // 
            // radFlickrRestricted
            // 
            this.radFlickrRestricted.AutoSize = true;
            this.radFlickrRestricted.Location = new System.Drawing.Point(6, 65);
            this.radFlickrRestricted.Name = "radFlickrRestricted";
            this.radFlickrRestricted.Size = new System.Drawing.Size(73, 17);
            this.radFlickrRestricted.TabIndex = 2;
            this.radFlickrRestricted.TabStop = true;
            this.radFlickrRestricted.Text = "Restricted";
            this.radFlickrRestricted.UseVisualStyleBackColor = true;
            // 
            // radFlickrModerate
            // 
            this.radFlickrModerate.AutoSize = true;
            this.radFlickrModerate.Location = new System.Drawing.Point(6, 42);
            this.radFlickrModerate.Name = "radFlickrModerate";
            this.radFlickrModerate.Size = new System.Drawing.Size(70, 17);
            this.radFlickrModerate.TabIndex = 1;
            this.radFlickrModerate.TabStop = true;
            this.radFlickrModerate.Text = "Moderate";
            this.radFlickrModerate.UseVisualStyleBackColor = true;
            // 
            // radFlickrSafe
            // 
            this.radFlickrSafe.AutoSize = true;
            this.radFlickrSafe.Location = new System.Drawing.Point(6, 19);
            this.radFlickrSafe.Name = "radFlickrSafe";
            this.radFlickrSafe.Size = new System.Drawing.Size(47, 17);
            this.radFlickrSafe.TabIndex = 0;
            this.radFlickrSafe.TabStop = true;
            this.radFlickrSafe.Text = "Safe";
            this.radFlickrSafe.UseVisualStyleBackColor = true;
            // 
            // groupFlickrPermissions
            // 
            this.groupFlickrPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupFlickrPermissions.Controls.Add(this.chkFlickrFamily);
            this.groupFlickrPermissions.Controls.Add(this.chkFlickrFriend);
            this.groupFlickrPermissions.Controls.Add(this.chkFlickrPublic);
            this.groupFlickrPermissions.Location = new System.Drawing.Point(12, 238);
            this.groupFlickrPermissions.Name = "groupFlickrPermissions";
            this.groupFlickrPermissions.Size = new System.Drawing.Size(80, 88);
            this.groupFlickrPermissions.TabIndex = 21;
            this.groupFlickrPermissions.TabStop = false;
            this.groupFlickrPermissions.Text = "Permissions";
            // 
            // chkFlickrFamily
            // 
            this.chkFlickrFamily.AutoSize = true;
            this.chkFlickrFamily.Location = new System.Drawing.Point(6, 65);
            this.chkFlickrFamily.Name = "chkFlickrFamily";
            this.chkFlickrFamily.Size = new System.Drawing.Size(55, 17);
            this.chkFlickrFamily.TabIndex = 2;
            this.chkFlickrFamily.Text = "Family";
            this.chkFlickrFamily.UseVisualStyleBackColor = true;
            // 
            // chkFlickrFriend
            // 
            this.chkFlickrFriend.AutoSize = true;
            this.chkFlickrFriend.Location = new System.Drawing.Point(6, 42);
            this.chkFlickrFriend.Name = "chkFlickrFriend";
            this.chkFlickrFriend.Size = new System.Drawing.Size(60, 17);
            this.chkFlickrFriend.TabIndex = 1;
            this.chkFlickrFriend.Text = "Friends";
            this.chkFlickrFriend.UseVisualStyleBackColor = true;
            // 
            // chkFlickrPublic
            // 
            this.chkFlickrPublic.AutoSize = true;
            this.chkFlickrPublic.Checked = true;
            this.chkFlickrPublic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFlickrPublic.Location = new System.Drawing.Point(6, 19);
            this.chkFlickrPublic.Name = "chkFlickrPublic";
            this.chkFlickrPublic.Size = new System.Drawing.Size(55, 17);
            this.chkFlickrPublic.TabIndex = 0;
            this.chkFlickrPublic.Text = "Public";
            this.chkFlickrPublic.UseVisualStyleBackColor = true;
            // 
            // ddlFlickrLicense
            // 
            this.ddlFlickrLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFlickrLicense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFlickrLicense.FormattingEnabled = true;
            this.ddlFlickrLicense.Location = new System.Drawing.Point(12, 328);
            this.ddlFlickrLicense.Name = "ddlFlickrLicense";
            this.ddlFlickrLicense.Size = new System.Drawing.Size(125, 21);
            this.ddlFlickrLicense.TabIndex = 24;
            // 
            // chkFlickrHidden
            // 
            this.chkFlickrHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFlickrHidden.AutoSize = true;
            this.chkFlickrHidden.Location = new System.Drawing.Point(143, 330);
            this.chkFlickrHidden.Name = "chkFlickrHidden";
            this.chkFlickrHidden.Size = new System.Drawing.Size(148, 17);
            this.chkFlickrHidden.TabIndex = 25;
            this.chkFlickrHidden.Text = "Hide from public searches";
            this.chkFlickrHidden.UseVisualStyleBackColor = true;
            // 
            // FlickrPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.chkFlickrHidden);
            this.Controls.Add(this.ddlFlickrLicense);
            this.Controls.Add(this.groupFlickrContentType);
            this.Controls.Add(this.groupFlickrSafetyLevel);
            this.Controls.Add(this.groupFlickrPermissions);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.txtDescription);
            this.Name = "FlickrPostForm";
            this.Text = "Post to Inkbunny";
            this.Shown += new System.EventHandler(this.Form_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.groupFlickrContentType.ResumeLayout(false);
            this.groupFlickrContentType.PerformLayout();
            this.groupFlickrSafetyLevel.ResumeLayout(false);
            this.groupFlickrSafetyLevel.PerformLayout();
            this.groupFlickrPermissions.ResumeLayout(false);
            this.groupFlickrPermissions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.Label lblUsername2;
		private System.Windows.Forms.GroupBox groupFlickrContentType;
		private System.Windows.Forms.RadioButton radFlickrOther;
		private System.Windows.Forms.RadioButton radFlickrScreenshot;
		private System.Windows.Forms.RadioButton radFlickrPhoto;
		private System.Windows.Forms.GroupBox groupFlickrSafetyLevel;
		private System.Windows.Forms.RadioButton radFlickrRestricted;
		private System.Windows.Forms.RadioButton radFlickrModerate;
		private System.Windows.Forms.RadioButton radFlickrSafe;
		private System.Windows.Forms.GroupBox groupFlickrPermissions;
		private System.Windows.Forms.CheckBox chkFlickrFamily;
		private System.Windows.Forms.CheckBox chkFlickrFriend;
		private System.Windows.Forms.CheckBox chkFlickrPublic;
		private System.Windows.Forms.ComboBox ddlFlickrLicense;
		private System.Windows.Forms.CheckBox chkFlickrHidden;
	}
}