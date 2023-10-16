using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TEdit.Editor.PixelArtTools.Tools
{
    internal class PathManager
    {
        public static string OpenFilePathDialog()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Pixel Art default file|*.txt";
            ofd.DefaultExt = "Pixel Art default file|*.txt";
            ofd.Title = "Import TXT file with tiles nd paint";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(ofd.InitialDirectory)) { Directory.CreateDirectory(ofd.InitialDirectory); }
            ofd.Multiselect = false;
            if ((bool)ofd.ShowDialog())
            {
                string filename = System.IO.Path.GetFullPath(ofd.FileName);
                // MessageBox.Show(filename, "debug for me", MessageBoxButton.OK, MessageBoxImage.Error);
                return filename;
            }
            else
            {
                string filename = null;
                return filename;
            }
        }
        public static string OpenDataPathDialog()
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string filename = System.IO.Path.GetFullPath(fbd.SelectedPath);
                // MessageBox.Show(filename, "debug for me", MessageBoxButton.OK, MessageBoxImage.Error);
                return filename;
            }
            else
            {
                string filename = null;
                return filename;
            }
        }
    }
}
