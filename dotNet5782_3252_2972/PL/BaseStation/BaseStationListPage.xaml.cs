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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BlApi;

namespace PL.BaseStation
{
    /// <summary>
    /// Interaction logic for BaseStationListPage.xaml
    /// </summary>
    public partial class BaseStationListPage : Page
    {
        BlApi.IBL myBL = BlFactory.GetBL();
        private ObservableCollection<BO.BaseStationToList> _baseStationCollection = new ObservableCollection<BO.BaseStationToList>();
        public ObservableCollection<BO.BaseStationToList> BaseStationCollection
        {
            get
            {
                return _baseStationCollection;
            }
            set
            {
                _baseStationCollection = value;

            }
        }

        public BaseStationListPage()
        {
            InitializeComponent();
            resetBaseStationList();
            BaseStationList.DataContext = BaseStationCollection;
            this.DataContext = this;
        }

        private void resetBaseStationList()
        {
            if (BaseStationCollection != null)
            {
                BaseStationCollection.Clear();
            }
            myBL.GetAllBaseStations().Distinct().ToList().ForEach(i => BaseStationCollection.Add(i));
        }

        private void BaseStationListReset_Click(object sender, RoutedEventArgs e)
        {
            resetBaseStationList();
        }

        private void BaseStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BaseStationList.SelectedItem == null) return;
            ShowBaseStationWindow SBSW = new ShowBaseStationWindow((BaseStationList.SelectedItem as BO.BaseStationToList).Id);
            RoutedEventHandler updateBS = (s, e) =>
            {
                int Id = int.Parse(SBSW.BaseStationId_TextBox.Text);
                updateSpecificBaseStation(Id);
            };
            SBSW.Update_Button.Click += updateBS;
            SBSW.CloseWindow_Button.Click += updateBS;
            
            SBSW.Show();
        }
        
        private void updateSpecificBaseStation(int Id)
        {
            BaseStationCollection.Remove(BaseStationCollection.First(d => d.Id == Id));
            BaseStationCollection.Add(myBL.TurnBaseStationToList(myBL.GetBaseStation(Id)));
            BaseStationList.DataContext = BaseStationCollection.OrderBy(d => d.Id);
        }

        private void DeleteBaseStation_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            ShowBaseStationWindow SBSW = new ShowBaseStationWindow();
            SBSW.AddBaseStation_Button.Click += (s, e) =>
            {
                int Id;
                if (!int.TryParse(SBSW.BaseStationId_TextBox.Text, out Id))
                {
                    return;
                }
                if (BaseStationCollection.FirstOrDefault(p => p.Id == Id) == null)
                {
                    BaseStationCollection.Add(myBL.TurnBaseStationToList(myBL.GetBaseStation(Id)));
                }
            };
            SBSW.Show();
        }
    }
}
