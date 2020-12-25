using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 二维码生成帮助类
    /// </summary>
    public class QRCodeHelper
    {
        /// <summary>
        /// 生成svg格式的二维码图片
        /// </summary>
        /// <param name="plainText">转化为二维码的内容</param>
        /// <param name="pixel">像素</param>
        /// <returns></returns>
        public static string CreateQRCodeBySvg(string plainText, string savePath,int pixel=10)
        {
            var generator = new QRCodeGenerator();
            var qrCodeData = generator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new SvgQRCode(qrCodeData);
            return qrCode.GetGraphic(pixel);
        }
    }
}
