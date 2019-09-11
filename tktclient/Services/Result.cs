
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tktclient.Services
{
    
    public class Result
    {
        [JsonProperty("code")]
        public String Code;
        [JsonProperty("msg")]
        public String Msg;

        public const string RESULT_SUCCESS = "success";
        public const string NETWORK_FAIL = "networkfail";
    }

    public class ResultT<T> : Result
    {
        [JsonProperty("data")]
        public T data;
    }
}
