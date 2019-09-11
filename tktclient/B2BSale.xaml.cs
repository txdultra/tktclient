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
using GalaSoft.MvvmLight.Messaging;
using tktclient.Controls;
using tktclient.Model;
using tktclient.Services;
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// B2BSale.xaml 的交互逻辑
    /// </summary>
    public partial class B2BSale : Page
    {
        private B2BSaleViewModel _viewModel;

        public B2BSale()
        {
            InitializeComponent();
            this._viewModel = this.DataContext as B2BSaleViewModel;
            this.InitB2Bers();
        }

        private void InitB2Bers()
        {
            this.txtB2B.ItemsSource = TktService.B2Bs;
            this.txtB2B.SelectedValuePath = "Value";
            this.txtB2B.DisplayMemberPath = "Key";
            this.txtB2B.SelectedIndex = -1;
            this.txtB2B.SelectionChanged += (changeSender, changedArgs) =>
            {
                if (this.txtB2B.SelectedValue == null || this.txtB2B.SelectedValue.ToString() == "-1")
                {
                    this._viewModel.CurrentTickets.Clear();
                    this._viewModel.SelectedTickets.Clear();
                    return;
                }
                this._viewModel.LoadTickets(this.txtB2B.SelectedValue.ToString());
            };
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
    }
}
