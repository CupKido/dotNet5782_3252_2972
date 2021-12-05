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
    /// Interaction logic for ShowDroneWindow.xaml
    /// </summary>
    public partial class ShowDroneWindow : Window
    {
        IBL.IBL myBL;
        public ShowDroneWindow(IBL.BO.DroneToList droneToList, IBL.IBL bl)
        {
            InitializeComponent();
            myBL = bl;
            IBL.BO.Drone drone = myBL.GetDrone(droneToList.Id);
            DroneId_TextBox.Text = drone.Id.ToString();
            DroneModel_TextBox.Text = drone.Model;
            DroneMaxWeight_TextBox.Text = drone.MaxWeight.ToString();
            DroneLocation_TextBox.Text = drone.CurrentLocation.ToString();
            DroneBattery_Data.Text = drone.Battery.ToString();
        }
    }
}
