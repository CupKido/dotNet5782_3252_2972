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

namespace PL.Parcel
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        private ObservableCollection<BO.ParcelToList> _parcelsCollection = new ObservableCollection<BO.ParcelToList>();
        public ObservableCollection<BO.ParcelToList> ParcelsCollection
        {
            get
            {
                return _parcelsCollection;
            }
            set
            {
                _parcelsCollection = value;

            }
        }

        public ParcelListPage()
        {
            InitializeComponent();
            resetParcelsList();
            ParcelList.DataContext = ParcelsCollection;
            this.DataContext = this;
            ParcelPriorityBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            ParcelsCollection.CollectionChanged += (s, e) =>
            {
                ParcelList.DataContext = ParcelsCollection.OrderBy(d => d.Id);
            };
            syncWithSimulator();
        }

        private void resetParcelsList()
        {
            if (ParcelsCollection != null)
            {
                ParcelsCollection.Clear();
            }
            myBL.GetAllParcels().OrderBy(p => p.Id).Distinct().ToList().ForEach(i => ParcelsCollection.Add(i));
        }

        private void ParcelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelList.SelectedItem == null) return;
            ShowParcelWindow SPW = new ShowParcelWindow((ParcelList.SelectedItem as BO.ParcelToList).Id);

            RoutedEventHandler updateParcel = (s, e) =>
            {
                int Id = int.Parse(SPW.ParcelId_TextBox.Text);
                updateSpecificParcel(Id);
            };
            SPW.CloseWindow_Button.Click += updateParcel;
            SPW.UpdateParcel_Button.Click += updateParcel;
            SPW.DeleteParcel_Button.Click += (s, e) =>
            {
                int Id = int.Parse(SPW.ParcelId_TextBox.Text);
                ParcelsCollection.Remove(ParcelsCollection.First(p => p.Id == Id));
            };
            SPW.Show();
        }

        private void updateSpecificParcel(int Id)
        {
            ParcelsCollection.Remove(ParcelsCollection.First(d => d.Id == Id));
            BO.ParcelToList parcelToList = myBL.TurnParcelToList(myBL.GetParcel(Id));
            if (filterSingleParcel(parcelToList))
            {
                ParcelsCollection.Add(parcelToList);
            }
        }

        private bool filterSingleParcel(BO.ParcelToList parcel)
        {
            if(ParcelPriorityBox.SelectedIndex >= 0)
            {
                BO.Priorities priority = (BO.Priorities)ParcelPriorityBox.SelectedIndex;
                if (parcel.Priority != priority) return false;
            }
            return true;
        }

        private void filterParcelsList()
        {
            List<BO.ParcelToList> parcelToLists = myBL.GetAllParcels().ToList();
            if (ParcelPriorityBox.SelectedIndex >= 0)
            {
                BO.Priorities priority = (BO.Priorities)ParcelPriorityBox.SelectedIndex;
                parcelToLists = parcelToLists.Where(ptl => ptl.Priority == priority).ToList();
            }
            ParcelsCollection.Clear();
            parcelToLists.OrderBy(p => p.Id).Distinct().ToList().ForEach(i => ParcelsCollection.Add(i));
        }

        private void ParcelsListReset_Click(object sender, RoutedEventArgs e)
        {
            resetParcelsList();
            resetComboBoxes();
        }

        private void DeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (myBL.GetParcel(((BO.ParcelToList)ParcelList.SelectedItem).Id).DroneId != 0)
                {
                    MessageBox.Show("Cant Delete Parcel:\nDrone Id already associated", "Drone ID ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                    myBL.DeleteParcel(((BO.ParcelToList)ParcelList.SelectedItem).Id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ParcelsCollection.Remove((ParcelList.SelectedItem as BO.ParcelToList));
        }

        private void AddParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowParcelWindow SPW = new ShowParcelWindow();
            SPW.AddParcel_Button.Click += (s, e) =>
            {
                int Id;
                if(!int.TryParse(SPW.ParcelId_TextBox.Text, out Id))
                {
                    return;
                }
                if(ParcelsCollection.FirstOrDefault(p=> p.Id == Id) == null)
                {
                    BO.ParcelToList parcelToList = myBL.TurnParcelToList(myBL.GetParcel(Id));
                    if (filterSingleParcel(parcelToList))
                    {
                        ParcelsCollection.Add(parcelToList);
                    }
                }
            };
            SPW.Show();
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
                            if (d.CurrentParcel.Id is not null && d.Status == BO.DroneStatuses.InDelivery)
                            { 
                                updateSpecificParcel((int)d.CurrentParcel.Id);
                            }
                            else if (d.Status == BO.DroneStatuses.Availible) { filterParcelsList(); }
                        }
                        catch
                        {
                            filterParcelsList();
                        }
                    };
                }

            }
        }

        private void resetComboBoxes()
        {
            ParcelPriorityBox.SelectedIndex = -1;
            ParcelPriorityBox.Text = "Select priority";
        }

        private void ParcelPriorityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterParcelsList();
        }
    }
}
