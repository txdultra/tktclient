using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using tktclient.Infrastructure;
using tktclient.Services;

namespace tktclient.Utils
{
    public class NoBuilder
    {
        private const string QRCODE_KEY = "h3HKH9d5mqP2g3jL";
        private const string AES_ENCODING = "utf-8";

        public static string NewOrderNo(DateTime date)
        {
            var ydm = date.ToString("yyyyMMdd");
            var clt = String.Format("{0:d4}", ClientContext.CurrentUser.Uid);
            //99代表所有换票的订单号
            return ydm + "99" + clt + new Random().Next(10000, 100000);
        }

        public static string NewQRCode(string orderNo, int childId, int sequence)
        {
            return orderNo + "-" + childId + "-" + sequence;
        }

        public static string QRCodeEncrypt(string code)
        {
            return AESEncrypt(code, QRCODE_KEY);
        }

        public static string AESEncrypt(string txt, string key)
        {
            var keyArray = Encoding.GetEncoding(AES_ENCODING).GetBytes(key);
            var toEncryptArray = Encoding.GetEncoding(AES_ENCODING).GetBytes(txt);
            using (var acsp = new AesCryptoServiceProvider { KeySize = 128, BlockSize = 128 })
            {
                acsp.GenerateIV();
                using (var aes = new AesCryptoServiceProvider
                    { Key = keyArray, IV = acsp.IV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    byte[] resultArray;
                    using (var cTransform = aes.CreateEncryptor())
                    {
                        resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    }
                    return Convert.ToBase64String(resultArray);
                }
            }
        }

        public static string AESDecrypt(string txt, string key)
        {
            var keyArray = Encoding.GetEncoding(AES_ENCODING).GetBytes(key);
            var toDecryptArray = Convert.FromBase64String(txt);
            using (var acsp = new AesCryptoServiceProvider { KeySize = 128, BlockSize = 128 })
            {
                acsp.GenerateIV();
                using (var aes = new AesCryptoServiceProvider { Key = keyArray, IV = acsp.IV, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    byte[] resultArray;
                    using (var cTransform = aes.CreateDecryptor())
                    {
                        resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                    }
                    return Encoding.GetEncoding(AES_ENCODING).GetString(resultArray);
                }
            }
        }
    }
}
