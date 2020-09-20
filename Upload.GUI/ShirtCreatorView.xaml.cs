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
using System.Windows.Shapes;

namespace Upload.GUI
{
    /// <summary>
    /// Interaction logic for ShirtCreatorView.xaml
    /// </summary>
    public partial class ShirtCreatorView : Window
    {
        public ShirtCreatorView()
        {
            InitializeComponent();
        }

        private void Btn_ToExcel_Click(object sender, RoutedEventArgs e)
        {
            FromExcel.Visibility = Visibility.Visible;
            ToExcel.Visibility = Visibility.Collapsed;
        }

        private void Btn_FromExcel_Click(object sender, RoutedEventArgs e)
        {
            ToExcel.Visibility = Visibility.Visible;
            FromExcel.Visibility = Visibility.Collapsed;
        }
    }
}
