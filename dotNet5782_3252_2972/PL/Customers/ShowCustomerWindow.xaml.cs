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
using BlApi;
using PL.Parcel;

namespace PL.Customers
{
    /// <summary>
    /// Interaction logic for ShowCustomerWindow.xaml
    /// </summary> 
    public partial class ShowCustomerWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;
        public ShowCustomerWindow()
        {
            InitializeComponent();
            IfPresentation.IsChecked = false;
            Width = 300;
        }

        public ShowCustomerWindow(int CustomerId)
        {
            InitializeComponent();
            BO.Customer c = myBL.GetCustomer(CustomerId);
            IfPresentation.IsChecked = true;
            this.DataContext = c;
            ArrivingParcelsList.DataContext = c.ToThisCustomer;
            GoingParcelsList.DataContext = c.FromThisCustomer;
        }

        public ShowCustomerWindow(int CustomerId, object n)
        {
            InitializeComponent();
            BO.Customer c = myBL.GetCustomer(CustomerId);
            IfPresentation.IsChecked = true;
            this.DataContext = c;
            ArrivingParcelsList.DataContext = c.ToThisCustomer;
            GoingParcelsList.DataContext = c.FromThisCustomer;
            ArrivingParcelsList.MouseDoubleClick -= ParcelsList_DoubleClick;
            GoingParcelsList.MouseDoubleClick -= ParcelsList_DoubleClick;
        }

        public void CloseWindow()
        {
            disallowClosure = false;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = disallowClosure;
        }

        private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void ParcelsList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if((sender as ListView).SelectedItem is null)
            {
                return;
            }
            try
            {
            Parcel.ShowParcelWindow SPW = new Parcel.ShowParcelWindow(((sender as ListView).SelectedItem as BO.ParcelInCustomer).Id);
            SPW.Show();
            }
            catch
            {
                return;
            }
            CloseWindow();
        }

        private void AddCustomer_Button_Click(object sender, RoutedEventArgs e)
        {
            int CustomerId;
            long phone ;
            double latitude,longitude;
            if (!int.TryParse(CustomerId_TextBox.Text, out CustomerId))
            {
                MessageBox.Show("ID must be a Number!", "Wrong ID type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CustomerName_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the customer's name", "Empty name value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!long.TryParse(CustomerPhone_TextBox.Text, out phone))
            {
                MessageBox.Show("Phone must be a Number!", "Wrong Phone type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
            if (!double.TryParse(CustomerLongitude_TextBox.Text, out longitude))
            {
                MessageBox.Show("Longitude must be a Number!", "Wrong Longitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(CustomerLatitude_TextBox.Text, out latitude))
            {
                MessageBox.Show("LAtitude must be a Number!", "Wrong LAtitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                myBL.AddCustomer(CustomerId, CustomerName_TextBox.Text, CustomerPhone_TextBox.Text, longitude, latitude);
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CloseWindow();
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            int customerId = int.Parse(CustomerId_TextBox.Text);
            
            string customerName = CustomerName_TextBox.Text;
            string customerPhone = CustomerPhone_TextBox.Text;
            int PhoneTemp;

            if (CustomerName_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the customer's name", "Empty name value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool check = int.TryParse(CustomerPhone_TextBox.Text, out PhoneTemp);
            if (!check)
            {
                MessageBox.Show(" Phone number has to be a number", "Wrong type Phone number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (customerPhone.Length != 10)
            {
                MessageBox.Show(" Phone number should contain 10 digits", "Wrong type Phone number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                myBL.UpdateCustomer(customerId , CustomerName_TextBox.Text , CustomerPhone_TextBox.Text );
                MessageBox.Show("Customer has been updated", "update confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void AddParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            ShowParcelWindow SPW = new ShowParcelWindow(int.Parse(CustomerId_TextBox.Text), "");
            SPW.AddParcel_Button.Click += (s, e) =>
            {
                int Id;
                if (!int.TryParse(SPW.SenderId_TextBox.Text, out Id)) return;
                BO.Customer c = myBL.GetCustomer(Id);
                GoingParcelsList.ItemsSource = c.FromThisCustomer;
            };
            SPW.Show();
        }
    }
}
