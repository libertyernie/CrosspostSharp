using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeasylSync
{
    public partial class LProgressBar : UserControl
    {
		private int _value, _minimum, _maximum;

        public LProgressBar() {
            InitializeComponent();

			this.Minimum = 0;
			this.Maximum = 100;
			this.Value = 0;
			this.ForeColor = Color.Black;

            this.DoubleBuffered = true;
        }

		[Description("The value at which the progress bar is empty. This property is thread-safe."), Category("Behavior")]
		public int Minimum {
			get {
				return _minimum;
			}
			set {
				_minimum = value;
				Invalidate();
			}
		}

		[Description("The value at which the progress bar is full. This property is thread-safe."), Category("Behavior")]
		public int Maximum {
			get {
				return _maximum;
			}
			set {
				_maximum = value;
				Invalidate();
			}
		}

		[Description("The initial value of the progress bar. This property is thread-safe."), Category("Behavior")] 
        public int Value {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
				Invalidate();
            }
        }

		/*private Color _color;
		[Description("The color of the progress bar. This property is thread-safe."), Category("Behavior")]
		public Color ForeColor {
			get {
				return _color;
			}
			set {
				_color = value;
				Invalidate();
			}
		}*/

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

        protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
            int barwidth = (Value - Minimum) * this.Width / Math.Max(Maximum - Minimum, 1);
			using (Brush brush = new SolidBrush(ForeColor)) {
				e.Graphics.FillRectangle(brush, 0, 0, barwidth, this.Height);
			}
        }
    }
}
