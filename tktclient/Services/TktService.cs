using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace tktclient.Services
{
    class TktService
    {
        public static readonly Config cfg = new Config();
        public static readonly IReadOnlyDictionary<string, string> B2Bs = new Dictionary<string, string>()
        {
            {"携程","2" },
            {"驴妈妈","6" },
            {"同城","7" },
            {"美团","8" },
            {"今启网","11" },
            {"南通濠滨网","12"},
            {"来这选","13" }
        };

        public static string GetB2BName(string value)
        {
            foreach (var b2b in B2Bs)
            {
                if (b2b.Value == value)
                    return b2b.Key;
            }
            return "未知";
        }

        private static RestClient GetClint()
        {
            return new RestClient(cfg.GetTktServiceAddress());
            ;
        }

        public static ResultT<List<TicketDto>> GetTickets(string partnerIds, string b2bValue = null)
        {
            //var partnerId = cfg.GetTktServiceValue("TktPartners");
            var pids = partnerIds;
            if (!string.IsNullOrEmpty(b2bValue))
            {
                switch (b2bValue)
                {
                    case "2":
                        pids = cfg.GetTktServiceValue("CtripPid");
                        break;
                    case "6":
                        pids = cfg.GetTktServiceValue("LumamaPid");
                        break;
                    case "7":
                        pids = cfg.GetTktServiceValue("TongchengPid");
                        break;
                    case "8":
                        pids = cfg.GetTktServiceValue("MeituanPid");
                        break;
                    case "11":
                        pids = cfg.GetTktServiceValue("JinqiPid");
                        break;
                    case "12":
                        pids = cfg.GetTktServiceValue("NthbPid");
                        break;
                    case "13":
                        pids = cfg.GetTktServiceValue("LzxPid");
                        break;
                }
            }
            var partners = new List<string>();
            var tmpPartners = pids.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (tmpPartners.Length > 1)
            {
                partners = tmpPartners.ToList();
            }
            else
            {
                partners.Add(pids);
            }

            ResultT<List<TicketDto>> result = null;
            foreach (var pid in partners)
            {
                var http = GetClint();
                var request = new RestRequest("v1/ticket/search");
                var date = DateTime.Now.ToString("yyyyMMdd");
                try
                {
                    request.AddParameter("cid", cfg.GetTktServiceValue("Cid"));
                    request.AddParameter("scenic_sid", cfg.GetTktServiceValue("ScenicSpotId"));
                    request.AddParameter("partner_id", pid);
                    request.AddParameter("date", date);
                    var r = http.Execute(request, Method.GET);
                    if (result == null)
                    {
                        result = JsonConvert.DeserializeObject<ResultT<List<TicketDto>>>(r.Content);
                    }
                    else
                    {
                        var tds = JsonConvert.DeserializeObject<ResultT<List<TicketDto>>>(r.Content);
                        if (tds.Code == Result.RESULT_SUCCESS)
                        {
                            result.data.AddRange(tds.data);
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ResultT<List<TicketDto>> {Code = Result.NETWORK_FAIL, Msg = e.Message};
                }
            }

            return result;
        }

        public static ResultT<ClientUserDto> Login(string userName, string pwd)
        {
            var http = GetClint();
            var request = new RestRequest("v1/client/login");
            try
            {
                request.AddParameter("user_name",userName);
                request.AddParameter("pwd",pwd);
                var result = http.Execute(request,Method.POST);
                return JsonConvert.DeserializeObject<ResultT<ClientUserDto>>(result.Content);
            }
            catch (Exception e)
            {
                return new ResultT<ClientUserDto> { Code = Result.NETWORK_FAIL, Msg = e.Message };
            }
        }

        public static string TicketHealthCheck()
        {
            var http = GetClint();
            var request = new RestRequest("v1/common/health");
            try
            {
                var result = http.Execute(request, Method.GET);
                return result.Content;
            }
            catch (Exception e)
            {
                return "fail";
            }
        }

        public static ResultT<OrderDto> GetOrderByCode(string code)
        {
            var http = GetClint();
            var request = new RestRequest("v1/order/detail/code/{code}");
            try
            {
                request.AddUrlSegment("code", code);
                var result = http.Execute(request,Method.GET);
                return JsonConvert.DeserializeObject<ResultT<OrderDto>>(result.Content);
            }
            catch (Exception e)
            {
                return new ResultT<OrderDto> { Code = Result.NETWORK_FAIL, Msg = e.Message };
            }
        }

        public static Result VerifyCode(string code)
        {
            var http = GetClint();
            var request = new RestRequest("v1/order/code/verify/{code}");
            try
            {
                request.AddUrlSegment("code", code);
                var result = http.Execute(request, Method.GET);
                return JsonConvert.DeserializeObject<Result>(result.Content);
            }
            catch (Exception e)
            {
                return new Result { Code = Result.NETWORK_FAIL, Msg = e.Message };
            }
        }
    }

   
}
