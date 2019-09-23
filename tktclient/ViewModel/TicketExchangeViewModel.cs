using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using tktclient.Db;
using tktclient.Infrastructure;
using tktclient.Services;
using tktclient.Utils;

namespace tktclient.ViewModel
{
    public class TicketExchangeViewModel : BusyStatusViewModel
    {
        private ObservableCollection<OrderDto> _orderInfoCollection;
        private OrderDto _orderInfo;
        private string _searchKey;
        private int _recondCount;
        private int _printQuantity;
        private int _perQuantity;
        private string _remark = "";
        private string _exCode;
        private ObservableCollection<SubOrderDto> _selectedSubOrders;
        private ICommand _searchCommand;
        private ICommand _clearCommand;
        private ICommand _printCommand;
        private int _remainTicket;
        private int _remainRibbons;

        public TicketExchangeViewModel()
        {
            this._remainTicket = ClientContext.RemainTickets;
            this._remainRibbons = ClientContext.RemainRibbons;
        }

        public ICommand SearchCommand
        {
            get
            {
                if (this._searchCommand == null)
                    this._searchCommand = new RelayCommand(this.Search, false);
                return this._searchCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (this._clearCommand == null)
                    this._clearCommand = new RelayCommand(this.Clear, false);
                return this._clearCommand;
            }
        }

        public ICommand PrintCommand
        {
            get
            {
                if (this._printCommand == null)
                    this._printCommand = new RelayCommand(this.Print, false);
                return this._printCommand;
            }
        }

        public ObservableCollection<OrderDto> OrderInfoCollection
        {
            get
            {
                if (this._orderInfoCollection == null)
                    this._orderInfoCollection = new ObservableCollection<OrderDto>();
                return this._orderInfoCollection;
            }
            set
            {
                this.Set(ref this._orderInfoCollection, value, false, nameof(OrderInfoCollection));
            }
        }

        public ObservableCollection<SubOrderDto> SelectedSubOrders
        {
            get
            {
                if (this._selectedSubOrders == null)
                    this._selectedSubOrders = new ObservableCollection<SubOrderDto>();
                return this._selectedSubOrders;
            }
            set
            {
                this.Set(ref this._selectedSubOrders, value, false, nameof(SelectedSubOrders));
            }
        }

        public string Remark
        {
            get { return this._remark; }
            set
            {
                this._remark = value;
                this.RaisePropertyChanged(nameof(_remark));
            }
        }

        public int PerQuantity
        {
            get
            {
                return this._perQuantity;
            }
            set
            {
                this._perQuantity = value;
                this.RaisePropertyChanged(nameof(PerQuantity));
            }
        }

        public int PrintQuantity
        {
            get
            {
                return this._printQuantity;
            }
            set
            {
                this._printQuantity = value;
                this.RaisePropertyChanged(nameof(PrintQuantity));
            }
        }

        public int RecordCount
        {
            get
            {
                return this._recondCount;
            }
            set
            {
                this._recondCount = value;
                this.RaisePropertyChanged(nameof(RecordCount));
            }
        }

        public int RemainTicket
        {
            get
            {
                return this._remainTicket;
            }
            set
            {
                this.Set<int>(ref this._remainTicket, value, false, nameof(RemainTicket));
                ClientContext.RemainTickets = value;
            }
        }

        public int RemainRibbons
        {
            get
            {
                return this._remainRibbons;
            }
            set
            {
                this.Set<int>(ref this._remainRibbons, value, false, nameof(RemainRibbons));
                ClientContext.RemainRibbons = value;
            }
        }

        public OrderDto OrderInfo
        {
            get
            {
                return this._orderInfo;
            }
            set
            {
                this.Set(ref this._orderInfo, value, false, nameof(OrderInfo));
                Messenger.Default.Send(new NotificationMessage("OrderInfoChange"));
            }
        }

        public string SearchKey
        {
            get
            {
                return this._searchKey;
            }
            set
            {
                this._searchKey = value;
                this.RaisePropertyChanged(nameof(SearchKey));
            }
        }

