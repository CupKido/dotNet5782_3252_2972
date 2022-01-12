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
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (this.Width);
            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - (this.Height);
            this.Show();
        }

        private void LogInAsManager_Click(object sender, RoutedEventArgs e)
        {
            MainManagerWindow MMW = new MainManagerWindow();
            if(this.Left > System.Windows.SystemParameters.PrimaryScreenWidth - MMW.Width)
            {
                MMW.Left = System.Windows.SystemParameters.PrimaryScreenWidth - MMW.Width;
                MMW.a.Left = System.Windows.SystemParameters.PrimaryScreenWidth - MMW.Width;
            }
            else
            {
                MMW.Left = this.Left;
                MMW.a.Left = this.Left;
            }
            if (this.Top > System.Windows.SystemParameters.PrimaryScreenHeight - MMW.Height)
            {
                MMW.Top = System.Windows.SystemParameters.PrimaryScreenHeight - MMW.Height;
            }
            else
            {
                MMW.Top = this.Top;
            }
            MMW.a.Top = MMW.Top - MMW.a.Height;
            
            MMW.Show();
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

    
        private void FileID_GotFocus(object sender, RoutedEventArgs e)
        {
            CustomerId.Text = "";
        }

        private void FilePhone_GotFocus(object sender, RoutedEventArgs e)
        {
            CustomerPhone.Text = "";
        }
    }
}
