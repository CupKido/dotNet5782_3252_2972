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
            Width = 300;
        }

        public ShowCustomerWindow(int CustomerId)
        {
            InitializeComponent();
            BO.Customer c = myBL.GetCustomer(CustomerId);
            IfPresentation.IsChecked = true;
            this.DataContext = c;
            ArrivingParcelsList.ItemsSource = c.ToThisCustomer;
            GoingParcelsList.ItemsSource = c.FromThisCustomer;
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
            int CustomerId , phone ;
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

            if (!int.TryParse(CustomerPhone_TextBox.Text, out phone))
            {
                MessageBox.Show("Phone must be a Number!", "Wrong Phone type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
            if (!double.TryParse(CustomerLongitude_TextBox.Text, out longitude))
            {
                MessageBox.Show("Longitude must be a Number!", "Wrong Longitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(CustomerLAtitude_TextBox.Text, out latitude))
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

      
    }
}
