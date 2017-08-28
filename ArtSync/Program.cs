using ArtSourceWrapper;
using System;
using System.IO;
using System.Windows.Forms;

namespace ArtSync {
	static class Program {
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			IECookiePersist.Suppress(true);

            // Force current directory (if a file or folder was dragged onto ArtSync.exe)
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var w = args.Length > 0 && (File.Exists(args[0]) || Directory.Exists(args[0]))
                ? new LocalPathWrapper(args[0])
                : null;
            Application.Run(new WeasylForm(w));
		}
	}
}
