namespace CrosspostSharp3 {
	partial class DestinationSelectionForm {
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
            this.btnDeviantArt = new System.Windows.Forms.Button();
            this.btnTwitter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDeviantArt
            // 
            this.btnDeviantArt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeviantArt.Location = new System.Drawing.Point(12, 12);
            this.btnDeviantArt.Name = "btnDeviantArt";
            this.btnDeviantArt.Size = new System.Drawing.Size(210, 23);
            this.btnDeviantArt.TabIndex = 0;
            this.btnDeviantArt.Text = "DeviantArt / Sta.sh";
            this.btnDeviantArt.UseVisualStyleBackColor = true;
            this.btnDeviantArt.Click += new System.EventHandler(this.btnDeviantArt_Click);
            // 
            // btnTwitter
            // 
            this.btnTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTwitter.Location = new System.Drawing.Point(12, 41);
            this.btnTwitter.Name = "btnTwitter";
            this.btnTwitter.Size = new System.Drawing.Size(210, 23);
            this.btnTwitter.TabIndex = 1;
            this.btnTwitter.Text = "Twitter";
            this.btnTwitter.UseVisualStyleBackColor = true;
            this.btnTwitter.Click += new System.EventHandler(this.btnTwitter_Click);
            // 
            // DestinationSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 76);
            this.Controls.Add(this.btnTwitter);
            this.Controls.Add(this.btnDeviantArt);
            this.Name = "DestinationSelectionForm";
            this.Text = "Select Destination";
            this.Shown += new System.EventHandler(this.DestinationSelectionForm_Shown);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDeviantArt;
		private System.Windows.Forms.Button btnTwitter;
	}
}