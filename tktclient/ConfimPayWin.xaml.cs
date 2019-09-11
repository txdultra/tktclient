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
using GalaSoft.MvvmLight.Messaging;
using tktclient.Controls;
using tktclient.Model;
using tktclient.Services;
using tktclient.Utils;
using tktclient.ViewModel;

namespace tktclient
{
    /// <summary>
    /// ConfimPayWin.xaml 的交互逻辑
    /// </summary>
    public partial class ConfimPayWin : Window
    {
        //public PayResultModel PayResult { get; private set; }
        private bool _IsClosed;
        private IList<PayTypeDto> _PayTypes;
        private ConfirmPayViewModel _ViewModel;
        private readonly BarcodePay _barcodePayModel;
        public PayResultModel PayResult { get; private set; }

        public ConfimPayWin(BarcodePay payModel)
        {
            this.InitializeComponent();
            this._ViewModel = this.DataContext as ConfirmPayViewModel;
            this._barcodePayModel = payModel;
            this._ViewModel.Init(payModel.TotalFee, payModel.ClientId, payModel.Quantity);
            this._PayTypes = payModel.AllPayTypes;
            this.Loaded += this.ConfimPayWin_Loaded;
            Messenger.Default.Register<bool>(this, (object)"BarCodePayCancel", (t =>
            {
                if (!t)
                    return;
                Messenger.Default.Unregister(this);
                this.Dispatcher.Invoke(() => this.DialogResult = false);
            }), false);
        }

        private void ConfimPayWin_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitPayTypes();
        }

        private void InitPayTypes()
        {
            for (int index = 0; index < this._PayTypes.Count; ++index)
            {
                PayTypeDto payType = this._PayTypes[index];
                var payBtn = new PayRadioButton();
                payBtn.PayCode = payType.Code;
                payBtn.Content = payType.Name;
                payBtn.Margin = new Thickness(0.0, 5.0, 0.0, 5.0);
                payBtn.FontSize = 23.0;
                payBtn.Tag = "\xE60F";
                payBtn.CommandParameter = payType;
                payBtn.Click += this.PayBtn_Click;
                this.wpPayTypes.Children.Add(payBtn);
                if (index == 0)
                {
                    payBtn.IsChecked = true;
                    this.PayBtn_Click(payBtn, null);
                }
            }
        }

        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            PayRadioButton payRadioButton = sender as PayRadioButton;
            if (payRadioButton == null || payRadioButton.CommandParameter == null)
                return;
            var commandParameter = payRadioButton.CommandParameter as PayTypeDto;
            this._ViewModel.RealPay = this._ViewModel.ShouldPay;
            //this.spCards.Visibility = Visibility.Collapsed;
            if (commandParameter == null)
                return;
            this._ViewModel.CurPayType = commandParameter;
            this._ViewModel.ErrorMessage = null;
            if (commandParameter.IsOnline)
            {
                //this.tbBarCode.Focus();
                this.tbRealPay.IsReadOnly = true;
            }
            else
                this.tbRealPay.IsReadOnly = false;
            if (commandParameter.Code == "SignPay" || commandParameter.Code == "UmsPay" || (commandParameter.Code == "CCBPay" || commandParameter.Code == "ABCPay"))
                this.tbRealPay.IsReadOnly = true;
            if (!(commandParameter.Code == "NKPay"))
                return;
            //this.tbBarCode.Focus();
            this.tbRealPay.IsReadOnly = true;
            //this.spCards.Visibility = Visibility.Visible;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this._IsClosed = true;
            this.DialogResult = false;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.SettleFun();
        }

        private async void SettleFun()
        {
            try
            {
                this.btnSettle.IsEnabled = false;
                this._ViewModel.ErrorMessage = null;
                if (this._ViewModel.CurPayType == null)
                {
                    NavigationServiceHelper.NoticeWarn("请选择支付方式", "提示", true, 3);
                }
                this.CheckPayResult(null);
            }
            finally
            {
                this.btnSettle.IsEnabled = true;
            }
        }

        private void CheckPayResult(string code = null)
        {
            try
            {
                this._ViewModel.IsBusy = true;
                this._ViewModel.BusyContent = "正在处理...";
                this.PayResult = this._ViewModel.GetResultModel(code);
                if (this.PayResult == null || this.PayResult.PayStatus == PayResultType.未支付)
                    return;
                this.Dispatcher.Invoke(() =>
                {
                    if (this._IsClosed)
                        return;
                    this.DialogResult = true;
                });
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this._ViewModel.IsBusy = false;
                this._ViewModel.BusyContent = null;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            this.btnOk_Click((object)null, null);
        }

        private void tbRealPay_LostFocus(object sender, RoutedEventArgs e)
        {
            this._ViewModel.SetRealPay(this.tbRealPay.Text);
        }

        private void tbRealPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._ViewModel.ErrorMessage = null;
            Decimal result = new Decimal();
            if (!Decimal.TryParse(this.tbRealPay.Text, out result))
                return;
            this._ViewModel.SetRealPay(this.tbRealPay.Text);
        }

        private void tbRealPay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _ViewModel.SetRealPay(tbRealPay.Text);
            }
            else if (e.Key == Key.Decimal && (tbRealPay.Text.Contains(".") || tbRealPay.Text.StartsWith(".")))
            {
                e.Handled = true;
            }
        }

    }
}
