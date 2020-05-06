using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    public class TicketSnapshotDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enter_remark")]
        public string EnterRemark { get; set; }

        [JsonProperty("buy_remark")]
        public string BuyRemark { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<string> Tags { get; set; } = new List<string>();

        [JsonProperty("prices")]
        public IEnumerable<TicketPriceSnapshotDto> Prices { get; set; } = new List<TicketPriceSnapshotDto>();
    }

    public class TicketPriceSnapshotDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("channel_type")]
        public int? ChannelType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("original_price")]
        public double? OriginalPrice { get; set; }

        [JsonProperty("refund_type")]
        public int? RefundType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("use_date")]
        public TicketUseDateSnapshotDto UseDate { get; set; }
    }

    public class TicketUseDateSnapshotDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enter_time")]
        public int? EnterTime { get; set; }
    }
}