        private void Clear()
        {
            this.OrderInfo = null;
            this.OrderInfoCollection.Clear();
            this.RecordCount = 0;
            this.SelectedSubOrders.Clear();
            this.PerQuantity = 0;
            this.PrintQuantity = 0;
            this._exCode = null;
        }


        public async void Search()
        {
            try
            {
                this.ErrorMessage = null;
                if (string.IsNullOrWhiteSpace(this.SearchKey))
                {
                    this.Clear();
                }
                else
                {
                    this.OrderInfo = null;
                    this.OrderInfoCollection.Clear();
                    this.IsBusy = true;
                    this.BusyContent = "正在查询...";
                    var requestResult = TktService.GetOrderByCode(this.SearchKey);
                    if (requestResult != null)
                    {
                        if (requestResult.Code == Result.RESULT_SUCCESS)
                        {
                            var dbOrder = await StorageProvider.GetOrderByCloudId(requestResult.data.OrderNo);
                            if (dbOrder != null)
                            {
                                requestResult.data.State = (short)OrderStates.已打印;
                                requestResult.data.Prints = await StorageProvider.GetPrints(dbOrder.OrderNo);
                            }
                            this.OrderInfoCollection.Add(requestResult.data);
                            this.OrderInfo = null;
                            this.RecordCount = 1;
                        }
                        else
                        {
                            NavigationServiceHelper.NoticeWarn(requestResult.Msg, "提示", true, 3);
                            this.ErrorMessage = requestResult.Msg;
                        }
                    }
                    else
                    {
                        NavigationServiceHelper.NoticeError("查询失败", "提示", true, 3);
                        this.ErrorMessage = "查询失败";
                    }
                    this.IsBusy = false;
                }
            }
            finally
            {
                this.IsBusy = false;
                this._exCode = this.SearchKey;
                this.SearchKey = null;
            }
        }

        public async void Print()
        {
            var ok = await PrintCheck();
            if ( !ok)
            {
                return;
            }
            ConfirmWindow confirmWindow = new ConfirmWindow("确定打印？", "提示", ConfirmType.Warn, ConfirmButton.OkCancel, false);
            if (!NavigationServiceHelper.ShowOwnerWin(confirmWindow, true) || confirmWindow._ConfirmResult != ConfirmResult.Ok)
                return;
            var tOrder = await CreateOrder();
            if (tOrder == null)
            {
                NavigationServiceHelper.NoticeWarn("订单创建失败", "警告", true, 5);
                return;
            }
            this.IssueTicket(tOrder);
        }

        internal async void IssueTicket(OrderEntity order)
        {
            var ticketSaleViewModel = this;
            try
            {
                ticketSaleViewModel.IsBusy = true;
                ticketSaleViewModel.BusyContent = "正在出票...";

                var result = await Printer.Print(order, (childId, seq, success) =>
                {
                    if (success)
                    {
                        this.RemainTicket--;
                        this.RemainRibbons--;
                    }
                });
                if (!result)
                {
                    ticketSaleViewModel.ErrorMessage = "出票失败:打印失败。";
                    NavigationServiceHelper.NoticeError(ticketSaleViewModel.ErrorMessage, "提示", true, 5);
                }
            }
            catch (Exception ex)
            {
                ticketSaleViewModel.ErrorMessage = "出票失败:数据未提交成功,服务器未响应。";
                NavigationServiceHelper.NoticeError(ticketSaleViewModel.ErrorMessage, "提示", true, 5);
            }
            finally
            {
                ticketSaleViewModel.IsBusy = false;
                ticketSaleViewModel.BusyContent = null;
                ticketSaleViewModel.Clear();
            }
        }

