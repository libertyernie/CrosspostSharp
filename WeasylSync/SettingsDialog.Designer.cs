namespace WeasylSync {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
			this.groupWeasyl = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtWeasylAPIKey = new System.Windows.Forms.TextBox();
			this.lblWeasylUsername = new System.Windows.Forms.Label();
			this.txtWeasylUsername = new System.Windows.Forms.TextBox();
			this.groupTumblr = new System.Windows.Forms.GroupBox();
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.lblTokenInfo = new System.Windows.Forms.Label();
			this.btnTumblrSignIn = new System.Windows.Forms.Button();
			this.menuSignIn = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemPrivate = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemIECookies = new System.Windows.Forms.ToolStripMenuItem();
			this.lblTokenStatus = new System.Windows.Forms.Label();
			this.lblToken = new System.Windows.Forms.Label();
			this.lblTags = new System.Windows.Forms.Label();
			this.txtTags = new System.Windows.Forms.TextBox();
			this.lblFooter = new System.Windows.Forms.Label();
			this.txtFooter = new System.Windows.Forms.TextBox();
			this.lblBlogName = new System.Windows.Forms.Label();
			this.txtBlogName = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupWeasyl.SuspendLayout();
			this.groupTumblr.SuspendLayout();
			this.menuSignIn.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupWeasyl
			// 
			this.groupWeasyl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupWeasyl.Controls.Add(this.label1);
			this.groupWeasyl.Controls.Add(this.txtWeasylAPIKey);
			this.groupWeasyl.Controls.Add(this.lblWeasylUsername);
			this.groupWeasyl.Controls.Add(this.txtWeasylUsername);
			this.groupWeasyl.Location = new System.Drawing.Point(12, 12);
			this.groupWeasyl.Name = "groupWeasyl";
			this.groupWeasyl.Size = new System.Drawing.Size(410, 71);
			this.groupWeasyl.TabIndex = 0;
			this.groupWeasyl.TabStop = false;
			this.groupWeasyl.Text = "Weasyl";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "API Key:";
			// 
			// txtWeasylAPIKey
			// 
			this.txtWeasylAPIKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtWeasylAPIKey.Location = new System.Drawing.Point(83, 45);
			this.txtWeasylAPIKey.Name = "txtWeasylAPIKey";
			this.txtWeasylAPIKey.Size = new System.Drawing.Size(321, 20);
			this.txtWeasylAPIKey.TabIndex = 2;
			// 
			// lblWeasylUsername
			// 
			this.lblWeasylUsername.AutoSize = true;
			this.lblWeasylUsername.Location = new System.Drawing.Point(6, 22);
			this.lblWeasylUsername.Name = "lblWeasylUsername";
			this.lblWeasylUsername.Size = new System.Drawing.Size(58, 13);
			this.lblWeasylUsername.TabIndex = 1;
			this.lblWeasylUsername.Text = "Username:";
			// 
			// txtWeasylUsername
			// 
			this.txtWeasylUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtWeasylUsername.Location = new System.Drawing.Point(83, 19);
			this.txtWeasylUsername.Name = "txtWeasylUsername";
			this.txtWeasylUsername.Size = new System.Drawing.Size(321, 20);
			this.txtWeasylUsername.TabIndex = 0;
			// 
			// groupTumblr
			// 
			this.groupTumblr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupTumblr.Controls.Add(this.lblHeader);
			this.groupTumblr.Controls.Add(this.txtHeader);
			this.groupTumblr.Controls.Add(this.lblTokenInfo);
			this.groupTumblr.Controls.Add(this.btnTumblrSignIn);
			this.groupTumblr.Controls.Add(this.lblTokenStatus);
			this.groupTumblr.Controls.Add(this.lblToken);
			this.groupTumblr.Controls.Add(this.lblTags);
			this.groupTumblr.Controls.Add(this.txtTags);
			this.groupTumblr.Controls.Add(this.lblFooter);
			this.groupTumblr.Controls.Add(this.txtFooter);
			this.groupTumblr.Controls.Add(this.lblBlogName);
			this.groupTumblr.Controls.Add(this.txtBlogName);
			this.groupTumblr.Location = new System.Drawing.Point(12, 89);
			this.groupTumblr.Name = "groupTumblr";
			this.groupTumblr.Size = new System.Drawing.Size(410, 213);
			this.groupTumblr.TabIndex = 4;
			this.groupTumblr.TabStop = false;
			this.groupTumblr.Text = "Tumblr";
			// 
			// lblHeader
			// 
			this.lblHeader.AutoSize = true;
			this.lblHeader.Location = new System.Drawing.Point(6, 48);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(45, 13);
			this.lblHeader.TabIndex = 13;
			this.lblHeader.Text = "Header:";
			// 
			// txtHeader
			// 
			this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeader.Location = new System.Drawing.Point(83, 45);
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.Size = new System.Drawing.Size(321, 20);
			this.txtHeader.TabIndex = 12;
			// 
			// lblTokenInfo
			// 
			this.lblTokenInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTokenInfo.Location = new System.Drawing.Point(80, 146);
			this.lblTokenInfo.Name = "lblTokenInfo";
			this.lblTokenInfo.Size = new System.Drawing.Size(324, 64);
			this.lblTokenInfo.TabIndex = 11;
			this.lblTokenInfo.Text = resources.GetString("lblTokenInfo.Text");
			// 
			// btnTumblrSignIn
			// 
			this.btnTumblrSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTumblrSignIn.Location = new System.Drawing.Point(329, 123);
			this.btnTumblrSignIn.Name = "btnTumblrSignIn";
			this.btnTumblrSignIn.Size = new System.Drawing.Size(75, 20);
			this.btnTumblrSignIn.TabIndex = 10;
			this.btnTumblrSignIn.Text = "Sign in";
			this.btnTumblrSignIn.UseVisualStyleBackColor = true;
			this.btnTumblrSignIn.Click += new System.EventHandler(this.btnTumblrSignIn_Click);
			// 
			// menuSignIn
			// 
			this.menuSignIn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPrivate,
            this.menuItemIECookies});
			this.menuSignIn.Name = "menuSignIn";
			this.menuSignIn.Size = new System.Drawing.Size(287, 48);
			// 
			// menuItemPrivate
			// 
			this.menuItemPrivate.Name = "menuItemPrivate";
			this.menuItemPrivate.Size = new System.Drawing.Size(286, 22);
			this.menuItemPrivate.Text = "Use private browser session (default)";
			this.menuItemPrivate.Click += new System.EventHandler(this.menuItemPrivate_Click);
			// 
			// menuItemIECookies
			// 
			this.menuItemIECookies.Name = "menuItemIECookies";
			this.menuItemIECookies.Size = new System.Drawing.Size(286, 22);
			this.menuItemIECookies.Text = "Use cookies/login from Internet Explorer";
			this.menuItemIECookies.Click += new System.EventHandler(this.menuItemIECookies_Click);
			// 
			// lblTokenStatus
			// 
			this.lblTokenStatus.AutoSize = true;
			this.lblTokenStatus.Location = new System.Drawing.Point(80, 127);
			this.lblTokenStatus.Name = "lblTokenStatus";
			this.lblTokenStatus.Size = new System.Drawing.Size(69, 13);
			this.lblTokenStatus.TabIndex = 9;
			this.lblTokenStatus.Text = "Not signed in";
			// 
			// lblToken
			// 
			this.lblToken.AutoSize = true;
			this.lblToken.Location = new System.Drawing.Point(6, 127);
			this.lblToken.Name = "lblToken";
			this.lblToken.Size = new System.Drawing.Size(41, 13);
			this.lblToken.TabIndex = 8;
			this.lblToken.Text = "Token:";
			// 
			// lblTags
			// 
			this.lblTags.AutoSize = true;
			this.lblTags.Location = new System.Drawing.Point(6, 100);
			this.lblTags.Name = "lblTags";
			this.lblTags.Size = new System.Drawing.Size(71, 13);
			this.lblTags.TabIndex = 6;
			this.lblTags.Text = "Default Tags:";
			// 
			// txtTags
			// 
			this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags.Location = new System.Drawing.Point(83, 97);
			this.txtTags.Name = "txtTags";
			this.txtTags.Size = new System.Drawing.Size(321, 20);
			this.txtTags.TabIndex = 5;
			// 
			// lblFooter
			// 
			this.lblFooter.AutoSize = true;
			this.lblFooter.Location = new System.Drawing.Point(6, 74);
			this.lblFooter.Name = "lblFooter";
			this.lblFooter.Size = new System.Drawing.Size(40, 13);
			this.lblFooter.TabIndex = 3;
			this.lblFooter.Text = "Footer:";
			// 
			// txtFooter
			// 
			this.txtFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFooter.Location = new System.Drawing.Point(83, 71);
			this.txtFooter.Name = "txtFooter";
			this.txtFooter.Size = new System.Drawing.Size(321, 20);
			this.txtFooter.TabIndex = 2;
			// 
			// lblBlogName
			// 
			this.lblBlogName.AutoSize = true;
			this.lblBlogName.Location = new System.Drawing.Point(6, 22);
			this.lblBlogName.Name = "lblBlogName";
			this.lblBlogName.Size = new System.Drawing.Size(31, 13);
			this.lblBlogName.TabIndex = 1;
			this.lblBlogName.Text = "Blog:";
			// 
			// txtBlogName
			// 
			this.txtBlogName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBlogName.Location = new System.Drawing.Point(83, 19);
			this.txtBlogName.Name = "txtBlogName";
			this.txtBlogName.Size = new System.Drawing.Size(321, 20);
			this.txtBlogName.TabIndex = 0;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Location = new System.Drawing.Point(266, 308);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(347, 308);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// SettingsDialog
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(434, 343);
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
			this.menuSignIn.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupWeasyl;
		private System.Windows.Forms.TextBox txtWeasylUsername;
		private System.Windows.Forms.Label lblWeasylUsername;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtWeasylAPIKey;
		private System.Windows.Forms.GroupBox groupTumblr;
		private System.Windows.Forms.Label lblFooter;
		private System.Windows.Forms.TextBox txtFooter;
		private System.Windows.Forms.Label lblBlogName;
		private System.Windows.Forms.TextBox txtBlogName;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Label lblToken;
		private System.Windows.Forms.Label lblTokenStatus;
		private System.Windows.Forms.Button btnTumblrSignIn;
		private System.Windows.Forms.Label lblTokenInfo;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.ContextMenuStrip menuSignIn;
		private System.Windows.Forms.ToolStripMenuItem menuItemPrivate;
		private System.Windows.Forms.ToolStripMenuItem menuItemIECookies;
	}
}