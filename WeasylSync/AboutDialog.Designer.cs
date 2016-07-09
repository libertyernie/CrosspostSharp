namespace WeasylSync {
	partial class AboutDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
			this.lblTitle = new System.Windows.Forms.Label();
			this.lnkJsonNET = new System.Windows.Forms.LinkLabel();
			this.lblTumblrSharpCopyright = new System.Windows.Forms.Label();
			this.lblJsonNETCopyright = new System.Windows.Forms.Label();
			this.lblBoth = new System.Windows.Forms.Label();
			this.btnOkay = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lnkTumblrSharp = new System.Windows.Forms.LinkLabel();
			this.lnkWebsite = new System.Windows.Forms.LinkLabel();
			this.lnkHtmlAgilityPack = new System.Windows.Forms.LinkLabel();
			this.lblHtmlAgilityPackLicense1 = new System.Windows.Forms.Label();
			this.lnkHtmlAgilityPackLicense2 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(7, 12);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(3);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(181, 25);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "WeasylSync 1.1";
			// 
			// lnkJsonNET
			// 
			this.lnkJsonNET.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkJsonNET.AutoSize = true;
			this.lnkJsonNET.Location = new System.Drawing.Point(9, 362);
			this.lnkJsonNET.Margin = new System.Windows.Forms.Padding(3);
			this.lnkJsonNET.Name = "lnkJsonNET";
			this.lnkJsonNET.Size = new System.Drawing.Size(57, 13);
			this.lnkJsonNET.TabIndex = 5;
			this.lnkJsonNET.TabStop = true;
			this.lnkJsonNET.Text = "Json.NET:";
			this.lnkJsonNET.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkJsonNET_LinkClicked);
			// 
			// lblTumblrSharpCopyright
			// 
			this.lblTumblrSharpCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTumblrSharpCopyright.AutoSize = true;
			this.lblTumblrSharpCopyright.Location = new System.Drawing.Point(64, 343);
			this.lblTumblrSharpCopyright.Margin = new System.Windows.Forms.Padding(3);
			this.lblTumblrSharpCopyright.Name = "lblTumblrSharpCopyright";
			this.lblTumblrSharpCopyright.Size = new System.Drawing.Size(223, 13);
			this.lblTumblrSharpCopyright.TabIndex = 4;
			this.lblTumblrSharpCopyright.Text = "Copyright (c) 2013-2014 Don\'t Panic Software";
			// 
			// lblJsonNETCopyright
			// 
			this.lblJsonNETCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblJsonNETCopyright.AutoSize = true;
			this.lblJsonNETCopyright.Location = new System.Drawing.Point(72, 362);
			this.lblJsonNETCopyright.Margin = new System.Windows.Forms.Padding(3);
			this.lblJsonNETCopyright.Name = "lblJsonNETCopyright";
			this.lblJsonNETCopyright.Size = new System.Drawing.Size(217, 13);
			this.lblJsonNETCopyright.TabIndex = 6;
			this.lblJsonNETCopyright.Text = "Copyright (c) 2007-2016 James Newton-King";
			// 
			// lblBoth
			// 
			this.lblBoth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblBoth.AutoSize = true;
			this.lblBoth.Location = new System.Drawing.Point(9, 381);
			this.lblBoth.Margin = new System.Windows.Forms.Padding(3);
			this.lblBoth.Name = "lblBoth";
			this.lblBoth.Size = new System.Drawing.Size(277, 13);
			this.lblBoth.TabIndex = 7;
			this.lblBoth.Text = "Both projects are distributed under the terms listed above.";
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(347, 426);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 11;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(12, 67);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(410, 270);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// lnkTumblrSharp
			// 
			this.lnkTumblrSharp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkTumblrSharp.AutoSize = true;
			this.lnkTumblrSharp.Location = new System.Drawing.Point(9, 343);
			this.lnkTumblrSharp.Margin = new System.Windows.Forms.Padding(3);
			this.lnkTumblrSharp.Name = "lnkTumblrSharp";
			this.lnkTumblrSharp.Size = new System.Drawing.Size(49, 13);
			this.lnkTumblrSharp.TabIndex = 3;
			this.lnkTumblrSharp.TabStop = true;
			this.lnkTumblrSharp.Text = "Tumblr#:";
			this.lnkTumblrSharp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTumblrSharp_LinkClicked);
			// 
			// lnkWebsite
			// 
			this.lnkWebsite.AutoSize = true;
			this.lnkWebsite.Location = new System.Drawing.Point(12, 44);
			this.lnkWebsite.Name = "lnkWebsite";
			this.lnkWebsite.Size = new System.Drawing.Size(214, 13);
			this.lnkWebsite.TabIndex = 1;
			this.lnkWebsite.TabStop = true;
			this.lnkWebsite.Text = "https://github.com/libertyernie/WeasylSync";
			this.lnkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebsite_LinkClicked);
			// 
			// lnkHtmlAgilityPack
			// 
			this.lnkHtmlAgilityPack.AutoSize = true;
			this.lnkHtmlAgilityPack.Location = new System.Drawing.Point(9, 407);
			this.lnkHtmlAgilityPack.Margin = new System.Windows.Forms.Padding(3);
			this.lnkHtmlAgilityPack.Name = "lnkHtmlAgilityPack";
			this.lnkHtmlAgilityPack.Size = new System.Drawing.Size(89, 13);
			this.lnkHtmlAgilityPack.TabIndex = 8;
			this.lnkHtmlAgilityPack.TabStop = true;
			this.lnkHtmlAgilityPack.Text = "Html Agility Pack:";
			this.lnkHtmlAgilityPack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHtmlAgilityPack_LinkClicked);
			// 
			// lblHtmlAgilityPackLicense1
			// 
			this.lblHtmlAgilityPackLicense1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblHtmlAgilityPackLicense1.AutoSize = true;
			this.lblHtmlAgilityPackLicense1.Location = new System.Drawing.Point(104, 407);
			this.lblHtmlAgilityPackLicense1.Margin = new System.Windows.Forms.Padding(3);
			this.lblHtmlAgilityPackLicense1.Name = "lblHtmlAgilityPackLicense1";
			this.lblHtmlAgilityPackLicense1.Size = new System.Drawing.Size(98, 13);
			this.lblHtmlAgilityPackLicense1.TabIndex = 9;
			this.lblHtmlAgilityPackLicense1.Text = "Available under the";
			// 
			// lnkHtmlAgilityPackLicense2
			// 
			this.lnkHtmlAgilityPackLicense2.AutoSize = true;
			this.lnkHtmlAgilityPackLicense2.Location = new System.Drawing.Point(198, 407);
			this.lnkHtmlAgilityPackLicense2.Margin = new System.Windows.Forms.Padding(3);
			this.lnkHtmlAgilityPackLicense2.Name = "lnkHtmlAgilityPackLicense2";
			this.lnkHtmlAgilityPackLicense2.Size = new System.Drawing.Size(125, 13);
			this.lnkHtmlAgilityPackLicense2.TabIndex = 10;
			this.lnkHtmlAgilityPackLicense2.TabStop = true;
			this.lnkHtmlAgilityPackLicense2.Text = "Microsoft Public License.";
			this.lnkHtmlAgilityPackLicense2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHtmlAgilityPackLicense2_LinkClicked);
			// 
			// AboutDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 461);
			this.Controls.Add(this.lnkHtmlAgilityPackLicense2);
			this.Controls.Add(this.lblHtmlAgilityPackLicense1);
			this.Controls.Add(this.lnkHtmlAgilityPack);
			this.Controls.Add(this.lnkWebsite);
			this.Controls.Add(this.lblBoth);
			this.Controls.Add(this.lnkJsonNET);
			this.Controls.Add(this.lblJsonNETCopyright);
			this.Controls.Add(this.lblTumblrSharpCopyright);
			this.Controls.Add(this.lnkTumblrSharp);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.btnOkay);
			this.Name = "AboutDialog";
			this.Text = "About";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.LinkLabel lnkJsonNET;
		private System.Windows.Forms.Label lblTumblrSharpCopyright;
		private System.Windows.Forms.Label lblJsonNETCopyright;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Label lblBoth;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.LinkLabel lnkTumblrSharp;
		private System.Windows.Forms.LinkLabel lnkWebsite;
		private System.Windows.Forms.LinkLabel lnkHtmlAgilityPack;
		private System.Windows.Forms.Label lblHtmlAgilityPackLicense1;
		private System.Windows.Forms.LinkLabel lnkHtmlAgilityPackLicense2;
	}
}