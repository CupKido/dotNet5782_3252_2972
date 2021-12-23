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
            DeleteParcel.IsEnabled = false;
        }

        private void resetParcelsList()
        {
            if (ParcelsCollection != null)
            {
                ParcelsCollection.Clear();
            }
            myBL.GetAllParcels().Distinct().ToList().ForEach(i => ParcelsCollection.Add(i));
        }

        private void ParcelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParcelsListReset_Click(object sender, RoutedEventArgs e)
        {
            resetParcelsList();
        }

        private void DeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DeleteParcel(((BO.ParcelToList)ParcelList.SelectedItem).Id);
            }
            catch
            {
                //TODO
            }
            resetParcelsList();
            DeleteParcel.IsEnabled = false;
        }

        private void AddParcel_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteParcel.IsEnabled = true;
        }
    }
}
