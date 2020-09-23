using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PublicWebApi.Common
{
    public class WhiteListHelper
    {

        public static bool IsPass(HttpContext context)
        {
            string RequestPath = context.Request.Path.Value;
            JsonReaderHelper jsonReaderHelper = new JsonReaderHelper();
            List<string> WhiteList = jsonReaderHelper.GetValue<List<string>>("appsettings", "WhiteList");
            foreach(string white in WhiteList)
            {
                if (RequestPath.StartsWith(white))
                {
                    return true;
                }
            }
            return false;
        }
        
    }

}

