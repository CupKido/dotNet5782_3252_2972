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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BlApi;
using PL.Parcel;

namespace PL
{

    public partial class DroneListPage : Page
    {
        BlApi.IBL myBL = BlFactory.GetBL();
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

        public DroneListPage()
        {
            InitializeComponent();
            resetDronesList();
            DroneList.DataContext = _dronesCollection;
            MaxWeightBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            DroneStatusBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            resetComboBoxes();
            this.DataContext = this;
            DronesCollection.CollectionChanged += (s, e) =>
            {
                DroneList.DataContext = DronesCollection.OrderBy(d => d.Id);
            };
            syncWithSimulator();
            
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

        private void deleteSelected_click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DeleteDrone(((BO.DroneToList)DroneList.SelectedItem).Id);
            }
            catch
            {
                //TODO
            }
            resetDronesList();
            filterDroneList();
            
        }

        private void addDrone_click(object sender, RoutedEventArgs e)
        {
            AddDroneWindow ADW = new AddDroneWindow();
            ADW.Show();
            ADW.AddDrone_Button.Click += (s, e) =>
            {
                int Id;
                if (!int.TryParse(ADW.DroneId_TextBox.Text, out Id)) return;
                if (DronesCollection.FirstOrDefault(d=> d.Id == Id) == null)
                {
                    try
                    {
                    BO.Drone d = myBL.GetDrone(Id);
                    DronesCollection.Add(myBL.TurnDroneToList(d));
                    }
                    catch
                    {
                        return;
                    }
                }
                filterDroneList();
            };
            //ADW.Closed += (s, e) =>
            //{
            //    resetDronesList();
            //    filterDroneList();

            //};
        }

        //private void closeWindow_click(object sender, RoutedEventArgs e)
        //{
        //    disallowClosure = false;
        //    this.Close();
        //}

        private void resetDronesList()
        {
            if (DronesCollection != null)
            {
                DronesCollection.Clear();
            }
            myBL.GetAllDrones().Distinct().ToList().ForEach(i => DronesCollection.Add(i));
        }

        private void restartDronesList()
        {
            if (DronesCollection != null)
            {
                DronesCollection.Clear();
            }
            myBL.GetAllDrones().Distinct().ToList().ForEach(i => DronesCollection.Add(i));
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
            List<AddDroneWindow> windows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(AddDroneWindow)
                                            select (AddDroneWindow)w).ToList();
            int Id = (DroneList.SelectedItem as BO.DroneToList).Id;
            foreach (AddDroneWindow adw in windows)
            {
                if(adw.DroneId_TextBox.Text == Id.ToString())
                {
                    MessageBox.Show($"Window for drone number {Id} is alreay open","Window is already open", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            if(DroneList.SelectedItem is null)
            {
                return;
            }
            AddDroneWindow ADW = new AddDroneWindow(Id);
            ADW.Update_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.DisCharge_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.Charge_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.Attribution_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.PickUp_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.Supply_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                updateSpecificDrone(Id);
            };

            ADW.Delete_Button.Click += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                DronesCollection.Remove(DronesCollection.First(d => d.Id == Id));
            };

            ADW.SimulatorWorker.ProgressChanged += (s, e) =>
            {
                int Id = int.Parse(ADW.DroneId_TextBox.Text);
                try
                {
                    updateSpecificDrone(Id);
                }
                catch
                {
                    try
                    {
                        DronesCollection.Remove(DronesCollection.First(d => d.Id == Id));
                    }
                    catch
                    {
                        resetDronesList();
                    }
                }
            };
            
            ADW.Show();

        }

        private void updateSpecificDrone(int Id)
        {
            
            DronesCollection.Remove(DronesCollection.First(d => d.Id == Id));
            BO.DroneToList droneToList = myBL.TurnDroneToList(myBL.GetDrone(Id));
            if (filterSingleDrone(droneToList))
            {
                DronesCollection.Add(myBL.TurnDroneToList(myBL.GetDrone(Id)));
            }
            
            
            //filterDroneList();
        }

        private bool filterSingleDrone(BO.DroneToList drone)
        {
            if (DroneStatusBox.SelectedIndex >= 0)
            {
                BO.DroneStatuses selectedStatus = (BO.DroneStatuses)DroneStatusBox.SelectedItem;
                if (selectedStatus != drone.Status) return false;
            }
            if (MaxWeightBox.SelectedIndex >= 0)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)MaxWeightBox.SelectedItem;
                if (selectedWeight != drone.MaxWeight) return false;
            }
            return true;
        }

        private void filterDroneList()
        {
            List<BO.DroneToList> filterList = myBL.GetAllDrones().ToList();
            if (DroneStatusBox.SelectedIndex >= 0)
            {
                BO.DroneStatuses selectedStatus = (BO.DroneStatuses)DroneStatusBox.SelectedItem;
                filterList = filterList.Where(d => d.Status == selectedStatus).ToList();
            }

            if (MaxWeightBox.SelectedIndex >= 0)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)MaxWeightBox.SelectedItem;
                filterList = filterList.Where(d => d.MaxWeight == selectedWeight).ToList();
            }
            insertToDroneCollection(filterList);
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    e.Cancel = disallowClosure;
        //}
        private void insertToDroneCollection(IEnumerable<BO.DroneToList> enu)
        {
            DronesCollection.Clear();
            enu.Distinct().ToList().ForEach(i => DronesCollection.Add(i));
        }

        private void syncWithSimulator()
        {
            List<AddDroneWindow> windows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(AddDroneWindow)
                                            select (AddDroneWindow)w).ToList();
            foreach (AddDroneWindow ADW in windows)
            {
                if (ADW.thisDroneId != 0)
                {
                    ADW.SimulatorWorker.ProgressChanged += (s, e) =>
                    {
                        try
                        {
                            updateSpecificDrone(ADW.thisDroneId);
                        }
                        catch
                        {
                            try
                            {
                                DronesCollection.Remove(DronesCollection.First(d => d.Id == ADW.thisDroneId));
                            }
                            catch
                            {
                                
                            }
                        }
                    };
                }

            }
        }

    }
}
