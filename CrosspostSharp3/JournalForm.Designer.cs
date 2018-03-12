﻿namespace CrosspostSharp3 {
	partial class JournalForm {
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
            this.lstSource = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblBody = new System.Windows.Forms.Label();
            this.lblTeaser = new System.Windows.Forms.Label();
            this.btnPost = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSource = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstDestination = new System.Windows.Forms.ListBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.txtTeaser = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstSource
            // 
            this.lstSource.DisplayMember = "Title";
            this.lstSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSource.FormattingEnabled = true;
            this.lstSource.IntegralHeight = false;
            this.lstSource.Location = new System.Drawing.Point(0, 13);
            this.lstSource.Name = "lstSource";
            this.lstSource.Size = new System.Drawing.Size(150, 248);
            this.lstSource.TabIndex = 0;
            this.lstSource.SelectedIndexChanged += new System.EventHandler(this.lstSource_SelectedIndexChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(153, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(156, 44);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(416, 20);
            this.txtTitle.TabIndex = 3;
            // 
            // lblBody
            // 
            this.lblBody.AutoSize = true;
            this.lblBody.Location = new System.Drawing.Point(156, 67);
            this.lblBody.Name = "lblBody";
            this.lblBody.Size = new System.Drawing.Size(31, 13);
            this.lblBody.TabIndex = 4;
            this.lblBody.Text = "Body";
            // 
            // lblTeaser
            // 
            this.lblTeaser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTeaser.AutoSize = true;
            this.lblTeaser.Location = new System.Drawing.Point(156, 224);
            this.lblTeaser.Name = "lblTeaser";
            this.lblTeaser.Size = new System.Drawing.Size(40, 13);
            this.lblTeaser.TabIndex = 6;
            this.lblTeaser.Text = "Teaser";
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.Location = new System.Drawing.Point(497, 326);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 8;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstSource);
            this.panel1.Controls.Add(this.lblSource);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 261);
            this.panel1.TabIndex = 9;
            // 
            // lblSource
            // 
            this.lblSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSource.Location = new System.Drawing.Point(0, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(150, 13);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Source";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstDestination);
            this.panel2.Controls.Add(this.lblDestination);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 261);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 100);
            this.panel2.TabIndex = 10;
            // 
            // lstDestination
            // 
            this.lstDestination.DisplayMember = "SiteName";
            this.lstDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDestination.FormattingEnabled = true;
            this.lstDestination.IntegralHeight = false;
            this.lstDestination.Location = new System.Drawing.Point(0, 13);
            this.lstDestination.Name = "lstDestination";
            this.lstDestination.Size = new System.Drawing.Size(150, 87);
            this.lstDestination.TabIndex = 0;
            this.lstDestination.SelectedIndexChanged += new System.EventHandler(this.lstDestination_SelectedIndexChanged);
            // 
            // lblDestination
            // 
            this.lblDestination.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDestination.Location = new System.Drawing.Point(0, 0);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(150, 13);
            this.lblDestination.TabIndex = 1;
            this.lblDestination.Text = "Destination";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(150, 361);
            this.panel3.TabIndex = 11;
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimestamp.Location = new System.Drawing.Point(156, 13);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(416, 13);
            this.lblTimestamp.TabIndex = 12;
            this.lblTimestamp.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtBody
            // 
            this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBody.Location = new System.Drawing.Point(156, 83);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(416, 138);
            this.txtBody.TabIndex = 13;
            // 
            // txtTeaser
            // 
            this.txtTeaser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTeaser.Location = new System.Drawing.Point(156, 240);
            this.txtTeaser.Multiline = true;
            this.txtTeaser.Name = "txtTeaser";
            this.txtTeaser.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTeaser.Size = new System.Drawing.Size(416, 80);
            this.txtTeaser.TabIndex = 14;
            // 
            // JournalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.txtTeaser);
            this.Controls.Add(this.txtBody);
            this.Controls.Add(this.lblTimestamp);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.lblTeaser);
            this.Controls.Add(this.lblBody);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Name = "JournalForm";
            this.Text = "Journals";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstSource;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblBody;
		private System.Windows.Forms.Label lblTeaser;
		private System.Windows.Forms.Button btnPost;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListBox lstDestination;
		private System.Windows.Forms.Label lblDestination;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lblTimestamp;
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.TextBox txtTeaser;
	}
}