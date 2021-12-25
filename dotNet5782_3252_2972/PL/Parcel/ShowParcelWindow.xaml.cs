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
            WeightCB.DataContext = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityCB.DataContext = Enum.GetValues(typeof(BO.Priorities));
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

        }
    }
}
