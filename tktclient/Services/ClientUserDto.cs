using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    public class ClientUserDto
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("serial_no")]
        public string SerialNo { get; set; }
    }
}
