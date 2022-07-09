using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using printtest;
using tktclient.Db;
using tktclient.Infrastructure;
using tktclient.Services;

namespace tktclient.Utils
{
    public class Printer
    { 
        public static async Task<bool> Print(OrderEntity order, Action<int,int,bool> everyTktPrintResultAction)
        {
            var printer = new TemplatePrinter();
            var cfg = new Config();
            var childOrders = await Db.StorageProvider.GetChildOrdersByOrderId(order.Id);
            foreach (var stm in childOrders)
            {
                for (var i = 0; i < stm.Nums; i++)
                {
                    var qrCode = NoBuilder.NewQRCode(stm.OrderNo, stm.Id, i + 1);

                    var print = new TktPrint();
                    print.ChildId = stm.Id;
                    print.CloudOrderId = order.CloudId;
                    print.CloudSubId = stm.CloudId;
                    print.OrderId = order.Id;
                    print.OrderNo = order.OrderNo;
                    print.QrCode = qrCode;
                    print.Printed = false;
                    var ok = await StorageProvider.SavePrint(print);
                    if (!ok)
                    {
                        NavigationServiceHelper.NoticeWarn("打印失败(保存数据失败)", "数据", true, 5);
                        return false;
                    }


                    var frxFileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                                      string.Format("PrinterTemplates\\{0}.frx", cfg.GetPrinterTemplateFileName());
                    string codePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                                      "PrinterTemplates\\Barcodes\\" + qrCode + ".png";
                    var encryptCode = NoBuilder.QRCodeEncrypt(qrCode);
                    QRCodeGenerator.GenerateQRBarCode(encryptCode, codePath);
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("txtTicketModelInfo", stm.TicketName);
                    //dictionary.Add("txtTotalMoney", "￥"+stm.UnitPrice + "元/" + stm.PerNums + "人");
                    if (stm.OriPrice > 0)
                        dictionary.Add("txtTotalMoney", "￥" + stm.OriPrice * stm.Nums + "元/" + stm.PerNums + "人");
                    else
                        dictionary.Add("txtTotalMoney", stm.PerNums + "人");
                    dictionary.Add("txtBarCode", "");
                    dictionary.Add("picBarCode", codePath);
                    dictionary.Add("txtAccountName", ClientContext.CurrentUser.UserName);
                    dictionary.Add("txtPeopleCount", stm.PerNums.ToString());
                    dictionary.Add("txtAccountNo", "");
                    dictionary.Add("txtTradeNO", stm.OrderNo);
                    dictionary.Add("txtSubId", stm.Id.ToString());
                    dictionary.Add("txtDescription", "");
                    dictionary.Add("txtInvalidDate", DateUtil.GetDateTime(stm.UseDate).ToString("yy/MM/dd"));
                    dictionary.Add("txtSaleDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (stm.EnterTime != null)
                    {
                        dictionary.Add("txtEnterTime", DateUtil.SecondToTimeSpan(stm.EnterTime.Value).ToString(@"hh\:mm"));
                    }

                    string message = "";


                    var printResult = printer.PrintFrxTemplate(frxFileName, cfg.GetPrintName(), dictionary, ref message, null, 1);
                    if (printResult == PrintResultType.打印成功)
                    {
                        var childOrder = await Db.StorageProvider.GetChildOrder(stm.Id);
                        childOrder.Prints++;
                        await Db.StorageProvider.UpdateChildOrder(childOrder);
                        print.Printed = true;
                        print.PrintTime = DateTime.Now;
                        await Db.StorageProvider.UpdatePrint(print);

                        everyTktPrintResultAction?.Invoke(stm.Id, i, true);
                    }
                    else
                    {
                        NavigationServiceHelper.NoticeWarn("打印失败(请检查设备是否正常)", "打印", true, 5);
                        return false;
                    }
                }
            }
            order.State = (int)OrderStates.已打印;
            await Db.StorageProvider.UpdateOrder(order);

            if (!string.IsNullOrEmpty(order.ExCode))
            {
                var result = TktService.VerifyCode(order.ExCode);
                if (result.Code == Result.RESULT_SUCCESS)
                {
                    order.ExCodeSync = true;
                    await Db.StorageProvider.UpdateOrder(order);
                }
            }

            return true;
        }

        public static async Task ResetRemain(ResetRemainType rrt)
        {
            var cfg = new Config();
            var pstting = await StorageProvider.GetPrinterSetting(cfg.GetPrinterNo());
            if (pstting == null)
            {
                return;
            }

            var ok = false;
            switch (rrt)
            {
                case ResetRemainType.纸质票:
                    ok = await StorageProvider.SetRemainTickets(cfg.GetPrinterNo(), cfg.GetInitPrinterTickets());
                    break;
                case ResetRemainType.碳带:
                    ok = await StorageProvider.SetRemainRibbons(cfg.GetPrinterNo(), cfg.GetInitPrinterRibbons());
                    break;
            }
            
            if (ok)
            {
                var log = new PrinterLog();
                log.PrinterNo = pstting.No;
                log.RemainTickets = pstting.RemainTickets;
                log.RemainRibbons = pstting.RemainRibbons;
                log.ResetTime = DateTime.Now;
                await StorageProvider.SavePrinterLog(log);
            }
        }

        public enum ResetRemainType
        {
            纸质票,
            碳带
        }
    }
}
