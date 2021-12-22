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
using System.Collections.ObjectModel;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;
        private ObservableCollection<BO.DroneToList> _dronesCollection = new ObservableCollection<BO.DroneToList>();
        public ObservableCollection<BO.DroneToList> DronesCollection
        {
            get
            {
                return _dronesCollection;
            }
            set
            {
                _dronesCollection = value;
                
            }
        }


        public DroneListWindow()
        {
            InitializeComponent();
            resetDronesList();
            DroneList.DataContext = _dronesCollection;
            MaxWeightBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            DroneStatusBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            resetComboBoxes();
            this.DataContext = this;
            //DroneList.ItemsSource = DronesCollection;

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
                resetDronesList();
                filterDroneList();
                
            };
        }

        private void closeWindow_click(object sender, RoutedEventArgs e)
        {
            disallowClosure = false;
            this.Close();
        }

        private void resetDronesList()
        {
            if(DronesCollection != null)
            {
                DronesCollection.Clear();
            }
            myBL.GetAllDrones().Distinct().ToList().ForEach(i => DronesCollection.Add(i));
            //foreach (BO.DroneToList DTL in myBL.GetAllDrones())
            //{
            //    DronesCollection.Add(DTL);
            //}
            //DronesCollection = new ObservableCollection<BO.DroneToList>(myBL.GetAllDrones());
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
            ADW.Closed += (s, e) =>
            {
                filterDroneList();
                resetDronesList();
            };
            ADW.Show();
            
        }

        private void filterDroneList()
        {
            resetDronesList();
            if (DroneStatusBox.SelectedIndex >= 0)
            {
                BO.DroneStatuses selectedStatus = (BO.DroneStatuses)DroneStatusBox.SelectedItem;
                insertToDroneCollection((new ObservableCollection<BO.DroneToList>(DronesCollection.Where(d => d.Status == selectedStatus))));
            }

            if (MaxWeightBox.SelectedIndex >= 0)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)MaxWeightBox.SelectedItem;
                insertToDroneCollection(new ObservableCollection<BO.DroneToList>(DronesCollection.Where(d => d.MaxWeight == selectedWeight)));
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

        private void insertToDroneCollection(IEnumerable<BO.DroneToList> enu)
        {
            DronesCollection.Clear();
            enu.Distinct().ToList().ForEach(i => DronesCollection.Add(i));
        }
    }
    
}
