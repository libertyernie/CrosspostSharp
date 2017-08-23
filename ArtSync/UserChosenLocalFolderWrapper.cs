using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
    public class UserChosenLocalFolderWrapper : LocalDirectoryWrapper {
        public Form Parent { get; set; }

        private static FolderBrowserDialog DIALOG = new FolderBrowserDialog {
            SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };

        public override string SelectDirectory() {
            if (DIALOG.ShowDialog(Parent) == DialogResult.OK) {
                return DIALOG.SelectedPath;
            } else {
                return null;
            }
        }
    }
}
