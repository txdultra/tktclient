using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Utils
{
    class QRCodeGenerator
    {
        public static bool GenerateQRBarCode(string barcode, string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    new GraphicsRenderer((ISizeCalculation)new FixedModuleSize(4, QuietZoneModules.Zero), Brushes.Black, Brushes.White).WriteToStream(new QrEncoder(ErrorCorrectionLevel.M).Encode(barcode).Matrix, ImageFormat.Png, (Stream)fileStream);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
