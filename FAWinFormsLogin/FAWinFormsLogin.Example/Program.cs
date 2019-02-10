using FAWinFormsLogin.loginPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAWinFormsLogin.Example {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var f = new LoginFormFA()) {
                if (f.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show($"b={f.BCookie}\na={f.ACookie}");
                }
            }
        }
    }
}
