namespace CrosspostSharp3.Mastodon {
	partial class MastodonCwPostForm {
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
			picUserIcon = new System.Windows.Forms.PictureBox();
			lblUsername1 = new System.Windows.Forms.Label();
			chkContentWarning = new System.Windows.Forms.CheckBox();
			txtContentWarning = new System.Windows.Forms.TextBox();
			lblContent = new System.Windows.Forms.Label();
			txtContent = new System.Windows.Forms.TextBox();
			chkIncludeImage = new System.Windows.Forms.CheckBox();
			lblUsername2 = new System.Windows.Forms.Label();
			btnPost = new System.Windows.Forms.Button();
			chkImageSensitive = new System.Windows.Forms.CheckBox();
			txtImageDescription = new System.Windows.Forms.TextBox();
			chkFocalPoint = new System.Windows.Forms.CheckBox();
			lblCollections = new System.Windows.Forms.Label();
			listBox1 = new System.Windows.Forms.ListBox();
			chkRemoveTransparency = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)picUserIcon).BeginInit();
			SuspendLayout();
			// 
			// picUserIcon
			// 
			picUserIcon.Location = new System.Drawing.Point(14, 14);
			picUserIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			picUserIcon.Name = "picUserIcon";
			picUserIcon.Size = new System.Drawing.Size(56, 55);
			picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			picUserIcon.TabIndex = 19;
			picUserIcon.TabStop = false;
			// 
			// lblUsername1
			// 
			lblUsername1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblUsername1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			lblUsername1.Location = new System.Drawing.Point(77, 14);
			lblUsername1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblUsername1.Name = "lblUsername1";
			lblUsername1.Size = new System.Drawing.Size(443, 20);
			lblUsername1.TabIndex = 0;
			lblUsername1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkContentWarning
			// 
			chkContentWarning.AutoSize = true;
			chkContentWarning.Location = new System.Drawing.Point(14, 78);
			chkContentWarning.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkContentWarning.Name = "chkContentWarning";
			chkContentWarning.Size = new System.Drawing.Size(118, 19);
			chkContentWarning.TabIndex = 2;
			chkContentWarning.Text = "Content warning:";
			chkContentWarning.UseVisualStyleBackColor = true;
			chkContentWarning.CheckedChanged += chkContentWarning_CheckedChanged;
			// 
			// txtContentWarning
			// 
			txtContentWarning.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtContentWarning.Enabled = false;
			txtContentWarning.Location = new System.Drawing.Point(145, 76);
			txtContentWarning.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtContentWarning.Name = "txtContentWarning";
			txtContentWarning.Size = new System.Drawing.Size(375, 23);
			txtContentWarning.TabIndex = 3;
			// 
			// lblContent
			// 
			lblContent.AutoSize = true;
			lblContent.Location = new System.Drawing.Point(10, 103);
			lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblContent.Name = "lblContent";
			lblContent.Size = new System.Drawing.Size(50, 15);
			lblContent.TabIndex = 4;
			lblContent.Text = "Content";
			// 
			// txtContent
			// 
			txtContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtContent.Location = new System.Drawing.Point(14, 121);
			txtContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtContent.Multiline = true;
			txtContent.Name = "txtContent";
			txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			txtContent.Size = new System.Drawing.Size(394, 116);
			txtContent.TabIndex = 5;
			// 
			// chkIncludeImage
			// 
			chkIncludeImage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkIncludeImage.AutoSize = true;
			chkIncludeImage.Location = new System.Drawing.Point(14, 248);
			chkIncludeImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkIncludeImage.Name = "chkIncludeImage";
			chkIncludeImage.Size = new System.Drawing.Size(101, 19);
			chkIncludeImage.TabIndex = 8;
			chkIncludeImage.Text = "Include image";
			chkIncludeImage.UseVisualStyleBackColor = true;
			chkIncludeImage.CheckedChanged += chkIncludeImage_CheckedChanged;
			// 
			// lblUsername2
			// 
			lblUsername2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblUsername2.AutoEllipsis = true;
			lblUsername2.Location = new System.Drawing.Point(77, 33);
			lblUsername2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblUsername2.Name = "lblUsername2";
			lblUsername2.Size = new System.Drawing.Size(443, 15);
			lblUsername2.TabIndex = 1;
			lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnPost
			// 
			btnPost.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnPost.Location = new System.Drawing.Point(433, 275);
			btnPost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnPost.Name = "btnPost";
			btnPost.Size = new System.Drawing.Size(88, 27);
			btnPost.TabIndex = 12;
			btnPost.Text = "Post";
			btnPost.UseVisualStyleBackColor = true;
			btnPost.Click += btnPost_Click;
			// 
			// chkImageSensitive
			// 
			chkImageSensitive.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkImageSensitive.AutoSize = true;
			chkImageSensitive.Location = new System.Drawing.Point(14, 280);
			chkImageSensitive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkImageSensitive.Name = "chkImageSensitive";
			chkImageSensitive.Size = new System.Drawing.Size(115, 19);
			chkImageSensitive.TabIndex = 10;
			chkImageSensitive.Text = "Mark as sensitive";
			chkImageSensitive.UseVisualStyleBackColor = true;
			// 
			// txtImageDescription
			// 
			txtImageDescription.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtImageDescription.Enabled = false;
			txtImageDescription.Location = new System.Drawing.Point(128, 245);
			txtImageDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			txtImageDescription.Name = "txtImageDescription";
			txtImageDescription.Size = new System.Drawing.Size(391, 23);
			txtImageDescription.TabIndex = 9;
			// 
			// chkFocalPoint
			// 
			chkFocalPoint.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkFocalPoint.AutoSize = true;
			chkFocalPoint.Checked = true;
			chkFocalPoint.CheckState = System.Windows.Forms.CheckState.Checked;
			chkFocalPoint.Enabled = false;
			chkFocalPoint.Location = new System.Drawing.Point(137, 280);
			chkFocalPoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkFocalPoint.Name = "chkFocalPoint";
			chkFocalPoint.Size = new System.Drawing.Size(126, 19);
			chkFocalPoint.TabIndex = 11;
			chkFocalPoint.Text = "Choose focal point";
			chkFocalPoint.UseVisualStyleBackColor = true;
			// 
			// lblCollections
			// 
			lblCollections.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lblCollections.AutoSize = true;
			lblCollections.Location = new System.Drawing.Point(415, 103);
			lblCollections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lblCollections.Name = "lblCollections";
			lblCollections.Size = new System.Drawing.Size(66, 15);
			lblCollections.TabIndex = 6;
			lblCollections.Text = "Collections";
			// 
			// listBox1
			// 
			listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			listBox1.DisplayMember = "Title";
			listBox1.FormattingEnabled = true;
			listBox1.IntegralHeight = false;
			listBox1.ItemHeight = 15;
			listBox1.Location = new System.Drawing.Point(415, 121);
			listBox1.Name = "listBox1";
			listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			listBox1.Size = new System.Drawing.Size(104, 116);
			listBox1.TabIndex = 7;
			// 
			// chkRemoveTransparency
			// 
			chkRemoveTransparency.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkRemoveTransparency.AutoSize = true;
			chkRemoveTransparency.Location = new System.Drawing.Point(271, 280);
			chkRemoveTransparency.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			chkRemoveTransparency.Name = "chkRemoveTransparency";
			chkRemoveTransparency.Size = new System.Drawing.Size(140, 19);
			chkRemoveTransparency.TabIndex = 22;
			chkRemoveTransparency.Text = "Remove transparency";
			chkRemoveTransparency.UseVisualStyleBackColor = true;
			// 
			// MastodonCwPostForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(534, 315);
			Controls.Add(chkRemoveTransparency);
			Controls.Add(listBox1);
			Controls.Add(lblCollections);
			Controls.Add(chkFocalPoint);
			Controls.Add(txtImageDescription);
			Controls.Add(chkImageSensitive);
			Controls.Add(btnPost);
			Controls.Add(lblUsername2);
			Controls.Add(chkIncludeImage);
			Controls.Add(txtContent);
			Controls.Add(lblContent);
			Controls.Add(txtContentWarning);
			Controls.Add(chkContentWarning);
			Controls.Add(picUserIcon);
			Controls.Add(lblUsername1);
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "MastodonCwPostForm";
			Text = "Post to Mastodon / Pixelfed";
			Shown += MastodonCwPostForm_Shown;
			((System.ComponentModel.ISupportInitialize)picUserIcon).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.CheckBox chkContentWarning;
		private System.Windows.Forms.TextBox txtContentWarning;
		private System.Windows.Forms.Label lblContent;
		private System.Windows.Forms.TextBox txtContent;
		private System.Windows.Forms.CheckBox chkIncludeImage;
		private System.Windows.Forms.Label lblUsername2;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.CheckBox chkImageSensitive;
		private System.Windows.Forms.TextBox txtImageDescription;
		private System.Windows.Forms.CheckBox chkFocalPoint;
		private System.Windows.Forms.Label lblCollections;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.CheckBox chkRemoveTransparency;
	}
}