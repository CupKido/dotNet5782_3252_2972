﻿using System;
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

        public AddDroneWindow(IBL.IBL bl)
        {
            InitializeComponent();
            myBL = bl;
            
            prepareForAddition();
        }


        public AddDroneWindow(IBL.IBL bl, IBL.BO.DroneToList drone)
        {
            InitializeComponent();
            myBL = bl;
            
            prepareForShow(drone);
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
                myBL.AddDrone(DroneId, DroneModel_TextBox.Text, (IBL.BO.WeightCategories)MaxWeightCB.SelectedItem, (int)StartingBSCB.SelectedItem);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Close();
        }

        private void prepareForAddition()
        {
            Width = 270;
            AddDrone_Button.Visibility = Visibility.Visible;
            Attribution_Button.Visibility = Visibility.Collapsed;
            PickUp_Button.Visibility = Visibility.Collapsed;
            Supply_Button.Visibility = Visibility.Collapsed;
            Update_Button.Visibility = Visibility.Collapsed;
            Charge_Button.Visibility = Visibility.Collapsed;
            DisCharge_Button.Visibility = Visibility.Collapsed;
            DroneLocation_TextBlock.Visibility = Visibility.Collapsed;
            DroneLocation_TextBox.Visibility = Visibility.Collapsed;
            DroneBattery_TextBlock.Visibility = Visibility.Collapsed;
            DroneBattery_Data.Visibility = Visibility.Collapsed;
            DroneParcel_TextBlock.Visibility = Visibility.Collapsed;
            DroneParcel_Data.Visibility = Visibility.Collapsed;

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            StartingBSCB.ItemsSource = from IBL.BO.BaseStationToList BS in myBL.GetAllBaseStations()
                                       select BS.Id;
        }

        private void prepareForShow(IBL.BO.DroneToList DTL)
        {
            Width = 420;
            AddDrone_Button.IsEnabled = false;
            AddDrone_Button.Visibility = Visibility.Collapsed;
            StartingBSCB.Visibility = Visibility.Collapsed;
            StartingBS_TextBlock.Visibility = Visibility.Collapsed;

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));

            IBL.BO.Drone drone = myBL.GetDrone(DTL.Id);

            IBL.BO.DroneStatuses status = drone.Status;
            //int parcelId = drone.CurrentParcel.Id;
           // IBL.BO.Parcel p = myBL.GetParcel(parcelId);
            
            if ((int)status == 0)
            {
                PickUp_Button.Visibility = Visibility.Collapsed;
                Supply_Button.Visibility = Visibility.Collapsed;
            }
            if ((int)status == 1)
            {
                Attribution_Button.Visibility = Visibility.Collapsed;
                PickUp_Button.Visibility = Visibility.Collapsed;
                Supply_Button.Visibility = Visibility.Collapsed;
            }
            if ((int)status == 2)
            {
                int parcelId = drone.CurrentParcel.Id;
                IBL.BO.Parcel p = myBL.GetParcel(parcelId);
                Attribution_Button.Visibility = Visibility.Collapsed;
                if(p.PickedUp==null || p.PickedUp ==DateTime.MinValue )
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
            this.Close();

        }

        private void DisCharge_Click(object sender, RoutedEventArgs e)
        {
            int DroneId = int.Parse(DroneId_TextBox.Text);
            try
            {
                myBL.DisChargeDrone(DroneId,100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Close();

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
            this.Close();
        }
    }
}