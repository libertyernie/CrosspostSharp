namespace CrosspostSharp {
    partial class MultiPhotoPostForm {
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLoadMore = new System.Windows.Forms.Button();
            this.lblTags = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.lblTweetLength = new System.Windows.Forms.Label();
            this.btnPostToTumblr = new System.Windows.Forms.Button();
            this.btnPostToTwitter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.DisplayMember = "Title";
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(133, 238);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(68, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(77, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(68, 68);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(151, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(68, 68);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(225, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(68, 68);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoadMore);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblTags);
            this.splitContainer1.Panel2.Controls.Add(this.txtTags);
            this.splitContainer1.Panel2.Controls.Add(this.lblText);
            this.splitContainer1.Panel2.Controls.Add(this.txtBody);
            this.splitContainer1.Panel2.Controls.Add(this.lblTweetLength);
            this.splitContainer1.Panel2.Controls.Add(this.btnPostToTumblr);
            this.splitContainer1.Panel2.Controls.Add(this.btnPostToTwitter);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox3);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox4);
            this.splitContainer1.Size = new System.Drawing.Size(434, 261);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnLoadMore
            // 
            this.btnLoadMore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadMore.Location = new System.Drawing.Point(0, 238);
            this.btnLoadMore.Name = "btnLoadMore";
            this.btnLoadMore.Size = new System.Drawing.Size(133, 23);
            this.btnLoadMore.TabIndex = 1;
            this.btnLoadMore.Text = "Load More";
            this.btnLoadMore.UseVisualStyleBackColor = true;
            this.btnLoadMore.Click += new System.EventHandler(this.btnLoadMore_Click);
            // 
            // lblTags
            // 
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(3, 199);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(34, 13);
            this.lblTags.TabIndex = 2;
            this.lblTags.Text = "Tags:";
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(43, 196);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(250, 20);
            this.txtTags.TabIndex = 3;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(3, 74);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(218, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Text (plain for Twitter, Markdown for Tumblr):";
            // 
            // txtBody
            // 
            this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBody.Location = new System.Drawing.Point(2, 90);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(291, 100);
            this.txtBody.TabIndex = 1;
            this.txtBody.TextChanged += new System.EventHandler(this.txtBody_TextChanged);
            // 
            // lblTweetLength
            // 
            this.lblTweetLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTweetLength.AutoSize = true;
            this.lblTweetLength.Location = new System.Drawing.Point(3, 219);
            this.lblTweetLength.Name = "lblTweetLength";
            this.lblTweetLength.Size = new System.Drawing.Size(0, 13);
            this.lblTweetLength.TabIndex = 4;
            // 
            // btnPostToTumblr
            // 
            this.btnPostToTumblr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostToTumblr.Location = new System.Drawing.Point(194, 235);
            this.btnPostToTumblr.Name = "btnPostToTumblr";
            this.btnPostToTumblr.Size = new System.Drawing.Size(100, 23);
            this.btnPostToTumblr.TabIndex = 6;
            this.btnPostToTumblr.Text = "Post to Tumblr";
            this.btnPostToTumblr.UseVisualStyleBackColor = true;
            this.btnPostToTumblr.Click += new System.EventHandler(this.btnPostToTumblr_Click);
            // 
            // btnPostToTwitter
            // 
            this.btnPostToTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostToTwitter.Location = new System.Drawing.Point(88, 235);
            this.btnPostToTwitter.Name = "btnPostToTwitter";
            this.btnPostToTwitter.Size = new System.Drawing.Size(100, 23);
            this.btnPostToTwitter.TabIndex = 5;
            this.btnPostToTwitter.Text = "Post to Twitter";
            this.btnPostToTwitter.UseVisualStyleBackColor = true;
            this.btnPostToTwitter.Click += new System.EventHandler(this.btnPostToTwitter_Click);
            // 
            // MultiPhotoPostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MultiPhotoPostForm";
            this.Text = "Multi-Photo Post";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnPostToTumblr;
        private System.Windows.Forms.Button btnPostToTwitter;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblTweetLength;
        private System.Windows.Forms.Button btnLoadMore;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label lblTags;
    }
}