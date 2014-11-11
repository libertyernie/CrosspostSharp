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
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lnkTumblrPost = new System.Windows.Forms.LinkLabel();
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
			this.lblMessage.Text = "A Tumblr post already exists with the tag #{TAG}:";
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
			this.lblQuestion.Text = "Are you sure you want to add a new post?";
			this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(166, 101);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 3;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
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
			// PostAlreadyExistsDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 136);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
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
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.LinkLabel lnkTumblrPost;
	}
}