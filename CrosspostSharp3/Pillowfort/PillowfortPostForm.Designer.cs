namespace CrosspostSharp3 {
	partial class PillowfortPostForm {
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
            this.chkMakeSquare = new System.Windows.Forms.CheckBox();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.chkIncludeImage = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTags = new System.Windows.Forms.Label();
            this.btnPost = new System.Windows.Forms.Button();
            this.chkRebloggable = new System.Windows.Forms.CheckBox();
            this.chkCommentable = new System.Windows.Forms.CheckBox();
            this.chkNsfw = new System.Windows.Forms.CheckBox();
            this.ddlPrivacy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMakeSquare
            // 
            this.chkMakeSquare.AutoSize = true;
            this.chkMakeSquare.Location = new System.Drawing.Point(302, 155);
            this.chkMakeSquare.Name = "chkMakeSquare";
            this.chkMakeSquare.Size = new System.Drawing.Size(88, 17);
            this.chkMakeSquare.TabIndex = 6;
            this.chkMakeSquare.Text = "Make square";
            this.chkMakeSquare.UseVisualStyleBackColor = true;
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
            this.lblUsername1.Size = new System.Drawing.Size(356, 17);
            this.lblUsername1.TabIndex = 0;
            this.lblUsername1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIncludeImage
            // 
            this.chkIncludeImage.AutoSize = true;
            this.chkIncludeImage.Location = new System.Drawing.Point(62, 155);
            this.chkIncludeImage.Name = "chkIncludeImage";
            this.chkIncludeImage.Size = new System.Drawing.Size(234, 17);
            this.chkIncludeImage.TabIndex = 5;
            this.chkIncludeImage.Text = "Include image (file will be uploaded to Imgur)";
            this.chkIncludeImage.UseVisualStyleBackColor = true;
            this.chkIncludeImage.CheckedChanged += new System.EventHandler(this.chkIncludeImage_CheckedChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 69);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(62, 66);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(360, 20);
            this.txtTitle.TabIndex = 2;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(12, 95);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(44, 26);
            this.lblContent.TabIndex = 3;
            this.lblContent.Text = "Content\r\n(HTML)";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(62, 92);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(360, 57);
            this.txtDescription.TabIndex = 4;
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(62, 228);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(279, 20);
            this.txtTags.TabIndex = 13;
            // 
            // lblTags
            // 
            this.lblTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 231);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(31, 13);
            this.lblTags.TabIndex = 12;
            this.lblTags.Text = "Tags";
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(347, 226);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 14;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // chkRebloggable
            // 
            this.chkRebloggable.AutoSize = true;
            this.chkRebloggable.Checked = true;
            this.chkRebloggable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRebloggable.Location = new System.Drawing.Point(62, 178);
            this.chkRebloggable.Name = "chkRebloggable";
            this.chkRebloggable.Size = new System.Drawing.Size(88, 17);
            this.chkRebloggable.TabIndex = 7;
            this.chkRebloggable.Text = "Allow reblogs";
            this.chkRebloggable.UseVisualStyleBackColor = true;
            // 
            // chkCommentable
            // 
            this.chkCommentable.AutoSize = true;
            this.chkCommentable.Checked = true;
            this.chkCommentable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommentable.Location = new System.Drawing.Point(156, 178);
            this.chkCommentable.Name = "chkCommentable";
            this.chkCommentable.Size = new System.Drawing.Size(102, 17);
            this.chkCommentable.TabIndex = 8;
            this.chkCommentable.Text = "Allow comments";
            this.chkCommentable.UseVisualStyleBackColor = true;
            // 
            // chkNsfw
            // 
            this.chkNsfw.AutoSize = true;
            this.chkNsfw.Location = new System.Drawing.Point(264, 178);
            this.chkNsfw.Name = "chkNsfw";
            this.chkNsfw.Size = new System.Drawing.Size(58, 17);
            this.chkNsfw.TabIndex = 9;
            this.chkNsfw.Text = "NSFW";
            this.chkNsfw.UseVisualStyleBackColor = true;
            // 
            // ddlPrivacy
            // 
            this.ddlPrivacy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ddlPrivacy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPrivacy.FormattingEnabled = true;
            this.ddlPrivacy.Location = new System.Drawing.Point(62, 201);
            this.ddlPrivacy.Name = "ddlPrivacy";
            this.ddlPrivacy.Size = new System.Drawing.Size(121, 21);
            this.ddlPrivacy.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Visibility";
            // 
            // PillowfortPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlPrivacy);
            this.Controls.Add(this.chkNsfw);
            this.Controls.Add(this.chkRebloggable);
            this.Controls.Add(this.chkCommentable);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.chkIncludeImage);
            this.Controls.Add(this.chkMakeSquare);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Name = "PillowfortPostForm";
            this.Text = "Post to Pillowfort";
            this.Shown += new System.EventHandler(this.PillowfortPostForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkMakeSquare;
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.CheckBox chkIncludeImage;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblContent;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.CheckBox chkRebloggable;
		private System.Windows.Forms.CheckBox chkCommentable;
		private System.Windows.Forms.CheckBox chkNsfw;
		private System.Windows.Forms.ComboBox ddlPrivacy;
		private System.Windows.Forms.Label label1;
	}
}