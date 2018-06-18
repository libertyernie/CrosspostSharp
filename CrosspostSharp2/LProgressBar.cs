using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp
{
	public partial class LProgressBar : UserControl, IProgress<double>
	{
		private double _value;

		public LProgressBar() {
			InitializeComponent();
            
			this.ForeColor = Color.Black;

			this.DoubleBuffered = true;
		}

        public void Report(double value) {
            _value = value;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			int barwidth = (int)(_value * this.Width);
			using (Brush brush = new SolidBrush(ForeColor)) {
				e.Graphics.FillRectangle(brush, 0, 0, barwidth, this.Height);
			}
		}
	}
}
