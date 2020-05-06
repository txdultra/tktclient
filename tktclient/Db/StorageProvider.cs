using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FastReport.DevComponents.DotNetBar.Rendering;
using FastReport.Utils;
using MySql.Data.MySqlClient;
using tktclient.Infrastructure;
using tktclient.Utils;
using Config = tktclient.Services.Config;

namespace tktclient.Db
{
    public class StorageProvider
    {
        private static readonly string provider;
        private static readonly string connectionString;
        private const string DB_SECRET_KEY = "m9HAHSczXYfo68mu";
        static StorageProvider()
        {
            var cfg = new Config();
            provider = cfg.GetDbProvider();
            Dapper.SqlMapper.SetTypeMap(typeof(OrderEntity), new ColumnAttributeTypeMapper<OrderEntity>());
            Dapper.SqlMapper.SetTypeMap(typeof(ChildOrderEntity), new ColumnAttributeTypeMapper<ChildOrderEntity>());
            Dapper.SqlMapper.SetTypeMap(typeof(PrinterSetting), new ColumnAttributeTypeMapper<PrinterSetting>());
            Dapper.SqlMapper.SetTypeMap(typeof(PrinterLog), new ColumnAttributeTypeMapper<PrinterLog>());
            Dapper.SqlMapper.SetTypeMap(typeof(TktPrint), new ColumnAttributeTypeMapper<TktPrint>());
            connectionString = ResolveConnectionString(cfg.DbConnectionString());

            //var nnn = "server=58.221.97.174;database=tktdb;uid=tktsrv;pwd=txd123;charset='utf8';SslMode = none;port=3306;";
            //var enstr = NoBuilder.AESEncrypt(nnn, DB_SECRET_KEY);
            //var xx = enstr.Length;
        }

        public static IDbTransaction GetTransaction()
        {
            var conn = GetConnection();
            return conn.BeginTransaction();
        }

        private static string ResolveConnectionString(string connStr)
        {
            switch (provider.ToUpper())
            {
                case "SQLITE":
                    return "data source=" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                           connStr + ";version=3;";
                case "MYSQL":
                    return NoBuilder.AESDecrypt(connStr, DB_SECRET_KEY);
            }
            throw new ArgumentException("不支持" + provider + "存储提供器");
        }

        internal static IDbConnection GetConnection()
        {
            switch (provider.ToUpper())
            {
                case "SQLITE":
                    return null;//new SQLiteConnection(connectionString);
                case "MYSQL":
                    return new MySqlConnection(connectionString);
            }
            throw new ArgumentException("不支持"+provider+"存储提供器");
        }

