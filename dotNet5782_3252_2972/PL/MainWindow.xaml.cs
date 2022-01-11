using BlApi;
using PL.Customers;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogInAsManager_Click(object sender, RoutedEventArgs e)
        {
            (new MainManagerWindow()).Show();
            this.Close();
        }

        private void LogInAsCustomer_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            if(!int.TryParse(CustomerId.Text, out Id))
            {
                MessageBox.Show("ID must be a Number!", "Wrong ID type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            long Phone;
            try
            {
                long.TryParse(CustomerPhone.Text, out Phone);
                
            }
            catch
            {
                MessageBox.Show("Phone must be a Number!", "Wrong Phone type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            
            try
            {
                BO.Customer customer = myBL.GetCustomer(Id);
                if (CustomerPhone.Text != customer.Phone)
                {
                    MessageBox.Show("Phone number does not match!", "Wrong Phone number", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(BO.ItemNotFoundException ex)
            {
                MessageBox.Show(ex.ToString(), "Customer Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            (new ShowCustomerWindow(Id, "")).Show();
            this.Close();

        }

        private void SignUp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (new ShowCustomerWindow()).Show();
        }

        private void SignUp_MouseEnter(object sender, MouseEventArgs e)
        {
            SignUp.Foreground = Brushes.Red;
        }

        private void SignUp_MouseLeave(object sender, MouseEventArgs e)
        {
            SignUp.Foreground = Brushes.Blue;
        }
    }
}
