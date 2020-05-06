using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Model
{
    public class TicketModel : ViewModelBase
    {
        private int _selectedCount;

        public int Id { get; set; }

        public int? EnterTime { get; set; }

        public string Name
        {
            get; set;
        }

        public int PerNumber { get; set; }

        public Decimal PriceRebate { get; set; }

        public double? OriginalPrice { get; set; }

        public int SelectedCount
        {
            get
            {
                return this._selectedCount;
            }
            set
            {
                this._selectedCount = value;
                this.RaisePropertyChanged(nameof(SelectedCount));
            }
        }
    }
}
