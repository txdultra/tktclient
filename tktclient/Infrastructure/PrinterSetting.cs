using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Db;

namespace tktclient.Infrastructure
{
    public class PrinterSetting
    {
        [Column("no")] public string No { get; set; }

        [Column("remain_tkts")] public int RemainTickets { get; set; }

        [Column("remain_rbns")] public int RemainRibbons { get; set; }

        [Column("last_time")] public DateTime LastTime { get; set; }
    }
}
