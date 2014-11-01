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
        public LProgressBar() {
            InitializeComponent();

			this.Minimum = 0;
			this.Maximum = 100;

            this.DoubleBuffered = true;
        }

		[Description("The value at which the progress bar is empty."), Category("Behavior")]
		public int Minimum { get; set; }

		[Description("The value at which the progress bar is full."), Category("Behavior")] 
        public int Maximum { get; set; }

        private int _value;
		[Description("The initial value of the progress bar."), Category("Behavior")] 
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
			Console.WriteLine("barwidth " + barwidth);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, barwidth, this.Height);
        }
    }
}
