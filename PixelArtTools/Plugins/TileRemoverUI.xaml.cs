using System;
using System.Windows;
using System.Windows.Controls;
using TEdit.Editor.Tools;

namespace TEdit.Editor.Plugins
{
    /// <summary>
    /// Interaction logic for ReplaceAllPlugin.xaml
    /// </summary>
    public partial class TileRemoverUI : Window
    {
        public string amount { get; private set; }

        public TileRemoverUI()
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
            amount = TileCount_Box.Text;
            Close();
        }
    }
}
