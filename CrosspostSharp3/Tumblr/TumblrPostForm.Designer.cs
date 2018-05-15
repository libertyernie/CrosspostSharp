namespace CrosspostSharp3 {
	partial class TumblrPostForm {
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
            this.lblUsername2 = new System.Windows.Forms.Label();
            this.chkIncludeTitle = new System.Windows.Forms.CheckBox();
            this.chkIncludeDescription = new System.Windows.Forms.CheckBox();
            this.chkIncludeLink = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnPost = new System.Windows.Forms.Button();
            this.lblTags = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.chkMakeSquare = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(12, 12);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(48, 48);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 17;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername1
            // 
            this.lblUsername1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername1.Location = new System.Drawing.Point(66, 12);
            this.lblUsername1.Name = "lblUsername1";
            this.lblUsername1.Size = new System.Drawing.Size(326, 17);
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
            this.lblUsername2.Size = new System.Drawing.Size(326, 13);
            this.lblUsername2.TabIndex = 1;
            this.lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIncludeTitle
            // 
            this.chkIncludeTitle.AutoSize = true;
            this.chkIncludeTitle.Checked = true;
            this.chkIncludeTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeTitle.Location = new System.Drawing.Point(12, 66);
            this.chkIncludeTitle.Name = "chkIncludeTitle";
            this.chkIncludeTitle.Size = new System.Drawing.Size(80, 17);
            this.chkIncludeTitle.TabIndex = 2;
            this.chkIncludeTitle.Text = "Include title";
            this.chkIncludeTitle.UseVisualStyleBackColor = true;
            // 
            // chkIncludeDescription
            // 
            this.chkIncludeDescription.AutoSize = true;
            this.chkIncludeDescription.Checked = true;
            this.chkIncludeDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeDescription.Location = new System.Drawing.Point(98, 66);
            this.chkIncludeDescription.Name = "chkIncludeDescription";
            this.chkIncludeDescription.Size = new System.Drawing.Size(115, 17);
            this.chkIncludeDescription.TabIndex = 3;
            this.chkIncludeDescription.Text = "Include description";
            this.chkIncludeDescription.UseVisualStyleBackColor = true;
            // 
            // chkIncludeLink
            // 
            this.chkIncludeLink.AutoSize = true;
            this.chkIncludeLink.Location = new System.Drawing.Point(219, 66);
            this.chkIncludeLink.Name = "chkIncludeLink";
            this.chkIncludeLink.Size = new System.Drawing.Size(80, 17);
            this.chkIncludeLink.TabIndex = 4;
            this.chkIncludeLink.Text = "Include link";
            this.chkIncludeLink.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 89);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(380, 81);
            this.textBox1.TabIndex = 6;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(317, 176);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 9;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // lblTags
            // 
            this.lblTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 181);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(31, 13);
            this.lblTags.TabIndex = 7;
            this.lblTags.Text = "Tags";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(49, 178);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(262, 20);
            this.txtTags.TabIndex = 8;
            // 
            // chkMakeSquare
            // 
            this.chkMakeSquare.AutoSize = true;
            this.chkMakeSquare.Location = new System.Drawing.Point(305, 66);
            this.chkMakeSquare.Name = "chkMakeSquare";
            this.chkMakeSquare.Size = new System.Drawing.Size(88, 17);
            this.chkMakeSquare.TabIndex = 5;
            this.chkMakeSquare.Text = "Make square";
            this.chkMakeSquare.UseVisualStyleBackColor = true;
            // 
            // TumblrPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 211);
            this.Controls.Add(this.chkMakeSquare);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkIncludeLink);
            this.Controls.Add(this.chkIncludeDescription);
            this.Controls.Add(this.chkIncludeTitle);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Name = "TumblrPostForm";
            this.Text = "Post to Tumblr";
            this.Shown += new System.EventHandler(this.TumblrPostForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.Label lblUsername2;
		private System.Windows.Forms.CheckBox chkIncludeTitle;
		private System.Windows.Forms.CheckBox chkIncludeDescription;
		private System.Windows.Forms.CheckBox chkIncludeLink;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.Label lblTags;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.CheckBox chkMakeSquare;
	}
}