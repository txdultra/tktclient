using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    public class SubOrderDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order_no")]
        public string OrderNo { get; set; }

        [JsonProperty("pay_time")]
        public DateTime? PayTime { get; set; }

        [JsonProperty("pay_no")]
        public string PayNo { get; set; }

        [JsonProperty("refund_time")]
        public DateTime? RefundTime { get; set; }

        [JsonProperty("refund_no")]
        public string RefundNo { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("scenic_id")]
        public int ScenicId { get; set; }

        [JsonProperty("scenic_sid")]
        public int ScenicSid { get; set; }

        [JsonProperty("ticket_id")]
        public int TicketId { get; set; }

        [JsonProperty("unit_price")]
        public Decimal UnitPrice { get; set; }

        [JsonProperty("total_price")]
        public Decimal TotalPrice { get; set; }

        [JsonProperty("nums")]
        public int Nums { get; set; }

        [JsonProperty("pernums")]
        public int PerNums { get; set; }

        [JsonProperty("state")]
        public short State { get; set; }

        [JsonProperty("use_date")]
        public DateTime? UseDate { get; set; }

        [JsonProperty("last_use_date")]
        public DateTime? LastUseDate { get; set; }

        [JsonProperty("card_type")]
        public short CardType { get; set; }

        [JsonProperty("user_card")]
        public string UserCard { get; set; }

        [JsonProperty("user_mobile")]
        public string UserMobile { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("cid")]
        public int Cid { get; set; }

        [JsonProperty("price_discount_type")]
        public short PriceDiscountType { get; set; }

        [JsonProperty("extra")]
        public dynamic Extra { get; set; }

        [JsonProperty("ticket_snapshot")]
        public TicketSnapshotDto Snapshot { get; set; }

        public int PrintedCount { get; set; }

        public string EnterTime { get; set; }
    }
}
