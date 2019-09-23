using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using tktclient.Db;
using tktclient.Infrastructure;
using tktclient.Model;
using tktclient.Services;
using tktclient.Utils;

namespace tktclient.ViewModel
{
    public class B2BSaleViewModel : BusyStatusViewModel
    {
        private ObservableCollection<TicketModel> _currentTickets;
        private ObservableCollection<SelectedTicketModel> _selectedTickets;
        private SelectedTicketModel _curTicket;
        private Decimal _totalAmount = new Decimal(0, 0, 0, false, 2);
        private int _totalQuantity;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;
        private ICommand _settleCommand;
        private OrderEntity order;
        private string _remark = "";
        private string _b2bNo = "";
        private string _selectB2BValue = "";
        private int _remainTicket;
        private int _remainRibbons;

        public B2BSaleViewModel()
        {
            this._remainTicket = ClientContext.RemainTickets;
            this._remainRibbons = ClientContext.RemainRibbons;
        }

        public ObservableCollection<TicketModel> CurrentTickets
        {
            get
            {
                if (this._currentTickets == null)
                    this._currentTickets = new ObservableCollection<TicketModel>();
                return this._currentTickets;
            }
            set
            {
                this._currentTickets = value;
                this.RaisePropertyChanged(nameof(CurrentTickets));
            }
        }
        public ObservableCollection<SelectedTicketModel> SelectedTickets
        {
            get
            {
                if (this._selectedTickets == null)
                    this._selectedTickets = new ObservableCollection<SelectedTicketModel>();
                return this._selectedTickets;
            }
            set
            {
                this._selectedTickets = value;
                this.RaisePropertyChanged(nameof(SelectedTickets));
            }
        }

        public SelectedTicketModel CurTicket
        {
            get
            {
                return this._curTicket;
            }
            set
            {
                this._curTicket = value;
                this.RaisePropertyChanged(nameof(CurTicket));
                //this.UpdateButton();
            }
        }

        public Decimal TotalAmount
        {
            get
            {
                return this._totalAmount;
            }
            set
            {
                this._totalAmount = value;
                this.RaisePropertyChanged(nameof(TotalAmount));
            }
        }

