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
    }
}
