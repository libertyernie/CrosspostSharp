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
			this.chkTitle = new System.Windows.Forms.CheckBox();
			this.chkDescription = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtTags = new System.Windows.Forms.TextBox();
			this.chkTags = new System.Windows.Forms.CheckBox();
			this.chkWeasylTag = new System.Windows.Forms.CheckBox();
			this.chkLink = new System.Windows.Forms.CheckBox();
			this.txtLink = new System.Windows.Forms.TextBox();
			this.btnEmail = new System.Windows.Forms.Button();
			this.chkTitleBold = new System.Windows.Forms.CheckBox();
			this.txtTitleSize = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblPt = new System.Windows.Forms.Label();
			this.lblLink = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			this.panel1.SuspendLayout();
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
			this.mainPictureBox.Size = new System.Drawing.Size(360, 267);
			this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.mainPictureBox.TabIndex = 6;
			this.mainPictureBox.TabStop = false;
			// 
			// chkTitle
			// 
			this.chkTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTitle.Checked = true;
			this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTitle.Location = new System.Drawing.Point(138, 285);
			this.chkTitle.Name = "chkTitle";
			this.chkTitle.Size = new System.Drawing.Size(18, 28);
			this.chkTitle.TabIndex = 8;
			this.chkTitle.UseVisualStyleBackColor = true;
			// 
			// chkDescription
			// 
			this.chkDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkDescription.Checked = true;
			this.chkDescription.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDescription.Location = new System.Drawing.Point(138, 337);
			this.chkDescription.Name = "chkDescription";
			this.chkDescription.Size = new System.Drawing.Size(18, 28);
			this.chkDescription.TabIndex = 10;
			this.chkDescription.UseVisualStyleBackColor = true;
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDescription.Location = new System.Drawing.Point(162, 337);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(336, 76);
			this.txtDescription.TabIndex = 9;
			this.txtDescription.Text = "L1\r\nL2\r\nL3\r\nL4\r\nL5\r\nL6";
			// 
			// txtTags
			// 
			this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTags.Location = new System.Drawing.Point(234, 453);
			this.txtTags.Name = "txtTags";
			this.txtTags.Size = new System.Drawing.Size(264, 28);
			this.txtTags.TabIndex = 11;
			// 
			// chkTags
			// 
			this.chkTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkTags.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTags.Checked = true;
			this.chkTags.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkTags.Location = new System.Drawing.Point(210, 453);
			this.chkTags.Name = "chkTags";
			this.chkTags.Size = new System.Drawing.Size(18, 28);
			this.chkTags.TabIndex = 12;
			this.chkTags.UseVisualStyleBackColor = true;
			// 
			// chkWeasylTag
			// 
			this.chkWeasylTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkWeasylTag.AutoEllipsis = true;
			this.chkWeasylTag.Checked = true;
			this.chkWeasylTag.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWeasylTag.Location = new System.Drawing.Point(138, 453);
			this.chkWeasylTag.Name = "chkWeasylTag";
			this.chkWeasylTag.Size = new System.Drawing.Size(66, 28);
			this.chkWeasylTag.TabIndex = 13;
			this.chkWeasylTag.Text = "#weasyl";
			this.chkWeasylTag.UseVisualStyleBackColor = true;
			// 
			// chkLink
			// 
			this.chkLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkLink.Checked = true;
			this.chkLink.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLink.Location = new System.Drawing.Point(138, 419);
			this.chkLink.Name = "chkLink";
			this.chkLink.Size = new System.Drawing.Size(18, 28);
			this.chkLink.TabIndex = 16;
			this.chkLink.UseVisualStyleBackColor = true;
			// 
			// txtLink
			// 
			this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLink.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLink.ForeColor = System.Drawing.Color.Blue;
			this.txtLink.Location = new System.Drawing.Point(162, 419);
			this.txtLink.Name = "txtLink";
			this.txtLink.Size = new System.Drawing.Size(336, 28);
			this.txtLink.TabIndex = 15;
			this.txtLink.Text = "View on Weasyl";
			// 
			// btnEmail
			// 
			this.btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEmail.Location = new System.Drawing.Point(381, 487);
			this.btnEmail.Name = "btnEmail";
			this.btnEmail.Size = new System.Drawing.Size(117, 23);
			this.btnEmail.TabIndex = 17;
			this.btnEmail.Text = "Email to Tumblr";
			this.btnEmail.UseVisualStyleBackColor = true;
			this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
			// 
			// chkTitleBold
			// 
			this.chkTitleBold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkTitleBold.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkTitleBold.Location = new System.Drawing.Point(408, 284);
			this.chkTitleBold.Name = "chkTitleBold";
			this.chkTitleBold.Size = new System.Drawing.Size(28, 28);
			this.chkTitleBold.TabIndex = 18;
			this.chkTitleBold.Text = "B";
			this.chkTitleBold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkTitleBold.UseVisualStyleBackColor = true;
			this.chkTitleBold.CheckedChanged += new System.EventHandler(this.chkTitleBold_CheckedChanged);
			// 
			// txtTitleSize
			// 
			this.txtTitleSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitleSize.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTitleSize.Location = new System.Drawing.Point(442, 285);
			this.txtTitleSize.Name = "txtTitleSize";
			this.txtTitleSize.Size = new System.Drawing.Size(30, 28);
			this.txtTitleSize.TabIndex = 19;
			this.txtTitleSize.TextChanged += new System.EventHandler(this.txtTitleSize_TextChanged);
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTitle.Location = new System.Drawing.Point(0, 0);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTitle.Size = new System.Drawing.Size(243, 28);
			this.txtTitle.TabIndex = 7;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.txtTitle);
			this.panel1.Location = new System.Drawing.Point(162, 285);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(243, 46);
			this.panel1.TabIndex = 20;
			// 
			// lblPt
			// 
			this.lblPt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPt.AutoSize = true;
			this.lblPt.Location = new System.Drawing.Point(478, 288);
			this.lblPt.Name = "lblPt";
			this.lblPt.Size = new System.Drawing.Size(20, 17);
			this.lblPt.TabIndex = 21;
			this.lblPt.Text = "pt";
			// 
			// lblLink
			// 
			this.lblLink.AutoSize = true;
			this.lblLink.Location = new System.Drawing.Point(138, 493);
			this.lblLink.Name = "lblLink";
			this.lblLink.Size = new System.Drawing.Size(0, 17);
			this.lblLink.TabIndex = 22;
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(510, 522);
			this.Controls.Add(this.lblPt);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.txtTitleSize);
			this.Controls.Add(this.chkTitleBold);
			this.Controls.Add(this.btnEmail);
			this.Controls.Add(this.chkLink);
			this.Controls.Add(this.txtLink);
			this.Controls.Add(this.chkWeasylTag);
			this.Controls.Add(this.chkTags);
			this.Controls.Add(this.txtTags);
			this.Controls.Add(this.chkDescription);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.chkTitle);
			this.Controls.Add(this.mainPictureBox);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.thumbnail4);
			this.Controls.Add(this.thumbnail3);
			this.Controls.Add(this.thumbnail2);
			this.Controls.Add(this.thumbnail1);
			this.Controls.Add(this.lblLink);
			this.Name = "Form1";
			this.Text = "LWeasyl";
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
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
		private System.Windows.Forms.CheckBox chkTitle;
		private System.Windows.Forms.CheckBox chkDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.CheckBox chkTags;
		private System.Windows.Forms.CheckBox chkWeasylTag;
		private System.Windows.Forms.CheckBox chkLink;
		private System.Windows.Forms.TextBox txtLink;
		private System.Windows.Forms.Button btnEmail;
		private System.Windows.Forms.CheckBox chkTitleBold;
		private System.Windows.Forms.TextBox txtTitleSize;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblPt;
		private System.Windows.Forms.Label lblLink;
	}
}

