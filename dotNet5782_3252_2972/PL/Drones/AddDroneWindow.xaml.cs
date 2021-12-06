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

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        IBL.IBL myBL;
        
        public AddDroneWindow(IBL.IBL bl, bool Addition, IBL.BO.DroneToList drone)
        {
            InitializeComponent();
            myBL = bl;
            if (Addition && drone == null) { prepareForAddition(); }
            else { prepareForShow(drone); }


            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            StartingBSCB.ItemsSource = myBL.GetAllBaseStations();
        }

        private void AddDrone_click(object sender, RoutedEventArgs e)
        {
            int DroneId;
            if(!int.TryParse(DroneId_TextBox.Text, out DroneId))
            {
                MessageBox.Show("ID must be a Number!", "Wrong ID type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(DroneModel_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the drone's model", "Empty model value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(MaxWeightCB.SelectedIndex < 0)
            {
                MessageBox.Show("Please select the max weight", "Empty weight value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StartingBSCB.SelectedIndex < 0)
            {
                MessageBox.Show("Please select the Starting base station", "Empty starting base station value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                myBL.AddDrone(DroneId, DroneModel_TextBox.Text, (IBL.BO.WeightCategories)MaxWeightCB.SelectedItem, ((IBL.BO.BaseStationToList)StartingBSCB.SelectedItem).Id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            (new DroneListWindow(myBL)).Show();
            this.Close();
        }

        private void prepareForAddition()
        {
            Width = 270;
            AddDrone.Visibility = Visibility.Visible;
            DroneLocation_TextBlock.Visibility = Visibility.Collapsed;
            DroneLocation_TextBox.Visibility = Visibility.Collapsed;
            DroneBattery_TextBlock.Visibility = Visibility.Collapsed;
            DroneBattery_Data.Visibility = Visibility.Collapsed;
            DroneParcel_TextBlock.Visibility = Visibility.Collapsed;
            DroneParcel_Data.Visibility = Visibility.Collapsed;
        }

        private void prepareForShow(IBL.BO.DroneToList DTL)
        {
            Width = 420;
            AddDrone.Visibility = Visibility.Collapsed;
            AddDrone.IsEnabled = false;
            StartingBSCB.Visibility = Visibility.Collapsed;
            StartingBS_TextBlock.Visibility = Visibility.Collapsed;

            IBL.BO.Drone drone = myBL.GetDrone(DTL.Id);
            DroneId_TextBox.Text = drone.Id.ToString();
            DroneId_TextBox.IsEnabled = false;
            DroneModel_TextBox.Text = drone.Model;
            DroneModel_TextBox.IsEnabled = false;
            MaxWeightCB.SelectedIndex = (int)drone.MaxWeight;
            MaxWeightCB.IsEnabled = false;
            DroneLocation_TextBox.Text = drone.CurrentLocation.ToString();
            DroneBattery_Data.Text = drone.Battery.ToString();
            if(drone.CurrentParcel == null)
            {
                DroneParcel_TextBlock.Visibility = Visibility.Collapsed;
                DroneParcel_Data.Visibility = Visibility.Collapsed;
            }
            else
            {
                DroneParcel_Data.Text = drone.CurrentParcel.Id.ToString();
            }
        }
    }
}
