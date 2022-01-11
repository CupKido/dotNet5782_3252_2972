using BlApi;
using PL.Customers;
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
            syncWithSimulator();
        }

        private void resetCustomersList()
        {
            if (CustomersCollection != null)
            {
                CustomersCollection.Clear();
            }
            myBL.GetAllCustomers().Distinct().ToList().ForEach(i => CustomersCollection.Add(i));
            CustomerList.DataContext = CustomersCollection.OrderBy(c => c.Id);
        }

        private void deleteSelected_click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DeleteCustomer(((BO.CustomerToList)CustomerList.SelectedItem).Id);
            }
            catch
            {
                //TODO
            }
            resetCustomersList();
        }

        private void CustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerList.SelectedItem == null) return;
            ShowCustomerWindow SCW = new ShowCustomerWindow((CustomerList.SelectedItem as BO.CustomerToList).Id);
            SCW.Update_Button.Click += (s, e) =>
            {
                int Id = int.Parse(SCW.CustomerId_TextBox.Text);
                updateSpecificCustomer(Id);
            };
            SCW.AddParcel_Button.Click += (s, e) =>
            {
                int Id = int.Parse(SCW.CustomerId_TextBox.Text);
                updateSpecificCustomer(Id);
            };
            SCW.Show();
        }

        private void updateSpecificCustomer(int Id)
        {
            CustomersCollection.Remove(CustomersCollection.First(c => c.Id == Id));
            CustomersCollection.Add(myBL.TurnCustomerToList(myBL.GetCustomer(Id)));
            CustomerList.DataContext = CustomersCollection.OrderBy(c => c.Id);
        }

        private void resetCustomersList_click(object sender, RoutedEventArgs e)
        {
            resetCustomersList();
        }

        private void addCustomer_click(object sender, RoutedEventArgs e)
        { 
           

            ShowCustomerWindow SCW = new ShowCustomerWindow();
            SCW.AddCustomer_Button.Click += (s, e) =>
            {
                int Id;
                if (!int.TryParse(SCW.CustomerId_TextBox.Text, out Id))
                {
                    return;
                }
                if (CustomersCollection.FirstOrDefault(p => p.Id == Id) == null)
                {
                    try
                    {
                    BO.Customer c = myBL.GetCustomer(Id);
                    CustomersCollection.Add(myBL.TurnCustomerToList(c));
                    }
                    catch
                    {
                        resetCustomersList();
                        return;
                    }
                }
            };
            SCW.Show();


        }

        private void syncWithSimulator()
        {
            List<AddDroneWindow> windows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(AddDroneWindow)
                                            select (AddDroneWindow)w).ToList();
            foreach(AddDroneWindow ADW in windows)
            {
                if(ADW.thisDroneId != 0)
                {
                    ADW.SimulatorWorker.ProgressChanged += (s, e) =>
                    {
                        try
                        {  
                            BO.Drone d = myBL.GetDrone(ADW.thisDroneId);
                            if (d.CurrentParcel.Id is not null && d.Status == BO.DroneStatuses.InDelivery)
                            {
                                BO.Parcel p = myBL.GetParcel((int)d.CurrentParcel.Id);
                                CustomersCollection.Remove(CustomersCollection.First(c => c.Id == p.Target.Id));
                                CustomersCollection.Remove(CustomersCollection.First(c => c.Id == p.Sender.Id));
                                CustomersCollection.Add(myBL.TurnCustomerToList(myBL.GetCustomer(p.Sender.Id)));
                                CustomersCollection.Add(myBL.TurnCustomerToList(myBL.GetCustomer(p.Target.Id)));
                                CustomerList.DataContext = CustomersCollection.OrderBy(c => c.Id);
                            }
                            else if(d.Status == BO.DroneStatuses.Availible) { resetCustomersList(); }
                        }
                        catch
                        {
                            resetCustomersList();
                        }
                    };
                }
                
            }
        }

    }
}
