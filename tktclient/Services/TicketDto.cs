using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    public class TicketDto
    {
        [JsonProperty("id")]
        public int Id;
        [JsonProperty("name")]
        public String Name;
        [JsonProperty("per_nums")]
        public int PerNums;
        [JsonProperty("enter_remeark")]
        public String EnterRemark;
        [JsonProperty("buy_remark")]
        public String BuyRemark;
        [JsonProperty("stocks")]
        public int Stocks;
        [JsonProperty("sold_count")]
        public int SoldCount;
        [JsonProperty("state")]
        public short State;
        [JsonProperty("icon_url")]
        public String IconUrl;
        [JsonProperty("tags")]
        public String[] Tags;
        [JsonProperty("extra")]
        public dynamic Extra;

        [JsonProperty("prices")]
        public List<TicketPrice> Prices;
    }

    public class TicketPrice
    {
        [JsonProperty("id")]
        public int Id;
        [JsonProperty("tid")]
        public int TicketId;
        [JsonProperty("channel_type")]
        public short ChannelType;
        [JsonProperty("name")]
        public String Name;
        [JsonProperty("price")]
        public double price;
        [JsonProperty("stocks")]
        public int Stocks;
        [JsonProperty("sold_count")]
        public int SoldCount;
        [JsonProperty("state")]
        public short State;
        [JsonProperty("refund_type")]
        public short RefundType;
        [JsonProperty("title")]
        public String Title;
        [JsonProperty("notice_remark")]
        public String NoticeRemark;
        [JsonProperty("remark")]
        public String Remark;
        [JsonProperty("description")]
        public String Description;
        [JsonProperty("extra")]
        public dynamic extra;
    }
}
