namespace CrosspostSharp3 {
	partial class FurryNetworkPostForm {
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
            this.picUserIcon = new System.Windows.Forms.PictureBox();
            this.lblUsername1 = new System.Windows.Forms.Label();
            this.lblUsername2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radFurryNetworkDraft = new System.Windows.Forms.RadioButton();
            this.radFurryNetworkUnlisted = new System.Windows.Forms.RadioButton();
            this.radFurryNetworkPublic = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radFurryNetworkPHoto = new System.Windows.Forms.RadioButton();
            this.radFurAffinityArtwork = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radFurryNetworkRating2 = new System.Windows.Forms.RadioButton();
            this.radFurryNetworkRating1 = new System.Windows.Forms.RadioButton();
            this.radFurryNetworkRating0 = new System.Windows.Forms.RadioButton();
            this.chkFurryNetworkAllowCommunityTags = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.txtDescription.Size = new System.Drawing.Size(360, 75);
            this.txtDescription.TabIndex = 5;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(297, 326);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 12;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // lblTags
            // 
            this.lblTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 196);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(31, 13);
            this.lblTags.TabIndex = 6;
            this.lblTags.Text = "Tags";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(12, 212);
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
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(12, 79);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(360, 20);
            this.txtTitle.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 102);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(119, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description (Markdown)";
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
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.radFurryNetworkDraft);
            this.groupBox4.Controls.Add(this.radFurryNetworkUnlisted);
            this.groupBox4.Controls.Add(this.radFurryNetworkPublic);
            this.groupBox4.Location = new System.Drawing.Point(194, 261);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(85, 88);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Status";
            // 
            // radFurryNetworkDraft
            // 
            this.radFurryNetworkDraft.AutoSize = true;
            this.radFurryNetworkDraft.Location = new System.Drawing.Point(6, 65);
            this.radFurryNetworkDraft.Name = "radFurryNetworkDraft";
            this.radFurryNetworkDraft.Size = new System.Drawing.Size(48, 17);
            this.radFurryNetworkDraft.TabIndex = 2;
            this.radFurryNetworkDraft.Text = "Draft";
            this.radFurryNetworkDraft.UseVisualStyleBackColor = true;
            // 
            // radFurryNetworkUnlisted
            // 
            this.radFurryNetworkUnlisted.AutoSize = true;
            this.radFurryNetworkUnlisted.Location = new System.Drawing.Point(6, 42);
            this.radFurryNetworkUnlisted.Name = "radFurryNetworkUnlisted";
            this.radFurryNetworkUnlisted.Size = new System.Drawing.Size(63, 17);
            this.radFurryNetworkUnlisted.TabIndex = 1;
            this.radFurryNetworkUnlisted.Text = "Unlisted";
            this.radFurryNetworkUnlisted.UseVisualStyleBackColor = true;
            // 
            // radFurryNetworkPublic
            // 
            this.radFurryNetworkPublic.AutoSize = true;
            this.radFurryNetworkPublic.Checked = true;
            this.radFurryNetworkPublic.Location = new System.Drawing.Point(6, 19);
            this.radFurryNetworkPublic.Name = "radFurryNetworkPublic";
            this.radFurryNetworkPublic.Size = new System.Drawing.Size(54, 17);
            this.radFurryNetworkPublic.TabIndex = 0;
            this.radFurryNetworkPublic.TabStop = true;
            this.radFurryNetworkPublic.Text = "Public";
            this.radFurryNetworkPublic.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.radFurryNetworkPHoto);
            this.groupBox2.Controls.Add(this.radFurAffinityArtwork);
            this.groupBox2.Location = new System.Drawing.Point(103, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(85, 88);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Media type";
            // 
            // radFurryNetworkPHoto
            // 
            this.radFurryNetworkPHoto.AutoSize = true;
            this.radFurryNetworkPHoto.Location = new System.Drawing.Point(6, 42);
            this.radFurryNetworkPHoto.Name = "radFurryNetworkPHoto";
            this.radFurryNetworkPHoto.Size = new System.Drawing.Size(53, 17);
            this.radFurryNetworkPHoto.TabIndex = 1;
            this.radFurryNetworkPHoto.Text = "Photo";
            this.radFurryNetworkPHoto.UseVisualStyleBackColor = true;
            // 
            // radFurAffinityArtwork
            // 
            this.radFurAffinityArtwork.AutoSize = true;
            this.radFurAffinityArtwork.Checked = true;
            this.radFurAffinityArtwork.Location = new System.Drawing.Point(6, 19);
            this.radFurAffinityArtwork.Name = "radFurAffinityArtwork";
            this.radFurAffinityArtwork.Size = new System.Drawing.Size(61, 17);
            this.radFurAffinityArtwork.TabIndex = 0;
            this.radFurAffinityArtwork.TabStop = true;
            this.radFurAffinityArtwork.Text = "Artwork";
            this.radFurAffinityArtwork.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.radFurryNetworkRating2);
            this.groupBox3.Controls.Add(this.radFurryNetworkRating1);
            this.groupBox3.Controls.Add(this.radFurryNetworkRating0);
            this.groupBox3.Location = new System.Drawing.Point(12, 261);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(85, 88);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rating";
            // 
            // radFurryNetworkRating2
            // 
            this.radFurryNetworkRating2.AutoSize = true;
            this.radFurryNetworkRating2.Location = new System.Drawing.Point(6, 65);
            this.radFurryNetworkRating2.Name = "radFurryNetworkRating2";
            this.radFurryNetworkRating2.Size = new System.Drawing.Size(58, 17);
            this.radFurryNetworkRating2.TabIndex = 2;
            this.radFurryNetworkRating2.TabStop = true;
            this.radFurryNetworkRating2.Text = "Explicit";
            this.radFurryNetworkRating2.UseVisualStyleBackColor = true;
            // 
            // radFurryNetworkRating1
            // 
            this.radFurryNetworkRating1.AutoSize = true;
            this.radFurryNetworkRating1.Location = new System.Drawing.Point(6, 42);
            this.radFurryNetworkRating1.Name = "radFurryNetworkRating1";
            this.radFurryNetworkRating1.Size = new System.Drawing.Size(58, 17);
            this.radFurryNetworkRating1.TabIndex = 1;
            this.radFurryNetworkRating1.TabStop = true;
            this.radFurryNetworkRating1.Text = "Mature";
            this.radFurryNetworkRating1.UseVisualStyleBackColor = true;
            // 
            // radFurryNetworkRating0
            // 
            this.radFurryNetworkRating0.AutoSize = true;
            this.radFurryNetworkRating0.Location = new System.Drawing.Point(6, 19);
            this.radFurryNetworkRating0.Name = "radFurryNetworkRating0";
            this.radFurryNetworkRating0.Size = new System.Drawing.Size(62, 17);
            this.radFurryNetworkRating0.TabIndex = 0;
            this.radFurryNetworkRating0.TabStop = true;
            this.radFurryNetworkRating0.Text = "General";
            this.radFurryNetworkRating0.UseVisualStyleBackColor = true;
            // 
            // chkFurryNetworkAllowCommunityTags
            // 
            this.chkFurryNetworkAllowCommunityTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFurryNetworkAllowCommunityTags.AutoSize = true;
            this.chkFurryNetworkAllowCommunityTags.Checked = true;
            this.chkFurryNetworkAllowCommunityTags.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFurryNetworkAllowCommunityTags.Location = new System.Drawing.Point(12, 238);
            this.chkFurryNetworkAllowCommunityTags.Name = "chkFurryNetworkAllowCommunityTags";
            this.chkFurryNetworkAllowCommunityTags.Size = new System.Drawing.Size(127, 17);
            this.chkFurryNetworkAllowCommunityTags.TabIndex = 8;
            this.chkFurryNetworkAllowCommunityTags.Text = "Allow community tags";
            this.chkFurryNetworkAllowCommunityTags.UseVisualStyleBackColor = true;
            // 
            // FurryNetworkPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.chkFurryNetworkAllowCommunityTags);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.picUserIcon);
            this.Controls.Add(this.lblUsername1);
            this.Controls.Add(this.lblUsername2);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.txtDescription);
            this.Name = "FurryNetworkPostForm";
            this.Text = "Post to Inkbunny";
            this.Shown += new System.EventHandler(this.Form_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picUserIcon)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
		private System.Windows.Forms.PictureBox picUserIcon;
		private System.Windows.Forms.Label lblUsername1;
		private System.Windows.Forms.Label lblUsername2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radFurryNetworkDraft;
		private System.Windows.Forms.RadioButton radFurryNetworkUnlisted;
		private System.Windows.Forms.RadioButton radFurryNetworkPublic;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radFurryNetworkPHoto;
		private System.Windows.Forms.RadioButton radFurAffinityArtwork;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radFurryNetworkRating2;
		private System.Windows.Forms.RadioButton radFurryNetworkRating1;
		private System.Windows.Forms.RadioButton radFurryNetworkRating0;
		private System.Windows.Forms.CheckBox chkFurryNetworkAllowCommunityTags;
	}
}