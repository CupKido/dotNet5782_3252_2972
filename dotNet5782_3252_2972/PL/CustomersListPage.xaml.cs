using BlApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListPage.xaml
    /// </summary>
    public partial class CustomersListPage : Page
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        private ObservableCollection<BO.CustomerToList> _customersCollection = new ObservableCollection<BO.CustomerToList>();
        public ObservableCollection<BO.CustomerToList> CustomersCollection
        {
            get
            {
                return _customersCollection;
            }
            set
            {
                _customersCollection = value;

            }
        }
        public CustomersListPage()
        {
            InitializeComponent();
            resetCustomersList();
            CustomerList.DataContext = _customersCollection;
            this.DataContext = this;
        }

        private void resetCustomersList()
        {
            if (CustomersCollection != null)
            {
                CustomersCollection.Clear();
            }
            myBL.GetAllCustomers().Distinct().ToList().ForEach(i => CustomersCollection.Add(i));
            //foreach (BO.DroneToList DTL in myBL.GetAllDrones())
            //{
            //    DronesCollection.Add(DTL);
            //}
            //DronesCollection = new ObservableCollection<BO.DroneToList>(myBL.GetAllDrones());
        }

        private void CustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void resetCustomersList_click(object sender, RoutedEventArgs e)
        {

        }

        private void addCustomer_click(object sender, RoutedEventArgs e)
        {

        }

    }
}
