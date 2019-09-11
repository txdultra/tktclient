using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    public class OrderDto
    {
        [JsonProperty("order_no")]
        public string OrderNo { get; set; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("pay_time")]
        public DateTime? PayTime { get; set; }

        [JsonProperty("pay_no")]
        public string PayNo { get; set; }

        [JsonProperty("refund_time")]
        public DateTime? RefundTime { get; set; }

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("childs")]
        public int Childs { get; set; }

        [JsonProperty("state")]
        public short State { get; set; }

        [JsonProperty("extra")] public OrderExtraDto Extra { get; set; }

        [JsonProperty("sub_orders")]
        public IEnumerable<SubOrderDto> SubOrders { get; set; } = new List<SubOrderDto>();

        public int Prints { get; set; }

    }

    public class OrderExtraDto
    {
        [JsonProperty("code_use_date")]
        public string CodeUseDate { get; set; }

        [JsonProperty("code_use_state")]
        public short CodeUseState { get; set; }

        [JsonProperty("ticket_code")]
        public string TicketCode { get; set; }
    }
}
