using BlApi;
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

namespace PL.Parcel
{
    /// <summary>
    /// Interaction logic for ShowParcelWindow.xaml
    /// </summary>
    public partial class ShowParcelWindow : Window
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        public ShowParcelWindow()
        {
            InitializeComponent();
            this.Width = 270;
            WeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityCB.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
        }

        public ShowParcelWindow(int ParcelId)
        {
            InitializeComponent();
            WeightCB.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityCB.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            IfPresentation.IsChecked = true;
            this.DataContext = myBL.GetParcel(ParcelId);
        }

        private void AddParcel_Button_Click(object sender, RoutedEventArgs e)
        {
            int SenderId;
            try
            {
                SenderId = int.Parse(SenderId_TextBox.Text);
            }
            catch
            {
                //TODO
                return;
            }

            int TargetId;
            try
            {
                TargetId = int.Parse(TargetId_TextBox.Text);
            }
            catch
            {
                //TODO
                return;
            }

            if (WeightCB.SelectedIndex == -1) return;
            if (PriorityCB.SelectedIndex == -1) return;

            try
            {
            myBL.AddParcel(SenderId, TargetId, (BO.WeightCategories)WeightCB.SelectedItem, (BO.Priorities)PriorityCB.SelectedItem);
            }
            catch
            {

            }
            this.Close();
        }
    }
}
