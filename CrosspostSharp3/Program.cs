using ISchemm.WinFormsOAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;

namespace CrosspostSharp3 {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			ExceptionHandler.SwallowWebExceptions = false;
			TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

			IECookiePersist.Suppress(true);
			IECompatibility.SetForCurrentProcess();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
