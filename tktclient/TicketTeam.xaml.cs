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
using tktclient.Controls;
using tktclient.Model;
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// TicketTeam.xaml 的交互逻辑
    /// </summary>
    public partial class TicketTeam : Page, ITktControlReviewLoad
    {
        private TicketTeamViewModel _viewModel;

        public TicketTeam()
        {
            InitializeComponent();
            this._viewModel = this.DataContext as TicketTeamViewModel;
            this._viewModel.LoadTickets();
        }

        private void TicketRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.TicketSelected((sender as RadioButton)?.CommandParameter);
        }

        private void TicketSelectedButton_UpdateEnent(TicketSelectedButton btn, bool isfocus)
        {
            SelectedTicketModel dataContext = btn.DataContext as SelectedTicketModel;
            if (dataContext == null)
                return;
            btn.IsChecked = true;
            this._viewModel.CurTicket = dataContext;
            if (isfocus)
                return;
            this._viewModel.UpdateTotal(dataContext);
        }

        private void SelectedTicket_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null || radioButton.DataContext == null)
                return;
            this._viewModel.CurTicket = radioButton.DataContext as SelectedTicketModel;
        }

        public void ReviewLoad()
        {
            (this._viewModel as ITktControlReviewLoad).ReviewLoad();
        }
    }
}
