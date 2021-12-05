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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL myBL;
        
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            myBL = bl;
            resetDronesList();
            resetComboBoxes();
            MaxWeightBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            DroneStatusBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
        }

        private void setMaxWeight_select(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (MaxWeightBox.SelectedIndex < 0) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)MaxWeightBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.MaxWeight == selectedWeight
                                    select d;
            if (DroneStatusBox.SelectedIndex < 0) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)DroneStatusBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.Status == selectedStatus
                                    select d;
        }

        private void setDroneStatus_select(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (DroneStatusBox.SelectedIndex < 0) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)DroneStatusBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.Status == selectedStatus
                                    select d;

            if (MaxWeightBox.SelectedIndex < 0) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)MaxWeightBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.MaxWeight == selectedWeight
                                    select d;
        }

        private void resetDronesList_click(object sender, RoutedEventArgs e)
        {
            resetDronesList();
            resetComboBoxes();
        }

        private void addDrone_click(object sender, RoutedEventArgs e)
        {
            AddDroneWindow ADW = new AddDroneWindow(myBL);
            ADW.Show();
            this.Close();
        }

        private void closeWindow_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void resetDronesList()
        {
            DroneList.ItemsSource = myBL.GetAllDrones();
        }

        private void resetComboBoxes()
        {
            MaxWeightBox.SelectedIndex = -1;
            MaxWeightBox.Text = "Select Weight";
            DroneStatusBox.SelectedIndex = -1;
            DroneStatusBox.Text = "Select Status";
        }

        private void DroneList_Selected(object sender, RoutedEventArgs e)
        {
            (new ShowDroneWindow((IBL.BO.DroneToList)DroneList.SelectedItem, myBL)).Show();
        }
    }
}
