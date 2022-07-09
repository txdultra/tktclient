using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace tktclient.Model
{
    public class SelectedTicketModel : ViewModelBase
    {
        private int _number = 1;
        private int _maxNum = 5000;
        private Decimal _realPrice;
        private int _visitorCount;
        private Decimal _sumPrice;
        private string _BarcodeRange;

        public event Action<SelectedTicketModel> SumChangeEvent;

        public int TicketId { get; set; }

        public int TicketPid { get; set; }

        public string EnterTime { get; set; }

        public string BarcodeTypeName { get; set; }

        public string TicketModelName { get; set; }

        public string TicketKindName { get; set; }

        public int PerNumber { get; set; }

        public double? OriginalPrice { get; set; }

        public Decimal RealPrice
        {
            get
            {
                return this._realPrice;
            }
            set
            {
                this._realPrice = value;
                this.RaisePropertyChanged(nameof(RealPrice));
            }
        }

        public int OldNumber { get; set; }

        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
                this.RaisePropertyChanged(nameof(Number));
            }
        }

        public int MaxNum
        {
            get
            {
                return this._maxNum;
            }
            set
            {
                this._maxNum = value;
                this.RaisePropertyChanged(nameof(MaxNum));
            }
        }

        public int VisitorCount
        {
            get
            {
                return this._visitorCount;
            }
            set
            {
                this._visitorCount = value;
                this.RaisePropertyChanged(nameof(VisitorCount));
            }
        }

        public Decimal SumPrice
        {
            get
            {
                return this._sumPrice;
            }
            set
            {
                this._sumPrice = value;
                this.RaisePropertyChanged(nameof(SumPrice));
            }
        }

        public string BarcodeRange
        {
            get
            {
                return this._BarcodeRange;
            }
            set
            {
                this._BarcodeRange = value;
                this.RaisePropertyChanged(nameof(BarcodeRange));
            }
        }

        public IList<string> Barcodes { get; set; }

        public int MediumNumber { get; set; }

        public bool IsContractTicet
        {
            get
            {
                if (!this.ContractPolicyId.HasValue)
                    return false;
                Guid? contractPolicyId = this.ContractPolicyId;
                Guid empty = Guid.Empty;
                if (!contractPolicyId.HasValue)
                    return true;
                if (!contractPolicyId.HasValue)
                    return false;
                return contractPolicyId.GetValueOrDefault() != empty;
            }
        }

        public Guid? ContractPolicyId { get; set; }

        private void InvokeChange()
        {
            this.SumPrice = this.RealPrice * (Decimal)this.Number;
            Action<SelectedTicketModel> sumChangeEvent = this.SumChangeEvent;
            if (sumChangeEvent == null)
                return;
            sumChangeEvent(this);
        }
    }
}
