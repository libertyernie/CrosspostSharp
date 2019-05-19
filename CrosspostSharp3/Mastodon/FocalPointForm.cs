using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class FocalPointForm : Form {
		public (double, double) FocalPoint { get; set; } = (0.0, 0.0);

		public FocalPointForm(Image image) {
			InitializeComponent();
			pictureBox1.Image = image;

			if (image.Width > image.Height) {
				double newHeight = pictureBox1.Width * image.Height / (1.0 * image.Width);
				double diff = pictureBox1.Height - newHeight;
				Height -= (int)diff;
			} else if (image.Width < image.Height) {
				double newWidth = pictureBox1.Height * image.Width / (1.0 * image.Height);
				double diff = pictureBox1.Width - newWidth;
				Width -= (int)diff;
			}
		}

		private void PictureBox1_Click(object sender, EventArgs e) {
			var point = pictureBox1.PointToClient(Cursor.Position);
			double newX = 2.0 * point.X / (1.0 * pictureBox1.Width) - 1;
			double newY = -2.0 * point.Y / (1.0 * pictureBox1.Height) + 1;
			FocalPoint = (newX, newY);
			DialogResult = DialogResult.OK;
		}
	}
}
