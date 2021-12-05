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
        
        public AddDroneWindow(IBL.IBL bl)
        {
            InitializeComponent();
            
            myBL = bl;
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
    }
}
