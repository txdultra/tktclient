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
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// TktExchange.xaml 的交互逻辑
    /// </summary>
    public partial class TktExchange : Page
    {
        private TicketExchangeViewModel _ViewModel;

        public TktExchange()
        {
            InitializeComponent();
            this._ViewModel = this.DataContext as TicketExchangeViewModel;
            Messenger.Default.Register<NotificationMessage>((object)this, m =>
            {
                if (m.Notification != "OrderInfoChange")
                    return;
                //this.CheckRecordControl.LoadCheckRecords(this._ViewModel?.TicketInfo?.Id);
                //this.ConsumptionRecordControl.LoadRecords(this._ViewModel?.TicketInfo?.Id, new DateTime?(), new DateTime?());
                //this.OperateRecordsControl.LoadData(this._ViewModel?.TicketInfo);
            }, false);
            this.Loaded += this.ExchangeOperate_Loaded;
        }

        private void ExchangeOperate_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbSearch.SetFocus();
        }

        private void TicketOperateButton_Click(object sender, RoutedEventArgs e)
        {
            this._ViewModel.OrderSelected((sender as RadioButton)?.CommandParameter);
        }
    }
}
