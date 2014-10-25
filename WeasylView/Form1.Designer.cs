namespace WeasylView {
	partial class Form1 {
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
			this.thumbnail1 = new System.Windows.Forms.PictureBox();
			this.thumbnail2 = new System.Windows.Forms.PictureBox();
			this.thumbnail3 = new System.Windows.Forms.PictureBox();
			this.thumbnail4 = new System.Windows.Forms.PictureBox();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.mainPictureBox = new System.Windows.Forms.PictureBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.chkTitle = new System.Windows.Forms.CheckBox();
			this.chkDescription = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtTags = new System.Windows.Forms.TextBox();
			this.chkTags = new System.Windows.Forms.CheckBox();
			this.chkWeasylTag = new System.Windows.Forms.CheckBox();
			this.chkLink = new System.Windows.Forms.CheckBox();
			this.txtLink = new System.Windows.Forms.TextBox();
			this.btnEmail = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnail1
			// 
			this.thumbnail1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail1.Location = new System.Drawing.Point(12, 12);
			this.thumbnail1.Name = "thumbnail1";
			this.thumbnail1.Size = new System.Drawing.Size(120, 120);
			this.thumbnail1.TabIndex = 0;
			this.thumbnail1.TabStop = false;
			// 
			// thumbnail2
			// 
			this.thumbnail2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail2.Location = new System.Drawing.Point(12, 138);
			this.thumbnail2.Name = "thumbnail2";
			this.thumbnail2.Size = new System.Drawing.Size(120, 120);
			this.thumbnail2.TabIndex = 1;
			this.thumbnail2.TabStop = false;
			// 
			// thumbnail3
			// 
			this.thumbnail3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail3.Location = new System.Drawing.Point(12, 264);
			this.thumbnail3.Name = "thumbnail3";
			this.thumbnail3.Size = new System.Drawing.Size(120, 120);
			this.thumbnail3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail3.TabIndex = 2;
			this.thumbnail3.TabStop = false;
			// 
			// thumbnail4
			// 
			this.thumbnail4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail4.Location = new System.Drawing.Point(12, 390);
			this.thumbnail4.Name = "thumbnail4";
			this.thumbnail4.Size = new System.Drawing.Size(120, 120);
			this.thumbnail4.TabIndex = 3;
			this.thumbnail4.TabStop = false;
			// 
			// btnUp
			// 
			this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUp.Location = new System.Drawing.Point(100, 12);
			this.btnUp.Margin = new System.Windows.Forms.Padding(0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(32, 32);
			this.btnUp.TabIndex = 4;
			this.btnUp.Text = "↑";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDown.Location = new System.Drawing.Point(100, 478);
			this.btnDown.Margin = new System.Windows.Forms.Padding(0);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(32, 32);
			this.btnDown.TabIndex = 5;
			this.btnDown.Text = "↓";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(138, 12);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(360, 294);
			this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.mainPictureBox.TabIndex = 6;
			this.mainPictureBox.TabStop = false;
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Location = new System.Drawing.Point(166, 312);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(332, 22);
			this.txtTitle.TabIndex = 7;
			// 
			// chkTitle
			// 
			this.chkTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTitle.Checked = true;
			this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTitle.Location = new System.Drawing.Point(138, 312);
			this.chkTitle.Name = "chkTitle";
			this.chkTitle.Size = new System.Drawing.Size(22, 22);
			this.chkTitle.TabIndex = 8;
			this.chkTitle.UseVisualStyleBackColor = true;
			// 
			// chkDescription
			// 
			this.chkDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkDescription.Checked = true;
			this.chkDescription.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDescription.Location = new System.Drawing.Point(138, 340);
			this.chkDescription.Name = "chkDescription";
			this.chkDescription.Size = new System.Drawing.Size(22, 22);
			this.chkDescription.TabIndex = 10;
			this.chkDescription.UseVisualStyleBackColor = true;
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(166, 340);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(332, 88);
			this.txtDescription.TabIndex = 9;
			this.txtDescription.Text = "L1\r\nL2\r\nL3\r\nL4\r\nL5\r\nL6";
			// 
			// txtTags
			// 
			this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags.Location = new System.Drawing.Point(244, 462);
			this.txtTags.Name = "txtTags";
			this.txtTags.Size = new System.Drawing.Size(254, 22);
			this.txtTags.TabIndex = 11;
			// 
			// chkTags
			// 
			this.chkTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTags.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTags.Checked = true;
			this.chkTags.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags.Location = new System.Drawing.Point(216, 463);
			this.chkTags.Name = "chkTags";
			this.chkTags.Size = new System.Drawing.Size(22, 22);
			this.chkTags.TabIndex = 12;
			this.chkTags.UseVisualStyleBackColor = true;
			// 
			// chkWeasylTag
			// 
			this.chkWeasylTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkWeasylTag.AutoEllipsis = true;
			this.chkWeasylTag.Checked = true;
			this.chkWeasylTag.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWeasylTag.Location = new System.Drawing.Point(138, 463);
			this.chkWeasylTag.Name = "chkWeasylTag";
			this.chkWeasylTag.Size = new System.Drawing.Size(72, 22);
			this.chkWeasylTag.TabIndex = 13;
			this.chkWeasylTag.Text = "#weasyl";
			this.chkWeasylTag.UseVisualStyleBackColor = true;
			// 
			// chkLink
			// 
			this.chkLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkLink.Checked = true;
			this.chkLink.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLink.Location = new System.Drawing.Point(138, 434);
			this.chkLink.Name = "chkLink";
			this.chkLink.Size = new System.Drawing.Size(22, 22);
			this.chkLink.TabIndex = 16;
			this.chkLink.UseVisualStyleBackColor = true;
			// 
			// txtLink
			// 
			this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLink.ForeColor = System.Drawing.Color.Blue;
			this.txtLink.Location = new System.Drawing.Point(166, 434);
			this.txtLink.Name = "txtLink";
			this.txtLink.Size = new System.Drawing.Size(332, 22);
			this.txtLink.TabIndex = 15;
			this.txtLink.Text = "View on Weasyl";
			// 
			// btnEmail
			// 
			this.btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEmail.Location = new System.Drawing.Point(381, 490);
			this.btnEmail.Name = "btnEmail";
			this.btnEmail.Size = new System.Drawing.Size(117, 23);
			this.btnEmail.TabIndex = 17;
			this.btnEmail.Text = "Email to Tumblr";
			this.btnEmail.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(510, 525);
			this.Controls.Add(this.btnEmail);
			this.Controls.Add(this.chkLink);
			this.Controls.Add(this.txtLink);
			this.Controls.Add(this.chkWeasylTag);
			this.Controls.Add(this.chkTags);
			this.Controls.Add(this.txtTags);
			this.Controls.Add(this.chkDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.chkTitle);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.mainPictureBox);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.thumbnail4);
			this.Controls.Add(this.thumbnail3);
			this.Controls.Add(this.thumbnail2);
			this.Controls.Add(this.thumbnail1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox thumbnail1;
		private System.Windows.Forms.PictureBox thumbnail2;
		private System.Windows.Forms.PictureBox thumbnail3;
		private System.Windows.Forms.PictureBox thumbnail4;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.PictureBox mainPictureBox;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.CheckBox chkTitle;
		private System.Windows.Forms.CheckBox chkDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.CheckBox chkTags;
		private System.Windows.Forms.CheckBox chkWeasylTag;
		private System.Windows.Forms.CheckBox chkLink;
		private System.Windows.Forms.TextBox txtLink;
		private System.Windows.Forms.Button btnEmail;
	}
}

