using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Utils
{
    public class UploadFileHelper
    {
        public JArray UploadFile(IFormFileCollection files)
        {
            try
            {
                JArray Filearr = new JArray();
                string BasePath = Directory.GetCurrentDirectory();
                foreach (var file in files)
                {
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss") + file.FileName;
                    while (File.Exists(BasePath + "/UploadFiles/BusinessCheckFile/" + name))
                    {
                        name = file.FileName + DateTime.Now.ToString("yyyyMMddhhmmss");
                    }
                    using (FileStream fs =File.Create(BasePath + "/UploadFiles/BusinessCheckFile/" + name))
                    {
                        file.CopyTo(fs);
                        fs.Close();
                    }
                    JObject obj = new JObject()
                    {
                        {"ID",Guid.NewGuid() },
                        {"FILE_NAME",file.Name },
                        {"FILE_URL", "/UploadFiles/BusinessCheckFile/" + name},
                        {"CREATEDATE",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") }
                    };
                    Filearr.Add(obj);
                }
                return Filearr;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
