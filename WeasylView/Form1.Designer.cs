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
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnail1
			// 
			this.thumbnail1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail1.Location = new System.Drawing.Point(12, 12);
			this.thumbnail1.Margin = new System.Windows.Forms.Padding(0);
			this.thumbnail1.Name = "thumbnail1";
			this.thumbnail1.Size = new System.Drawing.Size(120, 120);
			this.thumbnail1.TabIndex = 0;
			this.thumbnail1.TabStop = false;
			// 
			// thumbnail2
			// 
			this.thumbnail2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail2.Location = new System.Drawing.Point(12, 138);
			this.thumbnail2.Margin = new System.Windows.Forms.Padding(0);
			this.thumbnail2.Name = "thumbnail2";
			this.thumbnail2.Size = new System.Drawing.Size(120, 120);
			this.thumbnail2.TabIndex = 1;
			this.thumbnail2.TabStop = false;
			// 
			// thumbnail3
			// 
			this.thumbnail3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.thumbnail3.Location = new System.Drawing.Point(12, 264);
			this.thumbnail3.Margin = new System.Windows.Forms.Padding(0);
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
			this.thumbnail4.Margin = new System.Windows.Forms.Padding(0);
			this.thumbnail4.Name = "thumbnail4";
			this.thumbnail4.Size = new System.Drawing.Size(120, 120);
			this.thumbnail4.TabIndex = 3;
			this.thumbnail4.TabStop = false;
			// 
			// btnUp
			// 
			this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUp.Location = new System.Drawing.Point(135, 12);
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
			this.btnDown.Location = new System.Drawing.Point(135, 478);
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
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(138, 47);
			this.mainPictureBox.Margin = new System.Windows.Forms.Padding(0);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(428, 428);
			this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.mainPictureBox.TabIndex = 6;
			this.mainPictureBox.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(575, 522);
			this.Controls.Add(this.mainPictureBox);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.thumbnail4);
			this.Controls.Add(this.thumbnail3);
			this.Controls.Add(this.thumbnail2);
			this.Controls.Add(this.thumbnail1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.thumbnail1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox thumbnail1;
		private System.Windows.Forms.PictureBox thumbnail2;
		private System.Windows.Forms.PictureBox thumbnail3;
		private System.Windows.Forms.PictureBox thumbnail4;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.PictureBox mainPictureBox;
	}
}

