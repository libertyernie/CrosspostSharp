using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync
{
	public partial class LProgressBar : UserControl, IProgress<double>
	{
		private double _value;

		public LProgressBar() {
			InitializeComponent();
            
			this.ForeColor = Color.Black;

			this.DoubleBuffered = true;
		}

		[Description("Determines whether the control is visible or hidden. This property is thread-safe."), Category("Behavior")]
		public new bool Visible {
			get {
				return base.Visible;
			}
			set {
				if (this.InvokeRequired) {
					this.BeginInvoke(value ? new Action(Show) : new Action(Hide));
				} else {
					base.Visible = value;
				}
			}
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
