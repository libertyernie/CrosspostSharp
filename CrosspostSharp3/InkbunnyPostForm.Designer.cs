namespace CrosspostSharp3 {
	partial class InkbunnyPostForm {
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
            this.chkInkbunnyScraps = new System.Windows.Forms.CheckBox();
            this.chkInkbunnyPublic = new System.Windows.Forms.CheckBox();
            this.chkInkbunnyNotifyWatchers = new System.Windows.Forms.CheckBox();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.lblUsername2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
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
            this.txtDescription.Size = new System.Drawing.Size(360, 140);
            this.txtDescription.TabIndex = 5;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(297, 326);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 8;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // lblTags
            // 
            this.lblTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 261);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(31, 13);
            this.lblTags.TabIndex = 6;
            this.lblTags.Text = "Tags";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(12, 277);
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
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(12, 79);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(360, 20);
            this.txtTitle.TabIndex = 10;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 102);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(108, 13);
            this.lblDescription.TabIndex = 11;
            this.lblDescription.Text = "Description (BBCode)";
            // 
            // chkInkbunnyScraps
            // 
            this.chkInkbunnyScraps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInkbunnyScraps.AutoSize = true;
            this.chkInkbunnyScraps.Location = new System.Drawing.Point(12, 303);
            this.chkInkbunnyScraps.Name = "chkInkbunnyScraps";
            this.chkInkbunnyScraps.Size = new System.Drawing.Size(59, 17);
            this.chkInkbunnyScraps.TabIndex = 14;
            this.chkInkbunnyScraps.Text = "Scraps";
            this.chkInkbunnyScraps.UseVisualStyleBackColor = true;
            // 
            // chkInkbunnyPublic
            // 
            this.chkInkbunnyPublic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInkbunnyPublic.AutoSize = true;
            this.chkInkbunnyPublic.Checked = true;
            this.chkInkbunnyPublic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInkbunnyPublic.Location = new System.Drawing.Point(77, 303);
            this.chkInkbunnyPublic.Name = "chkInkbunnyPublic";
            this.chkInkbunnyPublic.Size = new System.Drawing.Size(55, 17);
            this.chkInkbunnyPublic.TabIndex = 15;
            this.chkInkbunnyPublic.Text = "Public";
            this.chkInkbunnyPublic.UseVisualStyleBackColor = true;
            this.chkInkbunnyPublic.CheckedChanged += new System.EventHandler(this.chkInkbunnyPublic_CheckedChanged);
            // 
            // chkInkbunnyNotifyWatchers
            // 
            this.chkInkbunnyNotifyWatchers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInkbunnyNotifyWatchers.AutoSize = true;
            this.chkInkbunnyNotifyWatchers.Checked = true;
            this.chkInkbunnyNotifyWatchers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInkbunnyNotifyWatchers.Location = new System.Drawing.Point(138, 303);
            this.chkInkbunnyNotifyWatchers.Name = "chkInkbunnyNotifyWatchers";
            this.chkInkbunnyNotifyWatchers.Size = new System.Drawing.Size(99, 17);
            this.chkInkbunnyNotifyWatchers.TabIndex = 16;
            this.chkInkbunnyNotifyWatchers.Text = "Notify watchers";
            this.chkInkbunnyNotifyWatchers.UseVisualStyleBackColor = true;
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
            this.lblUsername1.TabIndex = 18;
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
            this.lblUsername2.TabIndex = 19;
            this.lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InkbunnyPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Controls.Add(this.chkInkbunnyNotifyWatchers);
            this.Controls.Add(this.chkInkbunnyPublic);
            this.Controls.Add(this.chkInkbunnyScraps);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.txtDescription);
            this.Name = "InkbunnyPostForm";
            this.Text = "Post to Inkbunny";
            this.Shown += new System.EventHandler(this.Form_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
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
		private System.Windows.Forms.CheckBox chkInkbunnyScraps;
		private System.Windows.Forms.CheckBox chkInkbunnyPublic;
		private System.Windows.Forms.CheckBox chkInkbunnyNotifyWatchers;
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.Label lblUsername2;
	}
}