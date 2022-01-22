﻿namespace CrosspostSharp3.FurAffinity {
	partial class FurAffinityPostForm {
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
            this.lblKeywords = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.lblUsername2 = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.ddlCategory = new System.Windows.Forms.ComboBox();
            this.chkScraps = new System.Windows.Forms.CheckBox();
            this.ddlTheme = new System.Windows.Forms.ComboBox();
            this.lblTheme = new System.Windows.Forms.Label();
            this.chkLockComments = new System.Windows.Forms.CheckBox();
            this.ddlSpecies = new System.Windows.Forms.ComboBox();
            this.lblSpecies = new System.Windows.Forms.Label();
            this.ddlGender = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radRating2 = new System.Windows.Forms.RadioButton();
            this.radRating1 = new System.Windows.Forms.RadioButton();
            this.radRating0 = new System.Windows.Forms.RadioButton();
            this.chkRemoveTransparency = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDescription.Location = new System.Drawing.Point(14, 136);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(419, 161);
            this.txtDescription.TabIndex = 5;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(580, 376);
            this.btnPost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(88, 27);
            this.btnPost.TabIndex = 19;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // lblKeywords
            // 
            this.lblKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKeywords.AutoSize = true;
            this.lblKeywords.Location = new System.Drawing.Point(14, 301);
            this.lblKeywords.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(153, 15);
            this.lblKeywords.TabIndex = 6;
            this.lblKeywords.Text = "Keywords (space separated)";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(14, 320);
            this.txtTags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(419, 23);
            this.txtTags.TabIndex = 7;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(14, 73);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(29, 15);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(14, 91);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(419, 23);
            this.txtTitle.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(14, 118);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(67, 15);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description";
            // 
            // picUserIcon
            // 
            this.picUserIcon.Location = new System.Drawing.Point(14, 14);
            this.picUserIcon.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picUserIcon.Name = "picUserIcon";
            this.picUserIcon.Size = new System.Drawing.Size(56, 55);
            this.picUserIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUserIcon.TabIndex = 20;
            this.picUserIcon.TabStop = false;
            // 
            // lblUsername1
            // 
            this.lblUsername1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsername1.Location = new System.Drawing.Point(77, 14);
            this.lblUsername1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername1.Name = "lblUsername1";
            this.lblUsername1.Size = new System.Drawing.Size(590, 20);
            this.lblUsername1.TabIndex = 0;
            this.lblUsername1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsername2
            // 
            this.lblUsername2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsername2.AutoEllipsis = true;
            this.lblUsername2.Location = new System.Drawing.Point(77, 33);
            this.lblUsername2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername2.Name = "lblUsername2";
            this.lblUsername2.Size = new System.Drawing.Size(590, 15);
            this.lblUsername2.TabIndex = 1;
            this.lblUsername2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCategory
            // 
            this.lblCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(441, 95);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(55, 15);
            this.lblCategory.TabIndex = 9;
            this.lblCategory.Text = "Category";
            // 
            // ddlCategory
            // 
            this.ddlCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategory.FormattingEnabled = true;
            this.ddlCategory.Location = new System.Drawing.Point(505, 91);
            this.ddlCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ddlCategory.Name = "ddlCategory";
            this.ddlCategory.Size = new System.Drawing.Size(162, 23);
            this.ddlCategory.TabIndex = 10;
            // 
            // chkScraps
            // 
            this.chkScraps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkScraps.AutoSize = true;
            this.chkScraps.Location = new System.Drawing.Point(453, 122);
            this.chkScraps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkScraps.Name = "chkScraps";
            this.chkScraps.Size = new System.Drawing.Size(93, 19);
            this.chkScraps.TabIndex = 11;
            this.chkScraps.Text = "Put in scraps";
            this.chkScraps.UseVisualStyleBackColor = true;
            // 
            // ddlTheme
            // 
            this.ddlTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTheme.FormattingEnabled = true;
            this.ddlTheme.Location = new System.Drawing.Point(505, 149);
            this.ddlTheme.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ddlTheme.Name = "ddlTheme";
            this.ddlTheme.Size = new System.Drawing.Size(162, 23);
            this.ddlTheme.TabIndex = 13;
            // 
            // lblTheme
            // 
            this.lblTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(441, 152);
            this.lblTheme.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(43, 15);
            this.lblTheme.TabIndex = 12;
            this.lblTheme.Text = "Theme";
            // 
            // chkLockComments
            // 
            this.chkLockComments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkLockComments.AutoSize = true;
            this.chkLockComments.Location = new System.Drawing.Point(14, 351);
            this.chkLockComments.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkLockComments.Name = "chkLockComments";
            this.chkLockComments.Size = new System.Drawing.Size(352, 19);
            this.chkLockComments.TabIndex = 8;
            this.chkLockComments.Text = "Lock comments (minimal duration for this setting is 24 hours)";
            this.chkLockComments.UseVisualStyleBackColor = true;
            // 
            // ddlSpecies
            // 
            this.ddlSpecies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSpecies.DisplayMember = "Name";
            this.ddlSpecies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSpecies.FormattingEnabled = true;
            this.ddlSpecies.Location = new System.Drawing.Point(505, 180);
            this.ddlSpecies.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ddlSpecies.Name = "ddlSpecies";
            this.ddlSpecies.Size = new System.Drawing.Size(162, 23);
            this.ddlSpecies.TabIndex = 15;
            this.ddlSpecies.ValueMember = "Id";
            // 
            // lblSpecies
            // 
            this.lblSpecies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpecies.AutoSize = true;
            this.lblSpecies.Location = new System.Drawing.Point(441, 183);
            this.lblSpecies.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpecies.Name = "lblSpecies";
            this.lblSpecies.Size = new System.Drawing.Size(46, 15);
            this.lblSpecies.TabIndex = 14;
            this.lblSpecies.Text = "Species";
            // 
            // ddlGender
            // 
            this.ddlGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlGender.FormattingEnabled = true;
            this.ddlGender.Location = new System.Drawing.Point(505, 211);
            this.ddlGender.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ddlGender.Name = "ddlGender";
            this.ddlGender.Size = new System.Drawing.Size(162, 23);
            this.ddlGender.TabIndex = 17;
            // 
            // lblGender
            // 
            this.lblGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(441, 215);
            this.lblGender.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(45, 15);
            this.lblGender.TabIndex = 16;
            this.lblGender.Text = "Gender";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.radRating2);
            this.groupBox3.Controls.Add(this.radRating1);
            this.groupBox3.Controls.Add(this.radRating0);
            this.groupBox3.Location = new System.Drawing.Point(441, 242);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(226, 47);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rating";
            // 
            // radRating2
            // 
            this.radRating2.AutoSize = true;
            this.radRating2.Location = new System.Drawing.Point(161, 22);
            this.radRating2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radRating2.Name = "radRating2";
            this.radRating2.Size = new System.Drawing.Size(54, 19);
            this.radRating2.TabIndex = 2;
            this.radRating2.TabStop = true;
            this.radRating2.Text = "Adult";
            this.radRating2.UseVisualStyleBackColor = true;
            // 
            // radRating1
            // 
            this.radRating1.AutoSize = true;
            this.radRating1.Location = new System.Drawing.Point(86, 22);
            this.radRating1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radRating1.Name = "radRating1";
            this.radRating1.Size = new System.Drawing.Size(63, 19);
            this.radRating1.TabIndex = 1;
            this.radRating1.TabStop = true;
            this.radRating1.Text = "Mature";
            this.radRating1.UseVisualStyleBackColor = true;
            // 
            // radRating0
            // 
            this.radRating0.AutoSize = true;
            this.radRating0.Location = new System.Drawing.Point(7, 22);
            this.radRating0.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radRating0.Name = "radRating0";
            this.radRating0.Size = new System.Drawing.Size(65, 19);
            this.radRating0.TabIndex = 0;
            this.radRating0.TabStop = true;
            this.radRating0.Text = "General";
            this.radRating0.UseVisualStyleBackColor = true;
            // 
            // chkRemoveTransparency
            // 
            this.chkRemoveTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkRemoveTransparency.AutoSize = true;
            this.chkRemoveTransparency.Location = new System.Drawing.Point(379, 351);
            this.chkRemoveTransparency.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkRemoveTransparency.Name = "chkRemoveTransparency";
            this.chkRemoveTransparency.Size = new System.Drawing.Size(140, 19);
            this.chkRemoveTransparency.TabIndex = 21;
            this.chkRemoveTransparency.Text = "Remove transparency";
            this.chkRemoveTransparency.UseVisualStyleBackColor = true;
            // 
            // FurAffinityPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 417);
            this.Controls.Add(this.chkRemoveTransparency);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ddlGender);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.ddlSpecies);
            this.Controls.Add(this.lblSpecies);
            this.Controls.Add(this.chkLockComments);
            this.Controls.Add(this.ddlTheme);
            this.Controls.Add(this.lblTheme);
            this.Controls.Add(this.chkScraps);
            this.Controls.Add(this.ddlCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.txtDescription);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FurAffinityPostForm";
            this.Text = "Post to FurAffinity";
            this.Shown += new System.EventHandler(this.Form_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.Label lblKeywords;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.Label lblUsername2;
		private System.Windows.Forms.Label lblCategory;
		private System.Windows.Forms.ComboBox ddlCategory;
		private System.Windows.Forms.CheckBox chkScraps;
		private System.Windows.Forms.ComboBox ddlTheme;
		private System.Windows.Forms.Label lblTheme;
		private System.Windows.Forms.CheckBox chkLockComments;
		private System.Windows.Forms.ComboBox ddlSpecies;
		private System.Windows.Forms.Label lblSpecies;
		private System.Windows.Forms.ComboBox ddlGender;
		private System.Windows.Forms.Label lblGender;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radRating2;
		private System.Windows.Forms.RadioButton radRating1;
		private System.Windows.Forms.RadioButton radRating0;
		private System.Windows.Forms.CheckBox chkRemoveTransparency;
	}
}