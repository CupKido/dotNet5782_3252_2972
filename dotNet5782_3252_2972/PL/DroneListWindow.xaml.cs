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
        IBL.IBL myBL = new BLobject.BL();
        public DroneListWindow()
        {

            InitializeComponent();
            resetDronesList();
            resetComboBoxes();
            MaxWeightBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            DroneStatusBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
        }

        public void setMaxWeight_select(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (MaxWeightBox.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)MaxWeightBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.MaxWeight == selectedWeight
                                    select d;
            if (DroneStatusBox.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)DroneStatusBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.Status == selectedStatus
                                    select d;
        }

        public void setDroneStatus_select(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (DroneStatusBox.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)DroneStatusBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.Status == selectedStatus
                                    select d;
            if (MaxWeightBox.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)MaxWeightBox.SelectedItem;
            DroneList.ItemsSource = from IBL.BO.DroneToList d in DroneList.ItemsSource
                                    where d.MaxWeight == selectedWeight
                                    select d;
        }

        public void resetDronesList_click(object sender, RoutedEventArgs e)
        {
            resetDronesList();
            resetComboBoxes();
        }

        private void resetDronesList()
        {
            DroneList.ItemsSource = myBL.GetAllDrones();
        }

        private void resetComboBoxes()
        {
            MaxWeightBox.SelectedItem = null;
            DroneStatusBox.SelectedItem = null;
        }
    }
}
