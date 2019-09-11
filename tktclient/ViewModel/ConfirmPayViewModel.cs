using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Model;
using tktclient.Services;

namespace tktclient.ViewModel
{
    public class ConfirmPayViewModel : BusyStatusViewModel
    {
        private Decimal _shouldPay = new Decimal(0, 0, 0, false, (byte)2);
        private Decimal _change;
        private Decimal? _realPay;
        private bool _isCashPay;
        private bool _isOnLinePay;
        private PayTypeDto _curPayType;
        private readonly PayResultModel _resultModel = new PayResultModel();
        private int Quantity { get; set; }
        public int? ClientId { get; set; }
        public Decimal ShouldPay
        {
            get
            {
                return this._shouldPay;
            }
            set
            {
                this._shouldPay = value;
                this.RaisePropertyChanged(nameof(ShouldPay));
            }
        }

        public Decimal Change
        {
            get
            {
                return this._change;
            }
            set
            {
                this._change = value;
                this.RaisePropertyChanged(nameof(Change));
            }
        }

        public Decimal? RealPay
        {
            get
            {
                return this._realPay;
            }
            set
            {
                this._realPay = value;
                this.RaisePropertyChanged(nameof(RealPay));
                this.Change = (value ?? Decimal.Zero) - this.ShouldPay;
            }
        }

        public bool IsCashPay
        {
            get
            {
                return this._isCashPay;
            }
            set
            {
                this._isCashPay = value;
                this.RaisePropertyChanged(nameof(IsCashPay));
            }
        }

        public PayTypeDto CurPayType
        {
            get
            {
                return this._curPayType;
            }
            set
            {
                this._curPayType = value;
                this.RaisePropertyChanged(nameof(CurPayType));
                if (value != null)
                {
                    this.IsCashPay = value.Code == "Cash";
                    this.IsOnLinePay = value.IsOnline;
                    //if (value.Code != "NKPay")
                    //    return;
                    this.IsOnLinePay = true;
                }
                else
                    this.IsOnLinePay = false;
            }
        }
        public bool IsOnLinePay
        {
            get
            {
                return this._isOnLinePay;
            }
            set
            {
                this._isOnLinePay = value;
                this.RaisePropertyChanged(nameof(IsOnLinePay));
            }
        }

        public void Update()
        {
            this.Change = (this.RealPay ?? Decimal.Zero) - this.ShouldPay;
        }
        public bool IsBarcodePay()
        {
            return this.CurPayType.IsOnline;
        }

        public void Init(Decimal totalAmount, int? clientId, int quantity)
        {
            this.ShouldPay = totalAmount;
            this.RealPay = totalAmount;
            this.Change = Decimal.Zero;
            this.ClientId = clientId;
            this.Quantity = quantity;
        }

        public void SetRealPay(string payStr)
        {
            Decimal result = new Decimal(0, 0, 0, false, (byte)2);
            if (Decimal.TryParse(payStr, out result))
                this.RealPay = result;
            else
                this.RealPay = this.ShouldPay;
        }

        public PayResultModel GetResultModel(string code = null)
        {
            if (this.IsBarcodePay() && (this._resultModel == null || this._resultModel.PayStatus != PayResultType.已支付))
                return this._resultModel;
            if (this._resultModel.PayStatus != PayResultType.已支付)
            {
                var pay = this.RealPay ?? Decimal.Zero;
                if (pay < this.ShouldPay)
                {
                    this.ErrorMessage = "支付金额不足";
                    this._resultModel.PayStatus = PayResultType.未支付;
                    return this._resultModel;
                }
                if (pay >= this.ShouldPay + 100)
                {
                    this.ErrorMessage = "支付金额不符合规则，找零不能超过100！";
                    this._resultModel.PayStatus = PayResultType.未支付;
                    return this._resultModel;
                }
                if (!this.IsCashPay && pay != this.ShouldPay)
                {
                    this.ErrorMessage = "非现金支付，支付金额必须与应付金额一致！";
                    this._resultModel.PayStatus = PayResultType.未支付;
                    return this._resultModel;
                }
                this._resultModel.PayStatus = PayResultType.已支付;
                this._resultModel.Change = this.Change;
                this._resultModel.ShouldPay = this.ShouldPay;
                this._resultModel.TotalPay = pay;
                if (this.IsCashPay)
                {
                    this._resultModel.SelectedPayInfos = new PayTypeDto()
                    {
                        Id = this.CurPayType.Id,
                        Code = this.CurPayType.Code
                    };
                }
                else if (pay > Decimal.Zero)
                {

                    this._resultModel.PayStatus = PayResultType.未支付;
                    return this._resultModel;

                }
            }
            if (this._resultModel.SelectedPayInfos != null)
                this._resultModel.SelectedPayInfos.Name = this.CurPayType?.Name;
            return this._resultModel;
        }
    }
}
