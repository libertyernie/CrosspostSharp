namespace CrosspostSharp3 {
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
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.chkContentWarning = new System.Windows.Forms.CheckBox();
            this.txtContentWarning = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.chkIncludeImage = new System.Windows.Forms.CheckBox();
            this.lblUsername2 = new System.Windows.Forms.Label();
            this.btnPost = new System.Windows.Forms.Button();
            this.chkImageSensitive = new System.Windows.Forms.CheckBox();
            this.txtImageDescription = new System.Windows.Forms.TextBox();
            this.chkFocalPoint = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(12, 12);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(48, 48);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 19;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername1
            // 
            this.lblUsername1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername1.Location = new System.Drawing.Point(66, 12);
            this.lblUsername1.Name = "lblUsername1";
            this.lblUsername1.Size = new System.Drawing.Size(264, 17);
            this.lblUsername1.TabIndex = 0;
            this.lblUsername1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkContentWarning
            // 
            this.chkContentWarning.AutoSize = true;
            this.chkContentWarning.Location = new System.Drawing.Point(12, 68);
            this.chkContentWarning.Name = "chkContentWarning";
            this.chkContentWarning.Size = new System.Drawing.Size(106, 17);
            this.chkContentWarning.TabIndex = 2;
            this.chkContentWarning.Text = "Content warning:";
            this.chkContentWarning.UseVisualStyleBackColor = true;
            this.chkContentWarning.CheckedChanged += new System.EventHandler(this.chkContentWarning_CheckedChanged);
            // 
            // txtContentWarning
            // 
            this.txtContentWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentWarning.Enabled = false;
            this.txtContentWarning.Location = new System.Drawing.Point(124, 66);
            this.txtContentWarning.Name = "txtContentWarning";
            this.txtContentWarning.Size = new System.Drawing.Size(206, 20);
            this.txtContentWarning.TabIndex = 3;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(9, 89);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(44, 13);
            this.lblContent.TabIndex = 4;
            this.lblContent.Text = "Content";
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Location = new System.Drawing.Point(12, 105);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(318, 101);
            this.txtContent.TabIndex = 5;
            // 
            // chkIncludeImage
            // 
            this.chkIncludeImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIncludeImage.AutoSize = true;
            this.chkIncludeImage.Location = new System.Drawing.Point(12, 214);
            this.chkIncludeImage.Name = "chkIncludeImage";
            this.chkIncludeImage.Size = new System.Drawing.Size(92, 17);
            this.chkIncludeImage.TabIndex = 6;
            this.chkIncludeImage.Text = "Include image";
            this.chkIncludeImage.UseVisualStyleBackColor = true;
            this.chkIncludeImage.CheckedChanged += new System.EventHandler(this.chkIncludeImage_CheckedChanged);
            // 
            // lblUsername2
            // 
            this.lblUsername2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername2.AutoEllipsis = true;
            this.lblUsername2.Location = new System.Drawing.Point(66, 29);
            this.lblUsername2.Name = "lblUsername2";
            this.lblUsername2.Size = new System.Drawing.Size(264, 13);
            this.lblUsername2.TabIndex = 1;
            this.lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(255, 238);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 9;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // chkImageSensitive
            // 
            this.chkImageSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkImageSensitive.AutoSize = true;
            this.chkImageSensitive.Location = new System.Drawing.Point(12, 242);
            this.chkImageSensitive.Name = "chkImageSensitive";
            this.chkImageSensitive.Size = new System.Drawing.Size(108, 17);
            this.chkImageSensitive.TabIndex = 8;
            this.chkImageSensitive.Text = "Mark as sensitive";
            this.chkImageSensitive.UseVisualStyleBackColor = true;
            // 
            // txtImageDescription
            // 
            this.txtImageDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageDescription.Enabled = false;
            this.txtImageDescription.Location = new System.Drawing.Point(110, 212);
            this.txtImageDescription.Name = "txtImageDescription";
            this.txtImageDescription.Size = new System.Drawing.Size(220, 20);
            this.txtImageDescription.TabIndex = 7;
            // 
            // chkFocalPoint
            // 
            this.chkFocalPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFocalPoint.AutoSize = true;
            this.chkFocalPoint.Location = new System.Drawing.Point(126, 242);
            this.chkFocalPoint.Name = "chkFocalPoint";
            this.chkFocalPoint.Size = new System.Drawing.Size(114, 17);
            this.chkFocalPoint.TabIndex = 20;
            this.chkFocalPoint.Text = "Choose focal point";
            this.chkFocalPoint.UseVisualStyleBackColor = true;
            // 
            // MastodonCwPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 273);
            this.Controls.Add(this.chkFocalPoint);
            this.Controls.Add(this.txtImageDescription);
            this.Controls.Add(this.chkImageSensitive);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.lblUsername2);
            this.Controls.Add(this.chkIncludeImage);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.txtContentWarning);
            this.Controls.Add(this.chkContentWarning);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Name = "MastodonCwPostForm";
            this.Text = "Post to Mastodon";
            this.Shown += new System.EventHandler(this.MastodonCwPostForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
	}
}