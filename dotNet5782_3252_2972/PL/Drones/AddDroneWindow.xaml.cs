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
using System.ComponentModel;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;

        public AddDroneWindow()
        {

            InitializeComponent();
            
            
            prepareForAddition();
        }


        public AddDroneWindow(int DroneId)
        {
            InitializeComponent();
            
            prepareForShow(myBL.GetDrone(DroneId));
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
                myBL.AddDrone(DroneId, DroneModel_TextBox.Text, (BO.WeightCategories)MaxWeightCB.SelectedItem, (int)StartingBSCB.SelectedItem);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }

        private void prepareForAddition()
        {
            AdditionChBox.IsChecked = false;
            Width = 300;
            AddDrone_Button.Visibility = Visibility.Visible;

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            StartingBSCB.ItemsSource = from BO.BaseStationToList BS in myBL.GetAllBaseStations()
                                       select BS.Id;
        }

        private void prepareForShow(BO.Drone drone)
        {
            AdditionChBox.IsChecked = true;
            Width = 420;
            AddDrone_Button.Visibility = Visibility.Collapsed;
            
            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

            this.DataContext = drone;
            BO.DroneStatuses status = drone.Status;
            
            if ((int)status == 0)
            {
                PickUp_Button.Visibility = Visibility.Collapsed;
                Supply_Button.Visibility = Visibility.Collapsed;
                TimeInCharge_TextBox.Visibility = Visibility.Collapsed;
                DisCharge_Button.Visibility = Visibility.Collapsed;
            }
            if ((int)status == 1)
            {
                Attribution_Button.Visibility = Visibility.Collapsed;
                PickUp_Button.Visibility = Visibility.Collapsed;
                Supply_Button.Visibility = Visibility.Collapsed;
                Charge_Button.Visibility = Visibility.Collapsed;
            }
            if ((int)status == 2)
            {
                int parcelId = (int)drone.CurrentParcel.Id;
                BO.Parcel p = myBL.GetParcel(parcelId);
                TimeInCharge_TextBox.Visibility = Visibility.Collapsed;
                DisCharge_Button.Visibility = Visibility.Collapsed;
                Attribution_Button.Visibility = Visibility.Collapsed;
                Charge_Button.Visibility = Visibility.Collapsed;
                if (p.PickedUp==null || p.PickedUp ==DateTime.MinValue )
                {
                    Supply_Button.Visibility = Visibility.Collapsed;
                }
                else 
                {
                    PickUp_Button.Visibility = Visibility.Collapsed;
                }

            }

            DroneId_TextBox.Text = drone.Id.ToString();
            DroneId_TextBox.IsEnabled = false;

            DroneModel_TextBox.Text = drone.Model;
            DroneModel_TextBox.IsEnabled = true;

            MaxWeightCB.SelectedIndex = (int)drone.MaxWeight;
            MaxWeightCB.IsEnabled = false;

            DroneLocation_TextBox.Text = drone.CurrentLocation.ToString();


            if(drone.CurrentParcel.Id != 0)
            {
                DroneParcel_Data.DataContext = drone.CurrentParcel;
            }
            else
            {
                DroneParcel_Data.Text = "none";
                ShowCarriedParcel_Button.IsEnabled = false;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            string DroneModel = DroneModel_TextBox.Text;
            if (DroneModel_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the drone's model", "Empty model value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                myBL.UpdateDrone(DroneId, DroneModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DeleteDrone(int.Parse(DroneId_TextBox.Text));
            }
            catch
            {
                //TODO
            }
            disallowClosure = false;
            this.Close();
        }

        private void Charge_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            try
            {
                myBL.ChargeDrone(DroneId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Charge_Button.Visibility = Visibility.Collapsed;
            DisCharge_Button.Visibility = Visibility.Visible;
            TimeInCharge_TextBox.Visibility = Visibility.Visible;

        }

        private void DisCharge_Click(object sender, RoutedEventArgs e)
        {
            
            int DroneId = int.Parse(DroneId_TextBox.Text);
            if (TimeInCharge_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the time drone " + DroneId +" was in charge", "Empty Time in charge", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            float time = float.Parse(TimeInCharge_TextBox.Text);
            try
            {
                myBL.DisChargeDrone(DroneId,time);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Charge_Button.Visibility = Visibility.Visible;
            DisCharge_Button.Visibility = Visibility.Collapsed;
            TimeInCharge_TextBox.Visibility = Visibility.Collapsed;
            TimeInCharge_TextBox.Text = "";
        }

        private void Supply_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            try
            {
                myBL.SupplyParcel(DroneId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }

        private void Attribution_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            try
            {
                myBL.AttributionParcelToDrone(DroneId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }

        private void PickUp_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            try
            {
                myBL.PickUpParcelByDrone(DroneId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }

        private void MaxWeightCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            disallowClosure = false;
            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = disallowClosure;
        }

        private void ShowCarriedParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            Parcel.ShowParcelWindow SPW = new Parcel.ShowParcelWindow(int.Parse(DroneParcel_Data.Text));
            SPW.Show();
            disallowClosure = false;
            this.Close();
        }
    }
}
