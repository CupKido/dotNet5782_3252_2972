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
using System.ComponentModel;

namespace PL.BaseStation
{
    /// <summary>
    /// Interaction logic for ShowBaseStationWindow.xaml
    /// </summary>
    public partial class ShowBaseStationWindow : Window
    {

        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;
        public ShowBaseStationWindow()
        {
            InitializeComponent();
            Width = 300;
        }
        public ShowBaseStationWindow(int BaseStationId)
        {
            InitializeComponent();
            BO.BaseStationToList Bs = myBL.TurnBaseStationToList(myBL.GetBaseStation(BaseStationId));
            this.DataContext = Bs;
            IfPresentation.IsChecked = true;
            
            // ArrivingParcelsList.ItemsSource = c.ToThisCustomer;
            // GoingParcelsList.ItemsSource = c.FromThisCustomer;
        }

        private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = disallowClosure;
        }
        private void AddBaseStation_Button_Click(object sender, RoutedEventArgs e)
        {
            int BsId , numOfCharges;
            double latitude, longitude;
            BO.Location location = new BO.Location();
            
            if (!int.TryParse(BaseStationId_TextBox.Text, out BsId))
            {
                MessageBox.Show("ID must be a Number!", "Wrong ID type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BaseStationName_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the Base Station's name", "Empty name value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!double.TryParse(BaseStationLongitude_TextBox.Text, out longitude))
            {
                MessageBox.Show("Longitude must be a Number!", "Wrong Longitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(BaseStationLatitude_TextBox.Text, out latitude))
            {
                MessageBox.Show("LAtitude must be a Number!", "Wrong LAtitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(BaseStationChargeSlots_TextBox.Text, out numOfCharges))
            {
                MessageBox.Show("Number of charges must be a Number!", "Wrong Number of charges type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            location.Latitude = latitude;
            location.Longitude = longitude;
            try
            {
                myBL.AddBaseStations(BsId, BaseStationName_TextBlock.Text, location, numOfCharges);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CloseWindow();
        }
        public void CloseWindow()
        {
            disallowClosure = false;
            this.Close();
        }

      
    }
}