        public static async Task<bool> SaveOrder(OrderEntity order)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql = "insert into orders(cloud_id,order_no,nums,order_type,amount,per_nums,create_time,state,pay_type,real_pay,change_pay,should_pay,excode,excode_sync,remark,client_no,ext1,ext2,ext3)" +
                          " values(@CloudId,@OrderNo,@Nums,@OrderType,@Amount,@PerNums,@CreateTime,@State,@PayType,@RealPay,@ChangePay,@ShouldPay,@ExCode,@ExCodeSync,@Remark,@ClientNo,@Ext1,@Ext2,@Ext3);" +
                          "select last_insert_id();";
                var result = await conn.ExecuteScalarAsync<int>(sql, order);
                if (result > 0)
                {
                    order.Id = result;
                    return true;
                }
                return false;
            }
        }

        public static async Task<bool> UpdateOrder(OrderEntity order)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql = "update orders set cloud_id=@CloudId, order_no=@OrderNo,nums=@Nums,order_type=@OrderType,amount=@Amount,per_nums=@PerNums,create_time=@CreateTime," +
                    "state=@State,pay_type=@PayType,real_pay=@RealPay,change_pay=@ChangePay,should_pay=@ShouldPay,excode=@ExCode,excode_sync=@ExCodeSync,remark=@Remark,client_no=@ClientNo,ext1=@Ext1,ext2=@Ext2,ext3=@Ext3" +
                    " where id=@Id";
                var result = await conn.ExecuteAsync(sql, order);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task<OrderEntity> GetOrder(int id)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from orders where id=" + id;
                return await conn.QueryFirstOrDefaultAsync<OrderEntity>(sql);
            }
        }

        public static async Task<OrderEntity> GetOrderByCloudId(string cloudId)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from orders where cloud_id='" + cloudId  + "'";
                return await conn.QueryFirstOrDefaultAsync<OrderEntity>(sql);
            }
        }

        public static async Task<int> GetOrderByExCodeCount(string excode, OrderTypes orderType)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select count(0) from orders where excode='" + excode + "' and order_type=" + (int)orderType;
                return await conn.ExecuteScalarAsync<int>(sql);
            }
        }

        public static async Task<bool> SaveChildOrder(ChildOrderEntity childOrder)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql = "insert into child_orders(cloud_id,order_id,order_no,order_type,tkt_id,tkt_name,amount,unit_price,nums,per_nums,create_time,is_sync,prints,use_date,enter_time)" +
                          " values(@CloudId,@OrderId,@OrderNo,@OrderType,@TicketId,@TicketName,@Amount,@UnitPrice,@Nums,@PerNums,@CreateTime,@IsSync,@Prints,@UseDate,@EnterTime);" +
                          "select last_insert_id();";
                var result = await conn.ExecuteScalarAsync<int>(sql, childOrder);
                if (result > 0)
                {
                    childOrder.Id = result;
                    return true;
                }
                return false;
            }
        }

        public static async Task<bool> UpdateChildOrder(ChildOrderEntity order)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "update child_orders set cloud_id=@CloudId,order_id=@OrderId,order_no=@OrderNo,order_type=@OrderType,tkt_id=@TicketId,tkt_name=@TicketName,amount=@Amount," +
                    "unit_price=@UnitPrice,nums=@Nums,per_nums=@PerNums,create_time=@CreateTime,is_sync=@IsSync,prints=@Prints,use_date=@UseDate,enter_time=@EnterTime where id=@Id";
                var result = await conn.ExecuteAsync(sql, order);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task<int> GetPrints(string orderNo)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select sum(prints) from child_orders where order_no='" + orderNo + "'";
                return await conn.QuerySingleAsync<int>(sql);
            }
        }

        public static async Task<ChildOrderEntity> GetChildOrder(int id)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from child_orders where id=" + id;
                return await conn.QueryFirstOrDefaultAsync<ChildOrderEntity>(sql);
            }
        }

        public static async Task<ChildOrderEntity> GetChildOrderByCloudId(string cloudId)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from child_orders where cloud_id='" + cloudId + "'";
                return await conn.QueryFirstOrDefaultAsync<ChildOrderEntity>(sql);
            }
        }

        public static async Task<IEnumerable<ChildOrderEntity>> GetChildOrdersByOrderId(int orderId)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from child_orders where order_id='" + orderId + "'";
                return await conn.QueryAsync<ChildOrderEntity>(sql);
            }
        }


        public static async Task<bool> SavePrint(TktPrint print)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql = "insert into tkt_prints(child_id,cloud_order_id,cloud_sub_id,order_id,order_no,qrcode,printed,print_time)" +
                          " values(@ChildId,@CloudOrderId,@CloudSubId,@OrderId,@OrderNo,@QrCode,@Printed,@PrintTime);" +
                          "select last_insert_id();";
                var result = await conn.ExecuteScalarAsync<int>(sql, print);
                if (result > 0)
                {
                    print.Id = result;
                    return true;
                }
                return false;
            }
        }

        public static async Task<bool> UpdatePrint(TktPrint print)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "update tkt_prints set qrcode=@QrCode,printed=@Printed,print_time=@PrintTime where id=@Id";
                var result = await conn.ExecuteAsync(sql, print);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task<TktPrint> GetTktPrint(int id)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from tkt_prints where id=" + id;
                return await conn.QueryFirstOrDefaultAsync<TktPrint>(sql);
            }
        }

        public static async Task<bool> SetRemainTickets(string printerNo,int nums)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "update printers set remain_tkts="+ nums + " where no='" + printerNo + "'";
                var result = await conn.ExecuteAsync(sql);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task<bool> SetRemainRibbons(string printerNo, int nums)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "update printers set remain_rbns=" + nums + " where no='" + printerNo + "'";
                var result = await conn.ExecuteAsync(sql);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task<PrinterSetting> GetPrinterSetting(string no)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql =
                    "select * from printers where no='" + no + "'";
                return await conn.QueryFirstOrDefaultAsync<PrinterSetting>(sql);
            }
        }

        public static async Task<bool> SavePrinterLog(PrinterLog log)
        {
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                var sql = "insert into printers_log(printer_no,reset_time,remain_tkts,remain_rbns)" +
                          " values(@PrinterNo,@ResetTime,@RemainTickets,@RemianRibbons);" +
                          "select last_insert_id();";
                var result = await conn.ExecuteScalarAsync<int>(sql, log);
                if (result > 0)
                {
                    log.Id = result;
                    return true;
                }
                return false;
            }
        }
    }
}
