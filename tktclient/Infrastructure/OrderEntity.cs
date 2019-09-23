using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Db
{
    public class OrderEntity
    {
        [Column("id")] public int Id { get; set; }

        [Column("cloud_id")] public string CloudId { get; set; }

        [Column("order_no")]
        public string OrderNo { get; set; }

        [Column("nums")]
        public int Nums { get; set; }

        [Column("order_type")]
        public short OrderType { get; set; }

        [Column("amount")]
        public Decimal Amount { get; set; }

        [Column("per_nums")]
        public int PerNums { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("state")]
        public short State { get; set; }

        [Column("pay_type")]
        public int PayType { get; set; }

        [Column("real_pay")]
        public decimal RealPay { get; set; }

        [Column("change_pay")]
        public decimal ChangePay { get; set; }

        [Column("should_pay")]
        public decimal ShouldPay { get; set; }

        [Column("remark")]
        public string Remark { get; set; }

        [Column("excode")]
        public string ExCode { get; set; }

        [Column("excode_sync")]
        public bool ExCodeSync { get; set; }

        [Column("client_no")] public string ClientNo { get; set; }

        [Column("ext1")]
        public string Ext1 { get; set; }

        [Column("ext2")]
        public string Ext2 { get; set; }

        [Column("ext3")]
        public string Ext3 { get; set; }
    }
}
