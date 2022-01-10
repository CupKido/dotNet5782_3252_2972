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
using BlApi;
using PL.BaseStation;
using PL.Parcel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public MainManagerWindow()
        {
            InitializeComponent();
            //if(App.Current.)
            DroneListPage DLP = new DroneListPage();
            Main.Content = DLP;
            this.Show();

        }
        public void showDroneList_click(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            Main.Content = new DroneListPage();
        }

        private void showCustomersList_click(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            Main.Content = new CustomersListPage();
        }

        private void showParcelsList_click(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            Main.Content = new ParcelListPage();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowBaseStations_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            Main.Content = new BaseStationListPage();
        }
    }
}
