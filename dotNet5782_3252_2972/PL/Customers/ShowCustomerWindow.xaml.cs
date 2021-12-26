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
        }

        public ShowCustomerWindow(int CustomerId)
        {
            InitializeComponent();
            BO.Customer c = myBL.GetCustomer(CustomerId);
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


    }
}
