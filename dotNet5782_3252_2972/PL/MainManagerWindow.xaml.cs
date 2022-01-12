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
using PL.Customers;
using PL.Parcel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public Window a = new Window();
        public MainManagerWindow()
        {
            InitializeComponent();
            DroneListPage DLP = new DroneListPage();
            Main.Content = DLP;
            a.Width = 200;
            a.Height = 40;
            a.WindowStartupLocation = WindowStartupLocation.Manual;
            a.LocationChanged += (s, e) =>
            {
                this.Left = a.Left;
                this.Top = a.Top + 40;
            };
            a.Closed += (s, e) =>
            {
                this.Close();
            };
            
            this.Show();
            a.Show();
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
            List<AddDroneWindow> Dronewindows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(AddDroneWindow)
                                            select (AddDroneWindow)w).ToList();
            List<ShowParcelWindow> Parcelwindows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(ShowParcelWindow)
                                            select (ShowParcelWindow)w).ToList();
            List<ShowBaseStationWindow> BSwindows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(ShowBaseStationWindow)
                                            select (ShowBaseStationWindow)w).ToList();
            List<ShowCustomerWindow> Customerwindows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(ShowCustomerWindow)
                                            select (ShowCustomerWindow)w).ToList();
            foreach(AddDroneWindow adw in Dronewindows)
            {
                adw.closeWindow();
            }
            foreach(ShowParcelWindow spw in Parcelwindows)
            {
                spw.CloseWindow();
            }
            foreach(ShowBaseStationWindow sbsw in BSwindows)
            {
                sbsw.CloseWindow();
            }
            foreach(ShowCustomerWindow scw in Customerwindows)
            {
                scw.CloseWindow();
            }
            foreach(Window w in App.Current.Windows)
            {
                w.Close();
            }
            this.Close();
        }

        private void ShowBaseStations_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            Main.Content = new BaseStationListPage();
        }
    }
}
