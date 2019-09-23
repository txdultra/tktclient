using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Db;

namespace tktclient.Infrastructure
{
    public class PrinterLog
    {
        [Column("id")] public int Id { get; set; }

        [Column("printer_no")] public string PrinterNo { get; set; }

        [Column("reset_time")] public DateTime ResetTime { get; set; }

        [Column("remain_tkts")] public int RemainTickets { get; set; }

        [Column("remain_rbns")] public int RemainRibbons { get; set; }
    }
}
