using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tktclient.Db;

namespace tktclient.Infrastructure
{
    public class TktPrint
    {
        [Column("id")] public int Id { get; set; }

        [Column("child_id")] public int ChildId { get; set; }

        [Column("cloud_order_id")] public string CloudOrderId { get; set; }

        [Column("cloud_sub_id")] public string CloudSubId { get; set; }

        [Column("order_id")] public int OrderId { get; set; }

        [Column("order_no")] public string OrderNo { get; set; }

        [Column("qrcode")] public string QrCode { get; set; }

        [Column("printed")] public bool Printed { get; set; }

        [Column("print_time")] public DateTime? PrintTime { get; set; }

    }
}
