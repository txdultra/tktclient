using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Services;

namespace tktclient.Model
{
    public class PayResultModel
    {
        public PayResultType PayStatus { get; set; }

        public PayTypeDto SelectedPayInfos { get; set; }

        public Decimal Change { get; set; }

        public Decimal ShouldPay { get; set; }

        public Decimal TotalPay { get; set; }

        public Decimal DiscountSum { get; set; }
    }

    public enum PayResultType
    {
        未支付 = 0,
        已支付 = 2,
    }
}
