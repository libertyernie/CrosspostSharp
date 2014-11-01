using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class LProgressBar : UserControl
    {
        public LProgressBar()
        {
            InitializeComponent();
            this.Click += (o, e) =>
            {
                this.Refresh();
            };
            this.DoubleBuffered = true;
        }

		[Description("The value at which the progress bar is full."), Category("Behavior")] 
        public int Maximum { get; set; }

        private int _value;
		[Description("The initial value of the progress bar."), Category("Behavior")] 
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
				this.Invalidate();
            }
        }

		public void setVisible_ThreadSafe(bool visible) {
			if (this.InvokeRequired)
				this.BeginInvoke(new Action<bool>(setVisible_ThreadSafe), visible);
			else
				this.Visible = visible;
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int barwidth = Value * this.Width / Math.Max(Maximum, 1);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, barwidth, this.Height);
        }
    }
}
