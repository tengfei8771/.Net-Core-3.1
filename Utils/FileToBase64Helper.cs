using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Utils
{
    public class FileToBase64Helper
    {
        public string IFormToBase64(IFormFile File)
        {

            var fs = File.OpenReadStream();
            byte[] bs = new byte[fs.Length];
            int log = Convert.ToInt32(fs.Length);
            fs.Read(bs, 0, log);
            string Base64Main= Convert.ToBase64String(bs);
            string header = "data:" + File.ContentType.ToLower() + ";base64,";
            return header + Base64Main;
        }
    }
}
