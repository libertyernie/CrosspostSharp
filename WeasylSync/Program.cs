using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsWebBrowserOAuth;

namespace WeasylSync {
	static class Program {
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			IECookiePersist.Suppress(true);
			Application.Run(new WeasylForm());
		}
	}
}
