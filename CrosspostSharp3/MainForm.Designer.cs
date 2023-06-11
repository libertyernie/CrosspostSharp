namespace CrosspostSharp3 {
	partial class MainForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			accountSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			deviantArtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			furAffinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			mastodonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			pixelfedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			weasylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			menuStrip1.Size = new System.Drawing.Size(284, 24);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, exitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			openToolStripMenuItem.Text = "&Open...";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			exitToolStripMenuItem.Text = "E&xit";
			exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
			// 
			// toolsToolStripMenuItem
			// 
			toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { accountSetupToolStripMenuItem });
			toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			toolsToolStripMenuItem.Text = "&Tools";
			// 
			// accountSetupToolStripMenuItem
			// 
			accountSetupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { deviantArtToolStripMenuItem, furAffinityToolStripMenuItem, mastodonToolStripMenuItem, pixelfedToolStripMenuItem, weasylToolStripMenuItem });
			accountSetupToolStripMenuItem.Name = "accountSetupToolStripMenuItem";
			accountSetupToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			accountSetupToolStripMenuItem.Text = "&Account Setup";
			// 
			// deviantArtToolStripMenuItem
			// 
			deviantArtToolStripMenuItem.Name = "deviantArtToolStripMenuItem";
			deviantArtToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			deviantArtToolStripMenuItem.Text = "&DeviantArt";
			deviantArtToolStripMenuItem.Click += deviantArtToolStripMenuItem_Click;
			// 
			// furAffinityToolStripMenuItem
			// 
			furAffinityToolStripMenuItem.Name = "furAffinityToolStripMenuItem";
			furAffinityToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			furAffinityToolStripMenuItem.Text = "Fur&Affinity";
			furAffinityToolStripMenuItem.Click += furAffinityToolStripMenuItem_Click;
			// 
			// mastodonToolStripMenuItem
			// 
			mastodonToolStripMenuItem.Name = "mastodonToolStripMenuItem";
			mastodonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			mastodonToolStripMenuItem.Text = "&Mastodon";
			mastodonToolStripMenuItem.Click += mastodonToolStripMenuItem_Click;
			// 
			// pixelfedToolStripMenuItem
			// 
			pixelfedToolStripMenuItem.Name = "pixelfedToolStripMenuItem";
			pixelfedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			pixelfedToolStripMenuItem.Text = "&Pixelfed";
			pixelfedToolStripMenuItem.Click += pixelfedToolStripMenuItem_Click;
			// 
			// weasylToolStripMenuItem
			// 
			weasylToolStripMenuItem.Name = "weasylToolStripMenuItem";
			weasylToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			weasylToolStripMenuItem.Text = "&Weasyl";
			weasylToolStripMenuItem.Click += weasylToolStripMenuItem_Click;
			// 
			// helpToolStripMenuItem
			// 
			helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
			helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			aboutToolStripMenuItem.Text = "&About";
			aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(284, 161);
			Controls.Add(menuStrip1);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "MainForm";
			Text = "PostSharp 5";
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem accountSetupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem furAffinityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deviantArtToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem weasylToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mastodonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pixelfedToolStripMenuItem;
	}
}

