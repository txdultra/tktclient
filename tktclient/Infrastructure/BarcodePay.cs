using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Infrastructure;
using tktclient.Services;

namespace tktclient.Model
{
    public class BarcodePay
    {
        public int? ClientId { get; set; }

        public int TradeType { get; set; }

        public string OrderNo { get; set; }

        public string AuthCode { get; set; }

        public IList<PayTypeDto> AllPayTypes { get; set; } = new List<PayTypeDto>();

        public string GoodShortName { get; set; }

        public string GoodFullName { get; set; }

        public Decimal TotalFee { get; set; }

        public BarcodePayTypes PayType { get; set; }

        public string PayCode { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
