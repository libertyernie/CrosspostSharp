namespace CrosspostSharp3 {
	partial class AboutForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			label1 = new System.Windows.Forms.Label();
			linkLabel1 = new System.Windows.Forms.LinkLabel();
			textBox1 = new System.Windows.Forms.TextBox();
			btnOk = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label1.Location = new System.Drawing.Point(14, 9);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(536, 26);
			label1.TabIndex = 0;
			label1.Text = "CrosspostSharp 5.0 beta";
			// 
			// linkLabel1
			// 
			linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			linkLabel1.AutoSize = true;
			linkLabel1.Location = new System.Drawing.Point(292, 35);
			linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			linkLabel1.Name = "linkLabel1";
			linkLabel1.Size = new System.Drawing.Size(260, 15);
			linkLabel1.TabIndex = 1;
			linkLabel1.TabStop = true;
			linkLabel1.Text = "https://github.com/libertyernie/CrosspostSharp";
			linkLabel1.LinkClicked += linkLabel1_LinkClicked;
			// 
			// textBox1
			// 
			textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBox1.Location = new System.Drawing.Point(14, 62);
			textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			textBox1.Size = new System.Drawing.Size(536, 306);
			textBox1.TabIndex = 2;
			textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// btnOk
			// 
			btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnOk.Location = new System.Drawing.Point(463, 376);
			btnOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			btnOk.Name = "btnOk";
			btnOk.Size = new System.Drawing.Size(88, 27);
			btnOk.TabIndex = 3;
			btnOk.Text = "OK";
			btnOk.UseVisualStyleBackColor = true;
			// 
			// AboutForm
			// 
			AcceptButton = btnOk;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(565, 417);
			Controls.Add(btnOk);
			Controls.Add(textBox1);
			Controls.Add(linkLabel1);
			Controls.Add(label1);
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "AboutForm";
			Text = "AboutForm";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnOk;
	}
}