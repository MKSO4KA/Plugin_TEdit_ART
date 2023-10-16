using System;
using System.Windows;
using System.Windows.Controls;
using TEdit.Editor.PixelArtTools.Tools;

namespace TEdit.Editor.Plugins
{
    /// <summary>
    /// Interaction logic for ReplaceAllPlugin.xaml
    /// </summary>
    public partial class PixelArtCreatorPluginSettings : Window
    {
        public string DirectoryPath { get; private set; }
        public string PhotoPath { get; private set; }
        public string TorchPath { get; private set; }
        public PixelArtCreatorPluginSettings()
        {
            InitializeComponent();
            //NUDTextBox.Text = startvalue.ToString();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            //DirectoryPath = TilesPath_box.Text;
            PhotoPath = PhotoPath_box.Text;
            TorchPath = TorchPath_box.Text;
            Close();
        }

        private void OpenPathDialog(object sender, RoutedEventArgs e)
        {
            //TilesPath_box.Text = PathManager.OpenDataPathDialog();
        }

        private void OpenFileDialogPhoto(object sender, RoutedEventArgs e)
        {
            PhotoPath_box.Text = PathManager.OpenFilePathDialog();
        }
        private void OpenFileDialogTorch(object sender, RoutedEventArgs e)
        {
            TorchPath_box.Text = PathManager.OpenFilePathDialog();
        }
    }
}
