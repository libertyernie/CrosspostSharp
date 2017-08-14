namespace DeviantArtControls {
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
            this.lblArtistComments = new System.Windows.Forms.Label();
            this.txtArtistComments = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTags = new System.Windows.Forms.Label();
            this.grpMatureContent = new System.Windows.Forms.GroupBox();
            this.radStrict = new System.Windows.Forms.RadioButton();
            this.radModerate = new System.Windows.Forms.RadioButton();
            this.radNone = new System.Windows.Forms.RadioButton();
            this.grpMatureClassification = new System.Windows.Forms.GroupBox();
            this.chkIdeology = new System.Windows.Forms.CheckBox();
            this.chkLanguage = new System.Windows.Forms.CheckBox();
            this.chkGore = new System.Windows.Forms.CheckBox();
            this.chkSexual = new System.Windows.Forms.CheckBox();
            this.chkNudity = new System.Windows.Forms.CheckBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.btnCategory = new System.Windows.Forms.Button();
            this.chkAllowComments = new System.Windows.Forms.CheckBox();
            this.chkRequestCritique = new System.Windows.Forms.CheckBox();
            this.lblSharing = new System.Windows.Forms.Label();
            this.ddlSharing = new System.Windows.Forms.ComboBox();
            this.lblLicense = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnGalleryFolders = new System.Windows.Forms.Button();
            this.txtGalleryFolders = new System.Windows.Forms.TextBox();
            this.lblGalleryFolders = new System.Windows.Forms.Label();
            this.chkAllowFreeDownload = new System.Windows.Forms.CheckBox();
            this.btnPublish = new System.Windows.Forms.Button();
            this.grpMatureContent.SuspendLayout();
            this.grpMatureClassification.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblArtistComments
            // 
            this.lblArtistComments.Location = new System.Drawing.Point(3, 26);
            this.lblArtistComments.Name = "lblArtistComments";
            this.lblArtistComments.Size = new System.Drawing.Size(84, 20);
            this.lblArtistComments.TabIndex = 2;
            this.lblArtistComments.Text = "Artist comments:";
            this.lblArtistComments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtArtistComments
            // 
            this.txtArtistComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArtistComments.Location = new System.Drawing.Point(6, 49);
            this.txtArtistComments.Multiline = true;
            this.txtArtistComments.Name = "txtArtistComments";
            this.txtArtistComments.Size = new System.Drawing.Size(531, 63);
            this.txtArtistComments.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(39, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(498, 20);
            this.txtTitle.TabIndex = 1;
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(61, 118);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(476, 20);
            this.txtTags.TabIndex = 5;
            // 
            // lblTags
            // 
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(3, 121);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(34, 13);
            this.lblTags.TabIndex = 4;
            this.lblTags.Text = "Tags:";
            // 
            // grpMatureContent
            // 
            this.grpMatureContent.Controls.Add(this.radStrict);
            this.grpMatureContent.Controls.Add(this.radModerate);
            this.grpMatureContent.Controls.Add(this.radNone);
            this.grpMatureContent.Location = new System.Drawing.Point(3, 170);
            this.grpMatureContent.Name = "grpMatureContent";
            this.grpMatureContent.Size = new System.Drawing.Size(100, 132);
            this.grpMatureContent.TabIndex = 6;
            this.grpMatureContent.TabStop = false;
            this.grpMatureContent.Text = "Mature content";
            // 
            // radStrict
            // 
            this.radStrict.AutoSize = true;
            this.radStrict.Location = new System.Drawing.Point(6, 65);
            this.radStrict.Name = "radStrict";
            this.radStrict.Size = new System.Drawing.Size(49, 17);
            this.radStrict.TabIndex = 2;
            this.radStrict.Text = "Strict";
            this.radStrict.UseVisualStyleBackColor = true;
            // 
            // radModerate
            // 
            this.radModerate.AutoSize = true;
            this.radModerate.Location = new System.Drawing.Point(6, 42);
            this.radModerate.Name = "radModerate";
            this.radModerate.Size = new System.Drawing.Size(70, 17);
            this.radModerate.TabIndex = 1;
            this.radModerate.Text = "Moderate";
            this.radModerate.UseVisualStyleBackColor = true;
            // 
            // radNone
            // 
            this.radNone.AutoSize = true;
            this.radNone.Checked = true;
            this.radNone.Location = new System.Drawing.Point(6, 19);
            this.radNone.Name = "radNone";
            this.radNone.Size = new System.Drawing.Size(51, 17);
            this.radNone.TabIndex = 0;
            this.radNone.TabStop = true;
            this.radNone.Text = "None";
            this.radNone.UseVisualStyleBackColor = true;
            // 
            // grpMatureClassification
            // 
            this.grpMatureClassification.Controls.Add(this.chkIdeology);
            this.grpMatureClassification.Controls.Add(this.chkLanguage);
            this.grpMatureClassification.Controls.Add(this.chkGore);
            this.grpMatureClassification.Controls.Add(this.chkSexual);
            this.grpMatureClassification.Controls.Add(this.chkNudity);
            this.grpMatureClassification.Enabled = false;
            this.grpMatureClassification.Location = new System.Drawing.Point(109, 170);
            this.grpMatureClassification.Name = "grpMatureClassification";
            this.grpMatureClassification.Size = new System.Drawing.Size(150, 132);
            this.grpMatureClassification.TabIndex = 7;
            this.grpMatureClassification.TabStop = false;
            this.grpMatureClassification.Text = "Classification";
            // 
            // chkIdeology
            // 
            this.chkIdeology.AutoSize = true;
            this.chkIdeology.Location = new System.Drawing.Point(6, 111);
            this.chkIdeology.Name = "chkIdeology";
            this.chkIdeology.Size = new System.Drawing.Size(128, 17);
            this.chkIdeology.TabIndex = 4;
            this.chkIdeology.Text = "Ideologically sensitive";
            this.chkIdeology.UseVisualStyleBackColor = true;
            // 
            // chkLanguage
            // 
            this.chkLanguage.AutoSize = true;
            this.chkLanguage.Location = new System.Drawing.Point(6, 88);
            this.chkLanguage.Name = "chkLanguage";
            this.chkLanguage.Size = new System.Drawing.Size(104, 17);
            this.chkLanguage.TabIndex = 3;
            this.chkLanguage.Text = "Strong language";
            this.chkLanguage.UseVisualStyleBackColor = true;
            // 
            // chkGore
            // 
            this.chkGore.AutoSize = true;
            this.chkGore.Location = new System.Drawing.Point(6, 65);
            this.chkGore.Name = "chkGore";
            this.chkGore.Size = new System.Drawing.Size(93, 17);
            this.chkGore.TabIndex = 2;
            this.chkGore.Text = "Violence/gore";
            this.chkGore.UseVisualStyleBackColor = true;
            // 
            // chkSexual
            // 
            this.chkSexual.AutoSize = true;
            this.chkSexual.Location = new System.Drawing.Point(6, 42);
            this.chkSexual.Name = "chkSexual";
            this.chkSexual.Size = new System.Drawing.Size(95, 17);
            this.chkSexual.TabIndex = 1;
            this.chkSexual.Text = "Sexual themes";
            this.chkSexual.UseVisualStyleBackColor = true;
            // 
            // chkNudity
            // 
            this.chkNudity.AutoSize = true;
            this.chkNudity.Location = new System.Drawing.Point(6, 19);
            this.chkNudity.Name = "chkNudity";
            this.chkNudity.Size = new System.Drawing.Size(56, 17);
            this.chkNudity.TabIndex = 0;
            this.chkNudity.Text = "Nudity";
            this.chkNudity.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(3, 147);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 8;
            this.lblCategory.Text = "Category:";
            // 
            // txtCategory
            // 
            this.txtCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategory.Location = new System.Drawing.Point(61, 144);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(444, 20);
            this.txtCategory.TabIndex = 9;
            // 
            // btnCategory
            // 
            this.btnCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCategory.Location = new System.Drawing.Point(511, 144);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(26, 20);
            this.btnCategory.TabIndex = 10;
            this.btnCategory.Text = "...";
            this.btnCategory.UseVisualStyleBackColor = true;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // chkAllowComments
            // 
            this.chkAllowComments.AutoSize = true;
            this.chkAllowComments.Location = new System.Drawing.Point(265, 170);
            this.chkAllowComments.Name = "chkAllowComments";
            this.chkAllowComments.Size = new System.Drawing.Size(102, 17);
            this.chkAllowComments.TabIndex = 12;
            this.chkAllowComments.Text = "Allow comments";
            this.chkAllowComments.UseVisualStyleBackColor = true;
            // 
            // chkRequestCritique
            // 
            this.chkRequestCritique.AutoSize = true;
            this.chkRequestCritique.Location = new System.Drawing.Point(373, 170);
            this.chkRequestCritique.Name = "chkRequestCritique";
            this.chkRequestCritique.Size = new System.Drawing.Size(103, 17);
            this.chkRequestCritique.TabIndex = 13;
            this.chkRequestCritique.Text = "Request critique";
            this.chkRequestCritique.UseVisualStyleBackColor = true;
            // 
            // lblSharing
            // 
            this.lblSharing.AutoSize = true;
            this.lblSharing.Location = new System.Drawing.Point(265, 196);
            this.lblSharing.Name = "lblSharing";
            this.lblSharing.Size = new System.Drawing.Size(46, 13);
            this.lblSharing.TabIndex = 14;
            this.lblSharing.Text = "Sharing:";
            // 
            // ddlSharing
            // 
            this.ddlSharing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSharing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSharing.FormattingEnabled = true;
            this.ddlSharing.Items.AddRange(new object[] {
            "Show share buttons",
            "Hide share buttons",
            "Hide & require login to view"});
            this.ddlSharing.Location = new System.Drawing.Point(329, 193);
            this.ddlSharing.Name = "ddlSharing";
            this.ddlSharing.Size = new System.Drawing.Size(211, 21);
            this.ddlSharing.TabIndex = 15;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Location = new System.Drawing.Point(265, 223);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(47, 13);
            this.lblLicense.TabIndex = 16;
            this.lblLicense.Text = "License:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Default",
            "CC-BY",
            "CC-BY-SA",
            "CC-BY-ND",
            "CC-BY-NC",
            "CC-BY-NC-SA",
            "CC-BY-NC-ND"});
            this.comboBox1.Location = new System.Drawing.Point(329, 220);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(211, 21);
            this.comboBox1.TabIndex = 17;
            // 
            // btnGalleryFolders
            // 
            this.btnGalleryFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGalleryFolders.Location = new System.Drawing.Point(511, 247);
            this.btnGalleryFolders.Name = "btnGalleryFolders";
            this.btnGalleryFolders.Size = new System.Drawing.Size(26, 20);
            this.btnGalleryFolders.TabIndex = 20;
            this.btnGalleryFolders.Text = "...";
            this.btnGalleryFolders.UseVisualStyleBackColor = true;
            this.btnGalleryFolders.Click += new System.EventHandler(this.btnGalleryFolders_Click);
            // 
            // txtGalleryFolders
            // 
            this.txtGalleryFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGalleryFolders.Location = new System.Drawing.Point(350, 247);
            this.txtGalleryFolders.Name = "txtGalleryFolders";
            this.txtGalleryFolders.ReadOnly = true;
            this.txtGalleryFolders.Size = new System.Drawing.Size(155, 20);
            this.txtGalleryFolders.TabIndex = 19;
            // 
            // lblGalleryFolders
            // 
            this.lblGalleryFolders.AutoSize = true;
            this.lblGalleryFolders.Location = new System.Drawing.Point(265, 250);
            this.lblGalleryFolders.Name = "lblGalleryFolders";
            this.lblGalleryFolders.Size = new System.Drawing.Size(76, 13);
            this.lblGalleryFolders.TabIndex = 18;
            this.lblGalleryFolders.Text = "Gallery folders:";
            // 
            // chkAllowFreeDownload
            // 
            this.chkAllowFreeDownload.AutoSize = true;
            this.chkAllowFreeDownload.Location = new System.Drawing.Point(265, 273);
            this.chkAllowFreeDownload.Name = "chkAllowFreeDownload";
            this.chkAllowFreeDownload.Size = new System.Drawing.Size(100, 17);
            this.chkAllowFreeDownload.TabIndex = 21;
            this.chkAllowFreeDownload.Text = "Allow download";
            this.chkAllowFreeDownload.UseVisualStyleBackColor = true;
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPublish.AutoSize = true;
            this.btnPublish.Location = new System.Drawing.Point(421, 284);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(116, 23);
            this.btnPublish.TabIndex = 22;
            this.btnPublish.Text = "Publish to DeviantArt";
            this.btnPublish.UseVisualStyleBackColor = true;
            // 
            // DeviantArtUploadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.chkAllowFreeDownload);
            this.Controls.Add(this.btnGalleryFolders);
            this.Controls.Add(this.txtGalleryFolders);
            this.Controls.Add(this.lblGalleryFolders);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblLicense);
            this.Controls.Add(this.ddlSharing);
            this.Controls.Add(this.lblSharing);
            this.Controls.Add(this.chkRequestCritique);
            this.Controls.Add(this.chkAllowComments);
            this.Controls.Add(this.btnCategory);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.grpMatureClassification);
            this.Controls.Add(this.grpMatureContent);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.txtArtistComments);
            this.Controls.Add(this.lblArtistComments);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Name = "DeviantArtUploadControl";
            this.Size = new System.Drawing.Size(540, 310);
            this.grpMatureContent.ResumeLayout(false);
            this.grpMatureContent.PerformLayout();
            this.grpMatureClassification.ResumeLayout(false);
            this.grpMatureClassification.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Button btnCategory;
        private System.Windows.Forms.CheckBox chkAllowComments;
        private System.Windows.Forms.CheckBox chkRequestCritique;
        private System.Windows.Forms.Label lblSharing;
        private System.Windows.Forms.ComboBox ddlSharing;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnGalleryFolders;
        private System.Windows.Forms.TextBox txtGalleryFolders;
        private System.Windows.Forms.Label lblGalleryFolders;
        private System.Windows.Forms.CheckBox chkAllowFreeDownload;
        private System.Windows.Forms.Button btnPublish;
    }
}
