namespace WeasylSync {
	partial class PostAlreadyExistsDialog {
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.btnReplace = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lnkTumblrPost = new System.Windows.Forms.LinkLabel();
			this.btnAddNew = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(12, 9);
			this.lblMessage.Margin = new System.Windows.Forms.Padding(3);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(310, 16);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "A Tumblr post already exists with the tag {TAG}:";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblQuestion
			// 
			this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblQuestion.Location = new System.Drawing.Point(12, 53);
			this.lblQuestion.Margin = new System.Windows.Forms.Padding(3);
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size(310, 16);
			this.lblQuestion.TabIndex = 2;
			this.lblQuestion.Text = "Would you like to update the existing post or add a new one?";
			this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnReplace
			// 
			this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReplace.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnReplace.Location = new System.Drawing.Point(15, 101);
			this.btnReplace.Name = "btnReplace";
			this.btnReplace.Size = new System.Drawing.Size(75, 23);
			this.btnReplace.TabIndex = 3;
			this.btnReplace.Text = "Replace";
			this.btnReplace.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(247, 101);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lnkTumblrPost
			// 
			this.lnkTumblrPost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkTumblrPost.Location = new System.Drawing.Point(12, 31);
			this.lnkTumblrPost.Margin = new System.Windows.Forms.Padding(3);
			this.lnkTumblrPost.Name = "lnkTumblrPost";
			this.lnkTumblrPost.Size = new System.Drawing.Size(310, 16);
			this.lnkTumblrPost.TabIndex = 1;
			this.lnkTumblrPost.TabStop = true;
			this.lnkTumblrPost.Text = "lnkTumblrPost";
			this.lnkTumblrPost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lnkTumblrPost.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTumblrPost_LinkClicked);
			// 
			// btnAddNew
			// 
			this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddNew.DialogResult = System.Windows.Forms.DialogResult.No;
			this.btnAddNew.Location = new System.Drawing.Point(96, 101);
			this.btnAddNew.Name = "btnAddNew";
			this.btnAddNew.Size = new System.Drawing.Size(75, 23);
			this.btnAddNew.TabIndex = 5;
			this.btnAddNew.Text = "Add New";
			this.btnAddNew.UseVisualStyleBackColor = true;
			// 
			// PostAlreadyExistsDialog
			// 
			this.AcceptButton = this.btnCancel;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(334, 136);
			this.Controls.Add(this.btnAddNew);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnReplace);
			this.Controls.Add(this.lblQuestion);
			this.Controls.Add(this.lnkTumblrPost);
			this.Controls.Add(this.lblMessage);
			this.Name = "PostAlreadyExistsDialog";
			this.Text = "Post Exists";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Label lblQuestion;
		private System.Windows.Forms.Button btnReplace;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.LinkLabel lnkTumblrPost;
		private System.Windows.Forms.Button btnAddNew;
	}
}