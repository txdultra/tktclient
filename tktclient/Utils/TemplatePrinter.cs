using FastReport;
using FastReport.Barcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printtest
{
    public enum PrintResultType
    {
        打印成功 = 1,
        打印失败 = 2,
    }
    public class TemplatePrinter
    {
        public static readonly string DocumentNameKey = "PrinterDocumentName";

        public PrintResultType PrintFrxTemplate(
          string frxFileName,
          string printerName,
          Dictionary<string, string> printData,
          ref string msg,
          Dictionary<string, IEnumerable> souceData = null,
          int printCount = 1)
        {
            msg = "打印成功!";
            if (!File.Exists(frxFileName))
            {
                msg = "模板或者打印机名称为空！";
                return PrintResultType.打印失败;
            }
            if (string.IsNullOrEmpty(printerName))
            {
                msg = "打印机未设置！";
                return PrintResultType.打印失败;
            }
            Report report = new Report();
            try
            {
                report.Load(frxFileName);
                foreach (KeyValuePair<string, string> keyValuePair1 in printData)
                {
                    if (!string.IsNullOrEmpty(keyValuePair1.Value))
                    {
                        if (keyValuePair1.Key.StartsWith("pic"))
                        {
                            Base @base = report.FindObject(keyValuePair1.Key);
                            if (@base != null)
                                ((PictureObject)@base).ImageLocation = keyValuePair1.Value;
                        }
                        else if (keyValuePair1.Key.StartsWith("txt"))
                        {
                            Base @base = report.FindObject(keyValuePair1.Key);
                            if (@base != null)
                                ((TextObjectBase)@base).Text = keyValuePair1.Value;
                        }
                        else if (keyValuePair1.Key.StartsWith("barCode"))
                        {
                            Base @base = report.FindObject(keyValuePair1.Key);
                            if (@base != null)
                                ((BarcodeObject)@base).Text = keyValuePair1.Value;
                        }
                    }
                    if (souceData != null)
                    {
                        foreach (KeyValuePair<string, IEnumerable> keyValuePair2 in souceData)
                            report.RegisterData(keyValuePair2.Value, keyValuePair2.Key);
                    }
                }
                if (printData.ContainsKey(TemplatePrinter.DocumentNameKey))
                    report.FileName = printData[TemplatePrinter.DocumentNameKey];
                else if (printData.ContainsKey("txtBarCode"))
                    report.FileName = printData["txtBarCode"];
                report.PrintSettings.ShowDialog = false;
                report.PrintSettings.Printer = printerName;
                new EnvironmentSettings().ReportSettings = new ReportSettings()
                {
                    ShowProgress = false
                };
                // ISSUE: explicit non-virtual call
                if ((souceData != null ? souceData.Count : 0) > 0)
                {
                    report.MaxPages = 1;
                    ReportPage page = (ReportPage)report.Pages[0];
                    // ISSUE: explicit non-virtual call
                    page.PaperHeight = (float)(100.0 + (souceData != null ? (double)souceData.Count : 0.0) * 12.5);
                    report.Pages[0] = (PageBase)page;
                }
                for (int index = 0; index < printCount; ++index)
                    report.Print();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return PrintResultType.打印失败;
            }
            finally
            {
                report.Dispose();
            }
            return PrintResultType.打印成功;
        }
    }
}