        public int TotalQuantity
        {
            get
            {
                return this._totalQuantity;
            }
            set
            {
                this._totalQuantity = value;
                this.RaisePropertyChanged(nameof(TotalQuantity));
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

        public string B2BNo
        {
            get { return this._b2bNo; }
            set
            {
                this._b2bNo = value;
                this.RaisePropertyChanged(nameof(_b2bNo));
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

        public async void LoadTickets(string b2b)
        {
            this._selectB2BValue = b2b;
            B2BSaleViewModel ticketSaleViewModel = this;
            this.ClearTickets();
            this.CurrentTickets.Clear();
            try
            {
                ticketSaleViewModel.IsBusy = true;
                ticketSaleViewModel.BusyContent = "正在加载数据...";
                var result = TktService.GetTickets(b2b);
                if (result.Code == Result.RESULT_SUCCESS)
                {
                    var tkts = result.data.Where(a => a.Prices.Count > 0).Select(a => new TicketModel()
                    {
                        Name = a.Name,
                        PriceRebate = Convert.ToDecimal(a.Prices.First().price),
                        Id = a.Id,
                        PerNumber = a.PerNums
                    }).ToList();
                    this.CurrentTickets = new ObservableCollection<TicketModel>(tkts);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ticketSaleViewModel.IsBusy = false;
            }
        }

        public void TicketSelected(object obj)
        {
            var ticket = obj as TicketModel;
            if (ticket == null)
                return;
            this.Add(ticket.Id, 1, ticket);
        }

        private async void Add(int id, int num, TicketModel ticket)
        {
            B2BSaleViewModel ticketSaleViewModel = this;
            SelectedTicketModel item = ticketSaleViewModel.SelectedTickets.FirstOrDefault((t => t.TicketId == id));
            if (item != null)
            {
                if (item.Number >= item.MaxNum)
                {
                    NavigationServiceHelper.NoticeWarn($"最大数量为{(object)item.MaxNum}张", "提示", true, 3);
                    return;
                }
                ++item.Number;
                int num1 = await ticketSaleViewModel.SetPrintNumber(item) ? 1 : 0;
                item.SumPrice = item.RealPrice * item.Number;
            }
            else if (ticket != null)
            {
                int num1 = 1;
                var selectedTicketModel = new SelectedTicketModel();
                selectedTicketModel.TicketId = ticket.Id;
                selectedTicketModel.TicketKindName = "电商票";
                selectedTicketModel.Number = num1;
                selectedTicketModel.SumPrice = ticket.PriceRebate * num1;
                selectedTicketModel.RealPrice = ticket.PriceRebate;
                selectedTicketModel.Barcodes = null;
                selectedTicketModel.BarcodeTypeName = "二维码";
                selectedTicketModel.TicketModelName = ticket.Name + "("+TktService.GetB2BName(this._selectB2BValue)+")";
                selectedTicketModel.PerNumber = ticket.PerNumber;
                item = selectedTicketModel;
                item.SumChangeEvent += ticketSaleViewModel.Item_SumChangeEvent;
                if (await ticketSaleViewModel.SetPrintNumber(item))
                    ticketSaleViewModel.SelectedTickets.Add(item);
            }
            ticketSaleViewModel.UpdateTicketNum(item);
            ticketSaleViewModel.UpdateTotal(null);
        }

        private async Task<bool> SetPrintNumber(SelectedTicketModel ticket)
        {
            int mediumNumber = ticket.Number;

            ticket.MediumNumber = mediumNumber;
            ticket.OldNumber = ticket.Number;
            return true;
        }

        private void Item_SumChangeEvent(SelectedTicketModel obj)
        {
            this.UpdateTotal(obj);
        }
        private void UpdateTicketNum(SelectedTicketModel item)
        {
            if (item == null)
                return;
            var tktModel = this.CurrentTickets.FirstOrDefault((t => t.Id == item.TicketId));
            if (tktModel == null)
                return;
            tktModel.SelectedCount = item.Number;
        }

        public async void UpdateTotal(SelectedTicketModel data)
        {
            if (data != null)
            {
                int num = await this.SetPrintNumber(data) ? 1 : 0;
                data.SumPrice = data.Number * data.RealPrice;
                this.UpdateTicketNum(data);
            }
            this.TotalAmount = this.SelectedTickets.Sum(t => t.SumPrice);
            this.TotalQuantity = this.SelectedTickets.Sum(t => t.Number);
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (this._deleteCommand == null)
                    this._deleteCommand = new RelayCommand(this.DeleteTicket, false);
                return this._deleteCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (this._clearCommand == null)
                    this._clearCommand = new RelayCommand(this.ClearTickets, false);
                return this._clearCommand;
            }
        }
        public ICommand SettleCommand
        {
            get
            {
                if (this._settleCommand == null)
                    this._settleCommand = new RelayCommand(this.Settle, false);
                return this._settleCommand;
            }
        }

        public void DeleteTicket()
        {
            try
            {
                if (this.CurTicket == null || !this.SelectedTickets.Contains(this.CurTicket))
                    return;
                this.TotalQuantity -= this.CurTicket.Number;
                this.TotalAmount -= this.CurTicket.SumPrice;
                this.SelectedTickets.Remove(this.CurTicket);
                this.CurTicket.Number = 0;
                this.UpdateTicketNum(this.CurTicket);
                this.CurTicket = null;
            }
            catch (Exception ex)
            {
                this.UpdateTotal(null);
            }
        }

        public void ClearTickets()
        {
            if (this.SelectedTickets != null && this.SelectedTickets.Count > 0)
            {
                foreach (SelectedTicketModel selectedTicket in this.SelectedTickets)
                {
                    selectedTicket.Number = 0;
                    this.UpdateTicketNum(selectedTicket);
                }
            }
            this.CurTicket = null;
            this.SelectedTickets.Clear();
            this.order = null;
            this.TotalAmount = new Decimal(0, 0, 0, false, 2);
            this.TotalQuantity = 0;
        }

        public async Task<bool> SettleCheck()
        {
            B2BSaleViewModel ticketSaleViewModel = this;
            try
            {
                ticketSaleViewModel.IsBusy = true;
                ticketSaleViewModel.BusyContent = "正在验证门票";
                if (ticketSaleViewModel.SelectedTickets.Count <= 0)
                {
                    NavigationServiceHelper.NoticeWarn("请先选择门票", "选择门票", true, 3);
                    return false;
                }
                if (ticketSaleViewModel.SelectedTickets.Any(t => t.Number <= 0))
                {
                    NavigationServiceHelper.NoticeWarn("请选择门票数量", "选择门票数量", true, 3);
                    return false;
                }
                if (this.B2BNo.Trim().Length == 0)
                {
                    NavigationServiceHelper.NoticeWarn("请填写电商换票码", "换票码", true, 3);
                    return false;
                }
                var exCodeCount = await StorageProvider.GetOrderByExCodeCount(this.B2BNo.Trim(), OrderTypes.电商换票);
                if (exCodeCount > 0) 
                {
                    NavigationServiceHelper.NoticeWarn("换票码已使用", "已使用", true, 3);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                NavigationServiceHelper.NoticeError("所选门票不在有效期范围内", "提示", true, 3);
                return false;
            }
            finally
            {
                ticketSaleViewModel.IsBusy = false;
            }
        }

        public async void Settle()
        {
            if (!await this.SettleCheck())
                return;
            //监控打印纸数量
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

        private async Task<OrderEntity> CreateOrder()
        {
            if (order != null)
                return order;
            order = new OrderEntity();
            order.OrderNo = NoBuilder.NewOrderNo(DateTime.Now);

            order.Nums = this.TotalQuantity;
            order.OrderType = (int)OrderTypes.电商换票;
            order.Amount = this.TotalAmount;
            order.PerNums = this.SelectedTickets.Sum(s => s.PerNumber * s.Number);
            order.CreateTime = DateTime.Now;
            order.State = (int)OrderStates.未支付;
            order.PayType = (int)PayTypes.电商支付;
            order.ExCode = this.B2BNo;
            order.Ext1 = this._selectB2BValue;
            order.ClientNo = ClientContext.CurrentUser.SerialNo;
            var ok = await Db.StorageProvider.SaveOrder(order);
            if (ok)
            {
                foreach (var stm in this.SelectedTickets)
                {
                    var childOrder = new ChildOrderEntity();
                    childOrder.OrderId = order.Id;
                    childOrder.OrderType = (short)OrderTypes.电商换票;
                    childOrder.OrderNo = order.OrderNo;
                    childOrder.TicketId = stm.TicketId;
                    childOrder.TicketName = stm.TicketModelName;
                    childOrder.Amount = stm.SumPrice;
                    childOrder.UnitPrice = stm.RealPrice;
                    childOrder.Nums = stm.Number;
                    childOrder.PerNums = stm.PerNumber;
                    childOrder.CreateTime = DateTime.Now;
                    childOrder.UseDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    await Db.StorageProvider.SaveChildOrder(childOrder);
                }

                return order;
            }
            return null;
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
                ticketSaleViewModel.ClearTickets();
            }
        }
    }
}
