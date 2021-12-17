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
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        public MainWindow()
        {
            InitializeComponent();
            this.Show();
        }
        public void showDroneList_click(object sender, RoutedEventArgs e)
        {
           
            DroneListWindow DLW = new DroneListWindow(myBL);
            DLW.Show();
            
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
