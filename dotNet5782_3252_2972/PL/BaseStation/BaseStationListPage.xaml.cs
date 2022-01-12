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
using BlApi;

namespace PL.BaseStation
{
    /// <summary>
    /// Interaction logic for BaseStationListPage.xaml
    /// </summary>
    public partial class BaseStationListPage : Page
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        private ObservableCollection<BO.BaseStationToList> _baseStationCollection = new ObservableCollection<BO.BaseStationToList>();
        public ObservableCollection<BO.BaseStationToList> BaseStationCollection
        {
            get
            {
                return _baseStationCollection;
            }
            set
            {
                _baseStationCollection = value;

            }
        }

        public BaseStationListPage()
        {
            InitializeComponent();
            resetBaseStationList();
            BaseStationList.DataContext = BaseStationCollection;
            this.DataContext = this;
            BSStatusBox.ItemsSource = Enum.GetValues(typeof(PO.BaseStationStatus));
            BaseStationCollection.CollectionChanged += (s, e) => { BaseStationList.DataContext = BaseStationCollection.OrderBy(bs => bs.Id); };
            syncWithSimulator();
        }

        private void resetBaseStationList()
        {
            if (BaseStationCollection != null)
            {
                BaseStationCollection.Clear();
            }
            myBL.GetAllBaseStations().Distinct().ToList().ForEach(i => BaseStationCollection.Add(i));
        }

        private void BaseStationListReset_Click(object sender, RoutedEventArgs e)
        {
            resetBaseStationList();
            resetComboBoxes();
        }

        private void BaseStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BaseStationList.SelectedItem == null) return;
            ShowBaseStationWindow SBSW = new ShowBaseStationWindow((BaseStationList.SelectedItem as BO.BaseStationToList).Id);
            RoutedEventHandler updateBS = (s, e) =>
            {
                int Id = int.Parse(SBSW.BaseStationId_TextBox.Text);
                updateSpecificBaseStation(Id);
            };
            SBSW.Update_Button.Click += updateBS;
            SBSW.CloseWindow_Button.Click += updateBS;
            
            SBSW.Show();
        }
        
        private void updateSpecificBaseStation(int Id)
        {
            BaseStationCollection.Remove(BaseStationCollection.First(d => d.Id == Id));
            BO.BaseStationToList baseStation = myBL.TurnBaseStationToList(myBL.GetBaseStation(Id));
            if (filterSingleBS(baseStation))
            {
                BaseStationCollection.Add(baseStation);
            }

        }

        private bool filterSingleBS(BO.BaseStationToList baseStation)
        {
            if(BSStatusBox.SelectedIndex >= 0)
            {
                if (BSStatusBox.SelectedIndex != (baseStation.ChargeSlotsAvailible > 0 ? 0 : 1))
                {
                    return false;
                }
            }
            return true;
        }

        private void filterBSList()
        {
            List<BO.BaseStationToList> bsToLists = myBL.GetAllBaseStations().ToList();
            if (BSStatusBox.SelectedIndex >= 0)
            {
                bsToLists = bsToLists.Where(btl => BSStatusBox.SelectedIndex != (btl.ChargeSlotsAvailible > 0 ? 0 : 1)).ToList();
            }
            BaseStationCollection.Clear();
            bsToLists.OrderBy(bs => bs.Id).Distinct().ToList().ForEach(i => BaseStationCollection.Add(i));
        }

        private void AddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            ShowBaseStationWindow SBSW = new ShowBaseStationWindow();
            SBSW.AddBaseStation_Button.Click += (s, e) =>
            {
                int Id;
                if (!int.TryParse(SBSW.BaseStationId_TextBox.Text, out Id))
                {
                    return;
                }
                if (BaseStationCollection.FirstOrDefault(p => p.Id == Id) == null)
                {
                    BO.BaseStationToList baseStation = myBL.TurnBaseStationToList(myBL.GetBaseStation(Id));
                    if (filterSingleBS(baseStation))
                    {
                        BaseStationCollection.Add(baseStation);
                    }
                }
            };
            SBSW.Show();
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
                            BO.Drone d = myBL.GetDrone(ADW.thisDroneId);
                            int BSId = 0;
                            if (d.Status == BO.DroneStatuses.Maintenance)
                            {
                                BSId = myBL.GetChrgingInBaseStationId(d);
                                
                            }
                            else if (d.Status == BO.DroneStatuses.Availible && d.Battery > 90) 
                            {
                                BSId = myBL.closestBaseStation(d.CurrentLocation.Longitude, d.CurrentLocation.Latitude).Id;
                            }
                            else
                            {
                                return;
                            }
                            updateSpecificBaseStation(BSId);
                        }
                        catch
                        {
                            filterBSList();
                        }
                    };
                }

            }
        }

        private void BSStatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterBSList();
        }

        private void resetComboBoxes()
        {
            BSStatusBox.SelectedIndex = -1;
            BSStatusBox.Text = "Select status";
        }
    }
}
