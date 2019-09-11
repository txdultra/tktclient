using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Db
{
    public class ChildOrderEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cloud_id")] public string CloudId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("order_no")]
        public string OrderNo { get; set; }

        [Column("order_type")]
        public short OrderType { get; set; }

        [Column("tkt_id")]
        public int TicketId { get; set; }

        [Column("tkt_name")]
        public string TicketName { get; set; }

        [Column("amount")]
        public Decimal Amount { get; set; }

        [Column("unit_price")]
        public Decimal UnitPrice { get; set; }

        [Column("nums")]
        public int Nums { get; set; }
        
        [Column("per_nums")]
        public int PerNums { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("is_sync")]
        public int IsSync { get; set; }

        [Column("prints")]
        public int Prints { get; set; }

        [Column("use_date")]
        public int UseDate { get; set; }
    }
}
