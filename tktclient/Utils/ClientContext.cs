using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Db;
using tktclient.Services;

namespace tktclient.Utils
{
    public class ClientContext
    {
        private static Config cfg = new Config();
        public static ClientUserDto CurrentUser { get; set; }

        public static int RemainTickets
        {
            get
            {
                var remain = StorageProvider.GetPrinterSetting(cfg.GetPrinterNo()).Result;
                if (remain != null) return remain.RemainTickets;
                return 0;
            }
            set
            {
                var ok = StorageProvider.SetRemainTickets(cfg.GetPrinterNo(),value).Result;
                if (ok)
                {

                }
            }
        }

        public static int RemainRibbons
        {
            get
            {
                var remain = StorageProvider.GetPrinterSetting(cfg.GetPrinterNo()).Result;
                if (remain != null) return remain.RemainRibbons;
                return 0;
            }
            set
            {
                var ok = StorageProvider.SetRemainRibbons(cfg.GetPrinterNo(), value).Result;
                if (ok)
                {

                }
            }
        }
    }
}
