using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Upload.GUI
{
    /// <summary>
    /// Interaction logic for MainUploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : Window
    {
        public UploadWindow()
        {
            InitializeComponent();
        }

        private void button_add_folder_Click(object sender, RoutedEventArgs e)
        {
            UIElement gridAddFolder = FindName("grid_add_folder") as UIElement;
            if (gridAddFolder != null)
            {
                gridAddFolder.Visibility = Visibility.Visible;
            }
        }

        private void button_close_folder_Click(object sender, RoutedEventArgs e)
        {
            UIElement gridAddFolder = FindName("grid_add_folder") as UIElement;
            if (gridAddFolder != null)
            {
                gridAddFolder.Visibility = Visibility.Collapsed;
            }
        }
    }
}
