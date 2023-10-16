using System;
using System.Windows;
using System.Windows.Controls;
using TEdit.Editor.PixelArtTools.Tools;

namespace TEdit.Editor.Plugins
{
    /// <summary>
    /// Interaction logic for ReplaceAllPlugin.xaml
    /// </summary>
    public partial class PaletteCreatorPluginSettings : Window
    {
        //public string DirectoryPath { get; private set; }
        public string ExceptionsPath { get; private set; }
        public bool? UsersChouseExc { get; private set; } = false;
        public bool? UsersChouseTrch { get; private set; } = false;
        public bool? InvertExeptions { get; private set; } = false;
        public bool? InvertTorchs { get; private set; } = false;
        public string TorchsPath { get; private set; }
        //PathManager = new PathManager;
        public PaletteCreatorPluginSettings()
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
            ExceptionsPath = (ExceptionsPath_box.Text != null) ? ExceptionsPath_box.Text : "";
            TorchsPath = (TorchsPath_box.Text != null) ? TorchsPath_box.Text : "";
            UsersChouseExc = ExcCheck_box.IsChecked;
            UsersChouseTrch = TorCheck_box.IsChecked;
            //InvertTorchs = InvertCheck_box2.IsChecked;
            InvertExeptions = InvertCheck_box1.IsChecked;
            //TorchPath = TorchPath_box.Text;
            Close();
        }

        private void OpenPathDialog(object sender, RoutedEventArgs e)
        {
            //TilesPath_box.Text = PathManager.OpenDataPathDialog();
        }

        private void OpenFileDialogExcept(object sender, RoutedEventArgs e)
        {
            ExceptionsPath_box.Text = PathManager.OpenFilePathDialog();
        }
        private void OpenFileDialogTorch(object sender, RoutedEventArgs e)
        {
            TorchsPath_box.Text = PathManager.OpenFilePathDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
