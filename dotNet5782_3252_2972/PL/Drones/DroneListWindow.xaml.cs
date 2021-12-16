using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        BlApi.IBL myBL;
        bool disallowClosure = true;

        public DroneListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            myBL = bl;
            resetDronesList();
            resetComboBoxes();
            MaxWeightBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            DroneStatusBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
        }

        private void setMaxWeight_select(object sender, SelectionChangedEventArgs e)
        {
            filterDroneList();
        }

        private void setDroneStatus_select(object sender, SelectionChangedEventArgs e)
        {
            filterDroneList();
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
            ADW.Closed += (s, e) =>
            {
                filterDroneList();
                DroneList.Items.Refresh();
            };
        }

        private void closeWindow_click(object sender, RoutedEventArgs e)
        {
            disallowClosure = false;
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
            AddDroneWindow ADW = new AddDroneWindow(myBL, (BO.DroneToList)DroneList.SelectedItem);
            ADW.Show();
            ADW.Closed += (s, e) =>
            {
                filterDroneList();
                DroneList.Items.Refresh();
            };
        }

        private void filterDroneList()
        {
            DroneList.ItemsSource = myBL.GetAllDrones();
            if (DroneStatusBox.SelectedIndex >= 0)
            {
                BO.DroneStatuses selectedStatus = (BO.DroneStatuses)DroneStatusBox.SelectedItem;
                DroneList.ItemsSource = ((IEnumerable<BO.DroneToList>)DroneList.ItemsSource).Where(d => d.Status == selectedStatus);
            }

            if (MaxWeightBox.SelectedIndex >= 0)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)MaxWeightBox.SelectedItem;
                DroneList.ItemsSource = ((IEnumerable<BO.DroneToList>)DroneList.ItemsSource).Where(d => d.MaxWeight == selectedWeight);
            }
        }
       
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = disallowClosure;
        }

        private void DroneList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
