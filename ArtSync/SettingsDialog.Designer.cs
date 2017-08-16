namespace ArtSync {
	partial class SettingsDialog {
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
            this.groupWeasyl = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWeasylAPIKey = new System.Windows.Forms.TextBox();
            this.groupTumblr = new System.Windows.Forms.GroupBox();
            this.chkSidePadding = new System.Windows.Forms.CheckBox();
            this.btnTumblrSignIn = new System.Windows.Forms.Button();
            this.lblTokenStatus = new System.Windows.Forms.Label();
            this.lblToken = new System.Windows.Forms.Label();
            this.groupDefaults = new System.Windows.Forms.GroupBox();
            this.txtHeader = new System.Windows.Forms.TextBox();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTags = new System.Windows.Forms.Label();
            this.lblFooter = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtFooter = new System.Windows.Forms.TextBox();
            this.lblBlogName = new System.Windows.Forms.Label();
            this.txtBlogName = new System.Windows.Forms.TextBox();
            this.lblTokenInfo = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupInkbunnyDest = new System.Windows.Forms.GroupBox();
            this.btnInkbunnySignIn = new System.Windows.Forms.Button();
            this.lblInkbunnyToken = new System.Windows.Forms.Label();
            this.lblInkbunnyTokenStatus = new System.Windows.Forms.Label();
            this.groupTwitter = new System.Windows.Forms.GroupBox();
            this.btnTwitterSignIn = new System.Windows.Forms.Button();
            this.lblTwitterTokenStatus = new System.Windows.Forms.Label();
            this.lblTwitterToken = new System.Windows.Forms.Label();
            this.groupDeviantArt = new System.Windows.Forms.GroupBox();
            this.btnDeviantArtSignIn = new System.Windows.Forms.Button();
            this.lblDeviantArtTokenStatus = new System.Windows.Forms.Label();
            this.lblDeviantArtToken = new System.Windows.Forms.Label();
            this.chkIncludeGeneratedUniqueTag = new System.Windows.Forms.CheckBox();
            this.groupWeasyl.SuspendLayout();
            this.groupTumblr.SuspendLayout();
            this.groupDefaults.SuspendLayout();
            this.groupInkbunnyDest.SuspendLayout();
            this.groupTwitter.SuspendLayout();
            this.groupDeviantArt.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupWeasyl
            // 
            this.groupWeasyl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupWeasyl.Controls.Add(this.label1);
            this.groupWeasyl.Controls.Add(this.txtWeasylAPIKey);
            this.groupWeasyl.Location = new System.Drawing.Point(12, 12);
            this.groupWeasyl.Name = "groupWeasyl";
            this.groupWeasyl.Size = new System.Drawing.Size(410, 45);
            this.groupWeasyl.TabIndex = 1;
            this.groupWeasyl.TabStop = false;
            this.groupWeasyl.Text = "Weasyl";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "API Key:";
            // 
            // txtWeasylAPIKey
            // 
            this.txtWeasylAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWeasylAPIKey.Location = new System.Drawing.Point(83, 19);
            this.txtWeasylAPIKey.Name = "txtWeasylAPIKey";
            this.txtWeasylAPIKey.Size = new System.Drawing.Size(321, 20);
            this.txtWeasylAPIKey.TabIndex = 2;
            // 
            // groupTumblr
            // 
            this.groupTumblr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTumblr.Controls.Add(this.chkSidePadding);
            this.groupTumblr.Controls.Add(this.btnTumblrSignIn);
            this.groupTumblr.Controls.Add(this.lblTokenStatus);
            this.groupTumblr.Controls.Add(this.lblToken);
            this.groupTumblr.Controls.Add(this.groupDefaults);
            this.groupTumblr.Controls.Add(this.lblBlogName);
            this.groupTumblr.Controls.Add(this.txtBlogName);
            this.groupTumblr.Location = new System.Drawing.Point(12, 114);
            this.groupTumblr.Name = "groupTumblr";
            this.groupTumblr.Size = new System.Drawing.Size(410, 197);
            this.groupTumblr.TabIndex = 3;
            this.groupTumblr.TabStop = false;
            this.groupTumblr.Text = "Tumblr";
            // 
            // chkSidePadding
            // 
            this.chkSidePadding.AutoSize = true;
            this.chkSidePadding.Location = new System.Drawing.Point(9, 71);
            this.chkSidePadding.Name = "chkSidePadding";
            this.chkSidePadding.Size = new System.Drawing.Size(322, 17);
            this.chkSidePadding.TabIndex = 5;
            this.chkSidePadding.Text = "Add transparent padding to sides if image is taller than it is wide";
            this.chkSidePadding.UseVisualStyleBackColor = true;
            // 
            // btnTumblrSignIn
            // 
            this.btnTumblrSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTumblrSignIn.Location = new System.Drawing.Point(329, 19);
            this.btnTumblrSignIn.Name = "btnTumblrSignIn";
            this.btnTumblrSignIn.Size = new System.Drawing.Size(75, 20);
            this.btnTumblrSignIn.TabIndex = 2;
            this.btnTumblrSignIn.Text = "Sign in";
            this.btnTumblrSignIn.UseVisualStyleBackColor = true;
            this.btnTumblrSignIn.Click += new System.EventHandler(this.btnTumblrSignIn_Click);
            // 
            // lblTokenStatus
            // 
            this.lblTokenStatus.AutoSize = true;
            this.lblTokenStatus.Location = new System.Drawing.Point(80, 23);
            this.lblTokenStatus.Name = "lblTokenStatus";
            this.lblTokenStatus.Size = new System.Drawing.Size(69, 13);
            this.lblTokenStatus.TabIndex = 1;
            this.lblTokenStatus.Text = "Not signed in";
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(6, 23);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(41, 13);
            this.lblToken.TabIndex = 0;
            this.lblToken.Text = "Token:";
            // 
            // groupDefaults
            // 
            this.groupDefaults.Controls.Add(this.txtHeader);
            this.groupDefaults.Controls.Add(this.txtTags);
            this.groupDefaults.Controls.Add(this.lblTags);
            this.groupDefaults.Controls.Add(this.lblFooter);
            this.groupDefaults.Controls.Add(this.lblHeader);
            this.groupDefaults.Controls.Add(this.txtFooter);
            this.groupDefaults.Location = new System.Drawing.Point(9, 94);
            this.groupDefaults.Name = "groupDefaults";
            this.groupDefaults.Size = new System.Drawing.Size(392, 97);
            this.groupDefaults.TabIndex = 0;
            this.groupDefaults.TabStop = false;
            this.groupDefaults.Text = "Defaults";
            // 
            // txtHeader
            // 
            this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHeader.Location = new System.Drawing.Point(83, 19);
            this.txtHeader.Name = "txtHeader";
            this.txtHeader.Size = new System.Drawing.Size(303, 20);
            this.txtHeader.TabIndex = 1;
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(83, 71);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(303, 20);
            this.txtTags.TabIndex = 5;
            // 
            // lblTags
            // 
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(6, 74);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(71, 13);
            this.lblTags.TabIndex = 4;
            this.lblTags.Text = "Default Tags:";
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Location = new System.Drawing.Point(6, 48);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(40, 13);
            this.lblFooter.TabIndex = 2;
            this.lblFooter.Text = "Footer:";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(6, 22);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(45, 13);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Header:";
            // 
            // txtFooter
            // 
            this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFooter.Location = new System.Drawing.Point(83, 45);
            this.txtFooter.Name = "txtFooter";
            this.txtFooter.Size = new System.Drawing.Size(303, 20);
            this.txtFooter.TabIndex = 3;
            // 
            // lblBlogName
            // 
            this.lblBlogName.AutoSize = true;
            this.lblBlogName.Location = new System.Drawing.Point(6, 48);
            this.lblBlogName.Name = "lblBlogName";
            this.lblBlogName.Size = new System.Drawing.Size(60, 13);
            this.lblBlogName.TabIndex = 3;
            this.lblBlogName.Text = "Blog name:";
            // 
            // txtBlogName
            // 
            this.txtBlogName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBlogName.Location = new System.Drawing.Point(83, 45);
            this.txtBlogName.Name = "txtBlogName";
            this.txtBlogName.Size = new System.Drawing.Size(318, 20);
            this.txtBlogName.TabIndex = 4;
            // 
            // lblTokenInfo
            // 
            this.lblTokenInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTokenInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTokenInfo.Location = new System.Drawing.Point(12, 454);
            this.lblTokenInfo.Name = "lblTokenInfo";
            this.lblTokenInfo.Size = new System.Drawing.Size(410, 44);
            this.lblTokenInfo.TabIndex = 6;
            this.lblTokenInfo.Text = "This application will not see your passwords, but it will have access to your acc" +
    "ounts. The security tokens are stored in the file ArtSync.json, so make sure you" +
    " keep that file secure.";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(266, 501);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 501);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupInkbunnyDest
            // 
            this.groupInkbunnyDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInkbunnyDest.Controls.Add(this.btnInkbunnySignIn);
            this.groupInkbunnyDest.Controls.Add(this.lblInkbunnyToken);
            this.groupInkbunnyDest.Controls.Add(this.lblInkbunnyTokenStatus);
            this.groupInkbunnyDest.Location = new System.Drawing.Point(12, 317);
            this.groupInkbunnyDest.Name = "groupInkbunnyDest";
            this.groupInkbunnyDest.Size = new System.Drawing.Size(410, 45);
            this.groupInkbunnyDest.TabIndex = 4;
            this.groupInkbunnyDest.TabStop = false;
            this.groupInkbunnyDest.Text = "Inkbunny";
            // 
            // btnInkbunnySignIn
            // 
            this.btnInkbunnySignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInkbunnySignIn.Location = new System.Drawing.Point(329, 19);
            this.btnInkbunnySignIn.Name = "btnInkbunnySignIn";
            this.btnInkbunnySignIn.Size = new System.Drawing.Size(75, 20);
            this.btnInkbunnySignIn.TabIndex = 8;
            this.btnInkbunnySignIn.Text = "Sign in";
            this.btnInkbunnySignIn.UseVisualStyleBackColor = true;
            this.btnInkbunnySignIn.Click += new System.EventHandler(this.btnInkbunnySignIn_Click);
            // 
            // lblInkbunnyToken
            // 
            this.lblInkbunnyToken.AutoSize = true;
            this.lblInkbunnyToken.Location = new System.Drawing.Point(6, 23);
            this.lblInkbunnyToken.Name = "lblInkbunnyToken";
            this.lblInkbunnyToken.Size = new System.Drawing.Size(41, 13);
            this.lblInkbunnyToken.TabIndex = 6;
            this.lblInkbunnyToken.Text = "Token:";
            // 
            // lblInkbunnyTokenStatus
            // 
            this.lblInkbunnyTokenStatus.AutoEllipsis = true;
            this.lblInkbunnyTokenStatus.Location = new System.Drawing.Point(80, 23);
            this.lblInkbunnyTokenStatus.Name = "lblInkbunnyTokenStatus";
            this.lblInkbunnyTokenStatus.Size = new System.Drawing.Size(243, 13);
            this.lblInkbunnyTokenStatus.TabIndex = 7;
            this.lblInkbunnyTokenStatus.Text = "Not signed in";
            // 
            // groupTwitter
            // 
            this.groupTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTwitter.Controls.Add(this.btnTwitterSignIn);
            this.groupTwitter.Controls.Add(this.lblTwitterTokenStatus);
            this.groupTwitter.Controls.Add(this.lblTwitterToken);
            this.groupTwitter.Location = new System.Drawing.Point(12, 368);
            this.groupTwitter.Name = "groupTwitter";
            this.groupTwitter.Size = new System.Drawing.Size(410, 45);
            this.groupTwitter.TabIndex = 5;
            this.groupTwitter.TabStop = false;
            this.groupTwitter.Text = "Twitter";
            // 
            // btnTwitterSignIn
            // 
            this.btnTwitterSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTwitterSignIn.Location = new System.Drawing.Point(329, 19);
            this.btnTwitterSignIn.Name = "btnTwitterSignIn";
            this.btnTwitterSignIn.Size = new System.Drawing.Size(75, 20);
            this.btnTwitterSignIn.TabIndex = 2;
            this.btnTwitterSignIn.Text = "Sign in";
            this.btnTwitterSignIn.UseVisualStyleBackColor = true;
            this.btnTwitterSignIn.Click += new System.EventHandler(this.btnTwitterSignIn_Click);
            // 
            // lblTwitterTokenStatus
            // 
            this.lblTwitterTokenStatus.AutoSize = true;
            this.lblTwitterTokenStatus.Location = new System.Drawing.Point(80, 23);
            this.lblTwitterTokenStatus.Name = "lblTwitterTokenStatus";
            this.lblTwitterTokenStatus.Size = new System.Drawing.Size(69, 13);
            this.lblTwitterTokenStatus.TabIndex = 1;
            this.lblTwitterTokenStatus.Text = "Not signed in";
            // 
            // lblTwitterToken
            // 
            this.lblTwitterToken.AutoSize = true;
            this.lblTwitterToken.Location = new System.Drawing.Point(6, 23);
            this.lblTwitterToken.Name = "lblTwitterToken";
            this.lblTwitterToken.Size = new System.Drawing.Size(41, 13);
            this.lblTwitterToken.TabIndex = 0;
            this.lblTwitterToken.Text = "Token:";
            // 
            // groupDeviantArt
            // 
            this.groupDeviantArt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDeviantArt.Controls.Add(this.btnDeviantArtSignIn);
            this.groupDeviantArt.Controls.Add(this.lblDeviantArtTokenStatus);
            this.groupDeviantArt.Controls.Add(this.lblDeviantArtToken);
            this.groupDeviantArt.Location = new System.Drawing.Point(12, 63);
            this.groupDeviantArt.Name = "groupDeviantArt";
            this.groupDeviantArt.Size = new System.Drawing.Size(410, 45);
            this.groupDeviantArt.TabIndex = 2;
            this.groupDeviantArt.TabStop = false;
            this.groupDeviantArt.Text = "DeviantArt";
            // 
            // btnDeviantArtSignIn
            // 
            this.btnDeviantArtSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeviantArtSignIn.Location = new System.Drawing.Point(329, 19);
            this.btnDeviantArtSignIn.Name = "btnDeviantArtSignIn";
            this.btnDeviantArtSignIn.Size = new System.Drawing.Size(75, 20);
            this.btnDeviantArtSignIn.TabIndex = 6;
            this.btnDeviantArtSignIn.Text = "Sign in";
            this.btnDeviantArtSignIn.UseVisualStyleBackColor = true;
            this.btnDeviantArtSignIn.Click += new System.EventHandler(this.btnDeviantArtSignIn_Click);
            // 
            // lblDeviantArtTokenStatus
            // 
            this.lblDeviantArtTokenStatus.AutoSize = true;
            this.lblDeviantArtTokenStatus.Location = new System.Drawing.Point(80, 23);
            this.lblDeviantArtTokenStatus.Name = "lblDeviantArtTokenStatus";
            this.lblDeviantArtTokenStatus.Size = new System.Drawing.Size(69, 13);
            this.lblDeviantArtTokenStatus.TabIndex = 5;
            this.lblDeviantArtTokenStatus.Text = "Not signed in";
            // 
            // lblDeviantArtToken
            // 
            this.lblDeviantArtToken.AutoSize = true;
            this.lblDeviantArtToken.Location = new System.Drawing.Point(6, 23);
            this.lblDeviantArtToken.Name = "lblDeviantArtToken";
            this.lblDeviantArtToken.Size = new System.Drawing.Size(41, 13);
            this.lblDeviantArtToken.TabIndex = 4;
            this.lblDeviantArtToken.Text = "Token:";
            // 
            // chkIncludeGeneratedUniqueTag
            // 
            this.chkIncludeGeneratedUniqueTag.Location = new System.Drawing.Point(12, 419);
            this.chkIncludeGeneratedUniqueTag.Name = "chkIncludeGeneratedUniqueTag";
            this.chkIncludeGeneratedUniqueTag.Size = new System.Drawing.Size(410, 32);
            this.chkIncludeGeneratedUniqueTag.TabIndex = 9;
            this.chkIncludeGeneratedUniqueTag.Text = "Include generated unique tag (e.g. #weasyl1234567) in Tumblr, DeviantArt, and Ink" +
    "bunny posts so ArtSync can find them later";
            this.chkIncludeGeneratedUniqueTag.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 536);
            this.Controls.Add(this.chkIncludeGeneratedUniqueTag);
            this.Controls.Add(this.groupDeviantArt);
            this.Controls.Add(this.groupTwitter);
            this.Controls.Add(this.lblTokenInfo);
            this.Controls.Add(this.groupInkbunnyDest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupTumblr);
            this.Controls.Add(this.groupWeasyl);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.groupWeasyl.ResumeLayout(false);
            this.groupWeasyl.PerformLayout();
            this.groupTumblr.ResumeLayout(false);
            this.groupTumblr.PerformLayout();
            this.groupDefaults.ResumeLayout(false);
            this.groupDefaults.PerformLayout();
            this.groupInkbunnyDest.ResumeLayout(false);
            this.groupInkbunnyDest.PerformLayout();
            this.groupTwitter.ResumeLayout(false);
            this.groupTwitter.PerformLayout();
            this.groupDeviantArt.ResumeLayout(false);
            this.groupDeviantArt.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupWeasyl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtWeasylAPIKey;
		private System.Windows.Forms.GroupBox groupTumblr;
		private System.Windows.Forms.Label lblBlogName;
		private System.Windows.Forms.TextBox txtBlogName;
		private System.Windows.Forms.Label lblToken;
		private System.Windows.Forms.Label lblTokenStatus;
		private System.Windows.Forms.Button btnTumblrSignIn;
		private System.Windows.Forms.Label lblTokenInfo;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkSidePadding;
		private System.Windows.Forms.GroupBox groupInkbunnyDest;
		private System.Windows.Forms.GroupBox groupTwitter;
		private System.Windows.Forms.Button btnTwitterSignIn;
		private System.Windows.Forms.Label lblTwitterTokenStatus;
		private System.Windows.Forms.Label lblTwitterToken;
        private System.Windows.Forms.GroupBox groupDeviantArt;
        private System.Windows.Forms.Button btnDeviantArtSignIn;
        private System.Windows.Forms.Label lblDeviantArtTokenStatus;
        private System.Windows.Forms.Label lblDeviantArtToken;
        private System.Windows.Forms.Button btnInkbunnySignIn;
        private System.Windows.Forms.Label lblInkbunnyToken;
        private System.Windows.Forms.Label lblInkbunnyTokenStatus;
        private System.Windows.Forms.GroupBox groupDefaults;
        private System.Windows.Forms.TextBox txtHeader;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label lblTags;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtFooter;
        private System.Windows.Forms.CheckBox chkIncludeGeneratedUniqueTag;
    }
}