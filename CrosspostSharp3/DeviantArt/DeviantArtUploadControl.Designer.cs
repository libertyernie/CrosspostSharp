namespace CrosspostSharp3.DeviantArt {
	partial class DeviantArtUploadControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			lblArtistComments = new System.Windows.Forms.Label();
			txtArtistComments = new System.Windows.Forms.TextBox();
			lblTitle = new System.Windows.Forms.Label();
			txtTitle = new System.Windows.Forms.TextBox();
			txtTags = new System.Windows.Forms.TextBox();
			lblTags = new System.Windows.Forms.Label();
			grpMatureContent = new System.Windows.Forms.GroupBox();
			radStrict = new System.Windows.Forms.RadioButton();
			radModerate = new System.Windows.Forms.RadioButton();
			radNone = new System.Windows.Forms.RadioButton();
			grpMatureClassification = new System.Windows.Forms.GroupBox();
			chkIdeology = new System.Windows.Forms.CheckBox();
			chkLanguage = new System.Windows.Forms.CheckBox();
			chkGore = new System.Windows.Forms.CheckBox();
			chkSexual = new System.Windows.Forms.CheckBox();
			chkNudity = new System.Windows.Forms.CheckBox();
			chkAllowComments = new System.Windows.Forms.CheckBox();
			chkRequestCritique = new System.Windows.Forms.CheckBox();
			lblSharing = new System.Windows.Forms.Label();
			ddlSharing = new System.Windows.Forms.ComboBox();
			lblLicense = new System.Windows.Forms.Label();
			ddlLicense = new System.Windows.Forms.ComboBox();
			btnGalleryFolders = new System.Windows.Forms.Button();
			txtGalleryFolders = new System.Windows.Forms.TextBox();
			lblGalleryFolders = new System.Windows.Forms.Label();
			chkAllowFreeDownload = new System.Windows.Forms.CheckBox();
			btnPublish = new System.Windows.Forms.Button();
			chkAgree = new System.Windows.Forms.CheckBox();
			btnUpload = new System.Windows.Forms.Button();
			lblUploadTo = new System.Windows.Forms.Label();
			grpMatureContent.SuspendLayout();
			grpMatureClassification.SuspendLayout();
			SuspendLayout();
			// 
			// lblArtistComments
			// 
			lblArtistComments.Location = new System.Drawing.Point(4, 30);
			lblArtistComments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblArtistComments.Name = "lblArtistComments";
			lblArtistComments.Size = new System.Drawing.Size(98, 23);
			lblArtistComments.TabIndex = 2;
			lblArtistComments.Text = "Artist comments:";
			lblArtistComments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtArtistComments
			// 
			txtArtistComments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtArtistComments.Location = new System.Drawing.Point(4, 57);
			txtArtistComments.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtArtistComments.Multiline = true;
			txtArtistComments.Name = "txtArtistComments";
			txtArtistComments.Size = new System.Drawing.Size(298, 207);
			txtArtistComments.TabIndex = 3;
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Location = new System.Drawing.Point(4, 7);
			lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(32, 15);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Title:";
			// 
			// txtTitle
			// 
			txtTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTitle.Location = new System.Drawing.Point(46, 3);
			txtTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtTitle.Name = "txtTitle";
			txtTitle.Size = new System.Drawing.Size(256, 23);
			txtTitle.TabIndex = 1;
			// 
			// txtTags
			// 
			txtTags.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTags.Location = new System.Drawing.Point(50, 270);
			txtTags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtTags.Name = "txtTags";
			txtTags.Size = new System.Drawing.Size(251, 23);
			txtTags.TabIndex = 10;
			// 
			// lblTags
			// 
			lblTags.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblTags.AutoSize = true;
			lblTags.Location = new System.Drawing.Point(4, 273);
			lblTags.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblTags.Name = "lblTags";
			lblTags.Size = new System.Drawing.Size(33, 15);
			lblTags.TabIndex = 9;
			lblTags.Text = "Tags:";
			// 
			// grpMatureContent
			// 
			grpMatureContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			grpMatureContent.Controls.Add(radStrict);
			grpMatureContent.Controls.Add(radModerate);
			grpMatureContent.Controls.Add(radNone);
			grpMatureContent.Location = new System.Drawing.Point(309, 3);
			grpMatureContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			grpMatureContent.Name = "grpMatureContent";
			grpMatureContent.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			grpMatureContent.Size = new System.Drawing.Size(317, 48);
			grpMatureContent.TabIndex = 4;
			grpMatureContent.TabStop = false;
			grpMatureContent.Text = "Mature content";
			// 
			// radStrict
			// 
			radStrict.AutoSize = true;
			radStrict.Location = new System.Drawing.Point(162, 22);
			radStrict.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			radStrict.Name = "radStrict";
			radStrict.Size = new System.Drawing.Size(52, 19);
			radStrict.TabIndex = 2;
			radStrict.Text = "Strict";
			radStrict.UseVisualStyleBackColor = true;
			// 
			// radModerate
			// 
			radModerate.AutoSize = true;
			radModerate.Location = new System.Drawing.Point(74, 22);
			radModerate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			radModerate.Name = "radModerate";
			radModerate.Size = new System.Drawing.Size(76, 19);
			radModerate.TabIndex = 1;
			radModerate.Text = "Moderate";
			radModerate.UseVisualStyleBackColor = true;
			// 
			// radNone
			// 
			radNone.AutoSize = true;
			radNone.Checked = true;
			radNone.Location = new System.Drawing.Point(7, 22);
			radNone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			radNone.Name = "radNone";
			radNone.Size = new System.Drawing.Size(54, 19);
			radNone.TabIndex = 0;
			radNone.TabStop = true;
			radNone.Text = "None";
			radNone.UseVisualStyleBackColor = true;
			// 
			// grpMatureClassification
			// 
			grpMatureClassification.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			grpMatureClassification.Controls.Add(chkIdeology);
			grpMatureClassification.Controls.Add(chkLanguage);
			grpMatureClassification.Controls.Add(chkGore);
			grpMatureClassification.Controls.Add(chkSexual);
			grpMatureClassification.Controls.Add(chkNudity);
			grpMatureClassification.Enabled = false;
			grpMatureClassification.Location = new System.Drawing.Point(309, 59);
			grpMatureClassification.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			grpMatureClassification.Name = "grpMatureClassification";
			grpMatureClassification.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			grpMatureClassification.Size = new System.Drawing.Size(317, 73);
			grpMatureClassification.TabIndex = 5;
			grpMatureClassification.TabStop = false;
			grpMatureClassification.Text = "Classification";
			// 
			// chkIdeology
			// 
			chkIdeology.AutoSize = true;
			chkIdeology.Location = new System.Drawing.Point(135, 48);
			chkIdeology.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkIdeology.Name = "chkIdeology";
			chkIdeology.Size = new System.Drawing.Size(141, 19);
			chkIdeology.TabIndex = 4;
			chkIdeology.Text = "Ideologically sensitive";
			chkIdeology.UseVisualStyleBackColor = true;
			// 
			// chkLanguage
			// 
			chkLanguage.AutoSize = true;
			chkLanguage.Location = new System.Drawing.Point(7, 48);
			chkLanguage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkLanguage.Name = "chkLanguage";
			chkLanguage.Size = new System.Drawing.Size(113, 19);
			chkLanguage.TabIndex = 3;
			chkLanguage.Text = "Strong language";
			chkLanguage.UseVisualStyleBackColor = true;
			// 
			// chkGore
			// 
			chkGore.AutoSize = true;
			chkGore.Location = new System.Drawing.Point(197, 22);
			chkGore.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkGore.Name = "chkGore";
			chkGore.Size = new System.Drawing.Size(100, 19);
			chkGore.TabIndex = 2;
			chkGore.Text = "Violence/gore";
			chkGore.UseVisualStyleBackColor = true;
			// 
			// chkSexual
			// 
			chkSexual.AutoSize = true;
			chkSexual.Location = new System.Drawing.Point(79, 22);
			chkSexual.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkSexual.Name = "chkSexual";
			chkSexual.Size = new System.Drawing.Size(102, 19);
			chkSexual.TabIndex = 1;
			chkSexual.Text = "Sexual themes";
			chkSexual.UseVisualStyleBackColor = true;
			// 
			// chkNudity
			// 
			chkNudity.AutoSize = true;
			chkNudity.Location = new System.Drawing.Point(7, 22);
			chkNudity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkNudity.Name = "chkNudity";
			chkNudity.Size = new System.Drawing.Size(62, 19);
			chkNudity.TabIndex = 0;
			chkNudity.Text = "Nudity";
			chkNudity.UseVisualStyleBackColor = true;
			// 
			// chkAllowComments
			// 
			chkAllowComments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAllowComments.AutoSize = true;
			chkAllowComments.Checked = true;
			chkAllowComments.CheckState = System.Windows.Forms.CheckState.Checked;
			chkAllowComments.Location = new System.Drawing.Point(310, 163);
			chkAllowComments.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkAllowComments.Name = "chkAllowComments";
			chkAllowComments.Size = new System.Drawing.Size(116, 19);
			chkAllowComments.TabIndex = 7;
			chkAllowComments.Text = "Allow comments";
			chkAllowComments.UseVisualStyleBackColor = true;
			// 
			// chkRequestCritique
			// 
			chkRequestCritique.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkRequestCritique.AutoSize = true;
			chkRequestCritique.Location = new System.Drawing.Point(310, 188);
			chkRequestCritique.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkRequestCritique.Name = "chkRequestCritique";
			chkRequestCritique.Size = new System.Drawing.Size(111, 19);
			chkRequestCritique.TabIndex = 8;
			chkRequestCritique.Text = "Request critique";
			chkRequestCritique.UseVisualStyleBackColor = true;
			// 
			// lblSharing
			// 
			lblSharing.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblSharing.AutoSize = true;
			lblSharing.Location = new System.Drawing.Point(4, 302);
			lblSharing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblSharing.Name = "lblSharing";
			lblSharing.Size = new System.Drawing.Size(50, 15);
			lblSharing.TabIndex = 14;
			lblSharing.Text = "Sharing:";
			// 
			// ddlSharing
			// 
			ddlSharing.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			ddlSharing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ddlSharing.FormattingEnabled = true;
			ddlSharing.Items.AddRange(new object[] { "Show share buttons", "Hide share buttons", "Hide & require login to view" });
			ddlSharing.Location = new System.Drawing.Point(71, 299);
			ddlSharing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			ddlSharing.Name = "ddlSharing";
			ddlSharing.Size = new System.Drawing.Size(230, 23);
			ddlSharing.TabIndex = 15;
			// 
			// lblLicense
			// 
			lblLicense.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblLicense.AutoSize = true;
			lblLicense.Location = new System.Drawing.Point(318, 302);
			lblLicense.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblLicense.Name = "lblLicense";
			lblLicense.Size = new System.Drawing.Size(49, 15);
			lblLicense.TabIndex = 16;
			lblLicense.Text = "License:";
			// 
			// ddlLicense
			// 
			ddlLicense.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			ddlLicense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ddlLicense.FormattingEnabled = true;
			ddlLicense.Items.AddRange(new object[] { "Default", "CC-BY", "CC-BY-SA", "CC-BY-ND", "CC-BY-NC", "CC-BY-NC-SA", "CC-BY-NC-ND" });
			ddlLicense.Location = new System.Drawing.Point(414, 299);
			ddlLicense.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			ddlLicense.Name = "ddlLicense";
			ddlLicense.Size = new System.Drawing.Size(212, 23);
			ddlLicense.TabIndex = 17;
			// 
			// btnGalleryFolders
			// 
			btnGalleryFolders.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnGalleryFolders.Location = new System.Drawing.Point(596, 269);
			btnGalleryFolders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnGalleryFolders.Name = "btnGalleryFolders";
			btnGalleryFolders.Size = new System.Drawing.Size(30, 23);
			btnGalleryFolders.TabIndex = 13;
			btnGalleryFolders.Text = "...";
			btnGalleryFolders.UseVisualStyleBackColor = true;
			btnGalleryFolders.Click += btnGalleryFolders_Click;
			// 
			// txtGalleryFolders
			// 
			txtGalleryFolders.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			txtGalleryFolders.Location = new System.Drawing.Point(414, 269);
			txtGalleryFolders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtGalleryFolders.Name = "txtGalleryFolders";
			txtGalleryFolders.ReadOnly = true;
			txtGalleryFolders.Size = new System.Drawing.Size(174, 23);
			txtGalleryFolders.TabIndex = 12;
			// 
			// lblGalleryFolders
			// 
			lblGalleryFolders.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lblGalleryFolders.AutoSize = true;
			lblGalleryFolders.Location = new System.Drawing.Point(318, 272);
			lblGalleryFolders.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblGalleryFolders.Name = "lblGalleryFolders";
			lblGalleryFolders.Size = new System.Drawing.Size(85, 15);
			lblGalleryFolders.TabIndex = 11;
			lblGalleryFolders.Text = "Gallery folders:";
			// 
			// chkAllowFreeDownload
			// 
			chkAllowFreeDownload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAllowFreeDownload.AutoSize = true;
			chkAllowFreeDownload.Checked = true;
			chkAllowFreeDownload.CheckState = System.Windows.Forms.CheckState.Checked;
			chkAllowFreeDownload.Location = new System.Drawing.Point(310, 138);
			chkAllowFreeDownload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkAllowFreeDownload.Name = "chkAllowFreeDownload";
			chkAllowFreeDownload.Size = new System.Drawing.Size(112, 19);
			chkAllowFreeDownload.TabIndex = 6;
			chkAllowFreeDownload.Text = "Allow download";
			chkAllowFreeDownload.UseVisualStyleBackColor = true;
			// 
			// btnPublish
			// 
			btnPublish.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnPublish.AutoSize = true;
			btnPublish.Location = new System.Drawing.Point(545, 328);
			btnPublish.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnPublish.Name = "btnPublish";
			btnPublish.Size = new System.Drawing.Size(82, 27);
			btnPublish.TabIndex = 27;
			btnPublish.Text = "DeviantArt";
			btnPublish.UseVisualStyleBackColor = true;
			btnPublish.Click += btnPublish_Click;
			// 
			// chkAgree
			// 
			chkAgree.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkAgree.AutoSize = true;
			chkAgree.Location = new System.Drawing.Point(4, 333);
			chkAgree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkAgree.Name = "chkAgree";
			chkAgree.Size = new System.Drawing.Size(301, 19);
			chkAgree.TabIndex = 21;
			chkAgree.Text = "Agree to the Submission Policy and Terms of Service";
			chkAgree.UseVisualStyleBackColor = true;
			// 
			// btnUpload
			// 
			btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnUpload.AutoSize = true;
			btnUpload.Location = new System.Drawing.Point(479, 328);
			btnUpload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnUpload.Name = "btnUpload";
			btnUpload.Size = new System.Drawing.Size(58, 27);
			btnUpload.TabIndex = 26;
			btnUpload.Text = "Sta.sh";
			btnUpload.UseVisualStyleBackColor = true;
			btnUpload.Click += btnUpload_Click;
			// 
			// lblUploadTo
			// 
			lblUploadTo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lblUploadTo.AutoSize = true;
			lblUploadTo.Location = new System.Drawing.Point(407, 333);
			lblUploadTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblUploadTo.Name = "lblUploadTo";
			lblUploadTo.Size = new System.Drawing.Size(62, 15);
			lblUploadTo.TabIndex = 25;
			lblUploadTo.Text = "Upload to:";
			// 
			// DeviantArtUploadControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(lblUploadTo);
			Controls.Add(btnUpload);
			Controls.Add(chkAgree);
			Controls.Add(btnPublish);
			Controls.Add(chkAllowFreeDownload);
			Controls.Add(btnGalleryFolders);
			Controls.Add(txtGalleryFolders);
			Controls.Add(lblGalleryFolders);
			Controls.Add(ddlLicense);
			Controls.Add(lblLicense);
			Controls.Add(ddlSharing);
			Controls.Add(lblSharing);
			Controls.Add(chkRequestCritique);
			Controls.Add(chkAllowComments);
			Controls.Add(grpMatureClassification);
			Controls.Add(grpMatureContent);
			Controls.Add(txtTags);
			Controls.Add(lblTags);
			Controls.Add(txtArtistComments);
			Controls.Add(lblArtistComments);
			Controls.Add(txtTitle);
			Controls.Add(lblTitle);
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "DeviantArtUploadControl";
			Size = new System.Drawing.Size(630, 358);
			grpMatureContent.ResumeLayout(false);
			grpMatureContent.PerformLayout();
			grpMatureClassification.ResumeLayout(false);
			grpMatureClassification.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.Label lblArtistComments;
		private System.Windows.Forms.TextBox txtArtistComments;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.GroupBox grpMatureContent;
		private System.Windows.Forms.RadioButton radStrict;
		private System.Windows.Forms.RadioButton radModerate;
		private System.Windows.Forms.RadioButton radNone;
		private System.Windows.Forms.GroupBox grpMatureClassification;
		private System.Windows.Forms.CheckBox chkIdeology;
		private System.Windows.Forms.CheckBox chkLanguage;
		private System.Windows.Forms.CheckBox chkGore;
		private System.Windows.Forms.CheckBox chkSexual;
		private System.Windows.Forms.CheckBox chkNudity;
		private System.Windows.Forms.CheckBox chkAllowComments;
		private System.Windows.Forms.CheckBox chkRequestCritique;
		private System.Windows.Forms.Label lblSharing;
		private System.Windows.Forms.ComboBox ddlSharing;
		private System.Windows.Forms.Label lblLicense;
		private System.Windows.Forms.ComboBox ddlLicense;
		private System.Windows.Forms.Button btnGalleryFolders;
		private System.Windows.Forms.TextBox txtGalleryFolders;
		private System.Windows.Forms.Label lblGalleryFolders;
		private System.Windows.Forms.CheckBox chkAllowFreeDownload;
		private System.Windows.Forms.Button btnPublish;
		private System.Windows.Forms.CheckBox chkAgree;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.Label lblUploadTo;
	}
}
