using BlApi;
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

namespace PL.Parcel
{
    /// <summary>
    /// Interaction logic for ShowParcelWindow.xaml
    /// </summary>
    public partial class ShowParcelWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;
        public ShowParcelWindow()
        {
            InitializeComponent();
            resetComboBoxes();
            this.Width = 270;
        }

        public ShowParcelWindow(int ParcelId)
        {
            InitializeComponent();
            resetComboBoxes();
            IfPresentation.IsChecked = true;
            this.DataContext = myBL.GetParcel(ParcelId);
            Main.DataContext = this.DataContext;
        }

        public void resetComboBoxes()
        {
            WeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityCB.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
        }

        private void AddParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            int SenderId;
            try
            {
                SenderId = int.Parse(SenderId_TextBox.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int TargetId;
            try
            {
                TargetId = int.Parse(TargetId_TextBox.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (WeightCB.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a weight", "Weight selection ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (PriorityCB.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a priority", "Priority selection ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                ParcelId_TextBox.Text = myBL.AddParcel(SenderId, TargetId, (BO.WeightCategories)WeightCB.SelectedItem, (BO.Priorities)PriorityCB.SelectedItem).ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CloseWindow();
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

        private void DeleteParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            if((this.DataContext as BO.Parcel).DroneId != 0)
            {
                MessageBox.Show("Cant Delete Parcel:\nDrone Id already associated", "Drone ID ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
            myBL.DeleteParcel((this.DataContext as BO.Parcel).Id);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CloseWindow();
        }

        private void ShowDrone_Button_Click(object sender, RoutedEventArgs e)
        {
            AddDroneWindow ADW = new AddDroneWindow(int.Parse(DroneId_TextBox.Text));
            ADW.Show();
            CloseWindow();
        }

        private void ShowTarget_Button_Click(object sender, RoutedEventArgs e)
        {
            Customers.ShowCustomerWindow SCW = new Customers.ShowCustomerWindow(int.Parse(SenderId_TextBox.Text));
            SCW.Show();
            CloseWindow();
        }

        private void ShowSender_Button_Click(object sender, RoutedEventArgs e)
        {
            Customers.ShowCustomerWindow SCW = new Customers.ShowCustomerWindow(int.Parse(TargetId_TextBox.Text));
            SCW.Show();
            CloseWindow();
        }

        private void UpdateParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            int ParcelId = int.Parse(ParcelId_TextBox.Text);
            try
            {
                myBL.UpdateParcel(ParcelId, (BO.Priorities)PriorityCB.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }
    }
}
