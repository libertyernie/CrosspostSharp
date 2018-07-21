using ISchemm.WinFormsOAuth;
using SourceWrappers;
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
		static void Main(string[] args) {
			ExceptionHandler.SwallowWebExceptions = false;
			TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

			IECookiePersist.Suppress(true);
			IECompatibility.SetForCurrentProcess();

			// Force current directory (if a file or folder was dragged onto CrosspostSharp3.exe)
			Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args.Length == 1) {
				try {
					var artwork = PostConverter.FromFile(args[0]);
					if (artwork.data == null) {
						throw new Exception("This file does not contain a base-64 encoded \"data\" field.");
					}
					Application.Run(new ArtworkForm(artwork));
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				Application.Run(new MainForm());
			}
		}
	}
}