        private async Task<OrderEntity> CreateOrder()
        {
            var order = new OrderEntity();
            var sOrder = await StorageProvider.GetOrderByCloudId(this.OrderInfo.OrderNo);
            if (sOrder != null)
            {
                order = sOrder;
                return order;
            }

            order.CloudId = this.OrderInfo.OrderNo;
            order.OrderNo = NoBuilder.NewOrderNo(DateTime.Now);
            order.Nums = this.PrintQuantity;
            order.OrderType = (short)OrderTypes.换票;
            order.Amount = this.OrderInfo.Price;
            order.PerNums = this.PerQuantity;
            order.CreateTime = DateTime.Now;
            order.State = (int)OrderStates.已支付;
            order.PayType = (int)PayTypes.线上;
            order.Remark = this.Remark;
            order.ExCode = this._exCode;
            order.ExCodeSync = false;
            order.ClientNo = ClientContext.CurrentUser.SerialNo;
            var ok = await Db.StorageProvider.SaveOrder(order);
            if (ok)
            {
                foreach (var stm in this.OrderInfo.SubOrders)
                {
                    var childOrder = new ChildOrderEntity();
                    childOrder.OrderId = order.Id;
                    childOrder.OrderType = (short)OrderTypes.换票;
                    childOrder.CloudId = stm.Id.ToString();
                    childOrder.OrderNo = order.OrderNo;
                    childOrder.OrderType = order.OrderType;
                    childOrder.TicketId = stm.TicketId;
                    childOrder.TicketName = stm.Snapshot.Name;
                    childOrder.Amount = stm.TotalPrice;
                    childOrder.UnitPrice = stm.UnitPrice;
                    childOrder.Nums = stm.Nums;
                    childOrder.PerNums = stm.PerNums;
                    childOrder.CreateTime = DateTime.Now;
                    if (stm.UseDate != null)
                    {
                        childOrder.UseDate = int.Parse(stm.UseDate?.ToString("yyyyMMdd"));
                    }
                    else
                    {
                        childOrder.UseDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    }
                    await Db.StorageProvider.SaveChildOrder(childOrder);
                }
                return order;
            }
            return null;
        }

        private async Task<bool> PrintCheck()
        {
            if (this.SelectedSubOrders.Count == 0)
            {
                NavigationServiceHelper.NoticeError("打印失败:没有可打印的票", "提示", true, 3);
                this.ErrorMessage = "打印失败:没有可打印的票";
                return false;
            }
            if (this.OrderInfo == null)
            {
                NavigationServiceHelper.NoticeError("打印失败:未选择票", "提示", true, 3);
                this.ErrorMessage = "打印失败:未选择票";
                return false;
            }

            if (this.OrderInfo.Extra.CodeUseState == (int)TicketCodeStates.已使用)
            {
                NavigationServiceHelper.NoticeError("打印失败:兑换码已使用", "提示", true, 3);
                this.ErrorMessage = "打印失败:兑换码已使用";
                return false;
            }
            if (this.OrderInfo.Extra.CodeUseState == (int)TicketCodeStates.已过期)
            {
                NavigationServiceHelper.NoticeError("打印失败:兑换码已过期", "提示", true, 3);
                this.ErrorMessage = "打印失败:兑换码已过期";
                return false;
            }
            var sOrder = await StorageProvider.GetOrderByCloudId(this.OrderInfo.OrderNo);
            if (sOrder != null && sOrder.State == (int) OrderStates.已打印)
            {
                NavigationServiceHelper.NoticeError("打印失败:兑换码已换票", "提示", true, 3);
                this.ErrorMessage = "打印失败:兑换码已换票";
                return false;
            }
            return true;
        }

        public async void OrderSelected(object obj)
        {
            if (obj == null)
                return;
            this.OrderInfo = obj as OrderDto;
            this.SelectedSubOrders.Clear();
            this.PrintQuantity = 0;
            this.PerQuantity = 0;
            foreach (var subOrder in this.OrderInfo.SubOrders)
            {
                var dbOrder = await StorageProvider.GetChildOrderByCloudId(subOrder.Id.ToString());
                if (dbOrder != null)
                {
                    subOrder.PrintedCount = dbOrder.Prints;
                }

                this.SelectedSubOrders.Add(subOrder);
                this.PrintQuantity += subOrder.Nums;
                this.PerQuantity += subOrder.PerNums;
            }
        }

    }
}
