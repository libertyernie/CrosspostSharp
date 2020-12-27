﻿using ISchemm.WinFormsOAuth;
using SourceWrappers;
using System;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			IECookiePersist.Suppress(true);
			IECompatibility.SetForCurrentProcess();

			// Force current directory (if a file or folder was dragged onto CrosspostSharp3.exe)
			Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args.Length == 1) {
				try {
					var artwork = SavedPhotoPost.FromFile(args[0]);
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
