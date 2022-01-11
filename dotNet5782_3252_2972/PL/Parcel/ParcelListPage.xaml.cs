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
            syncWithSimulator();
        }

        private void resetParcelsList()
        {
            if (ParcelsCollection != null)
            {
                ParcelsCollection.Clear();
            }
            myBL.GetAllParcels().OrderBy(p => p.Id).Distinct().ToList().ForEach(i => ParcelsCollection.Add(i));
            ParcelList.DataContext = ParcelsCollection;
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
            ParcelsCollection.Add(myBL.TurnParcelToList(myBL.GetParcel(Id)));
            ParcelList.DataContext = ParcelsCollection.OrderBy(d => d.Id);
        }

        private void ParcelsListReset_Click(object sender, RoutedEventArgs e)
        {
            resetParcelsList();
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
                if(ParcelsCollection.FirstOrDefault(p=> p.Id==Id) == null)
                {
                    ParcelsCollection.Add(myBL.TurnParcelToList(myBL.GetParcel(Id)));
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
                                BO.Parcel p = myBL.GetParcel((int)d.CurrentParcel.Id);
                                
                                ParcelsCollection.Remove(ParcelsCollection.First(parcel => parcel.Id == p.Id));
                                ParcelsCollection.Add(myBL.TurnParcelToList(myBL.GetParcel(p.Id)));
                                ParcelList.DataContext = ParcelsCollection.OrderBy(parcel => parcel.Id);
                            }
                            else if (d.Status == BO.DroneStatuses.Availible) { resetParcelsList(); }
                        }
                        catch
                        {
                            resetParcelsList();
                        }
                    };
                }

            }
        }



    }
}
