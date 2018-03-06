namespace CrosspostSharp3 {
	partial class TwitterPostForm {
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
            this.chkPotentiallySensitive = new System.Windows.Forms.CheckBox();
            this.lblCounter = new System.Windows.Forms.Label();
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
            this.chkIncludeLink.Checked = true;
            this.chkIncludeLink.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.textBox1.Location = new System.Drawing.Point(12, 89);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(360, 81);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(297, 176);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 7;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // chkPotentiallySensitive
            // 
            this.chkPotentiallySensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPotentiallySensitive.AutoSize = true;
            this.chkPotentiallySensitive.Checked = true;
            this.chkPotentiallySensitive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPotentiallySensitive.Location = new System.Drawing.Point(12, 180);
            this.chkPotentiallySensitive.Name = "chkPotentiallySensitive";
            this.chkPotentiallySensitive.Size = new System.Drawing.Size(199, 17);
            this.chkPotentiallySensitive.TabIndex = 6;
            this.chkPotentiallySensitive.Text = "Includes potentially sensitive material";
            this.chkPotentiallySensitive.UseVisualStyleBackColor = true;
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(243, 181);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(48, 13);
            this.lblCounter.TabIndex = 18;
            this.lblCounter.Text = "280/280";
            // 
            // TwitterPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.chkPotentiallySensitive);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkIncludeLink);
            this.Controls.Add(this.chkIncludeDescription);
            this.Controls.Add(this.chkIncludeTitle);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Name = "TwitterPostForm";
            this.Text = "Post to Twitter";
            this.Shown += new System.EventHandler(this.TwitterPostForm_Shown);
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
		private System.Windows.Forms.CheckBox chkPotentiallySensitive;
		private System.Windows.Forms.Label lblCounter;
	}
}