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
        }

        private void BaseStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeleteBaseStation_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddBaseStation_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
