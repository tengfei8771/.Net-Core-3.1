using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace WebApi.Common
{
    public class GetNeedOrNeedNotFlag
    {
        public static bool GetFlag(HttpContext context)
        {
            bool flag = false;
            var Endpoint = HttpContextHelper.GetEndPoint(context);
            if (Endpoint == null)
            {
                return flag;
            }
            var attributes = Endpoint.Metadata;
            var NeedNot = attributes.GetMetadata<NeedNotValidateAttribute>();
            var Need = attributes.GetMetadata<NeedValidateAttribute>();
            int FirstIndex = -1;
            int SecondIndex = -1;
            if(FirstIndex == -1&& SecondIndex == -1)
            {
                return flag;
            }
            else
            {
                return FirstIndex > SecondIndex ? !NeedNot.NeedNotValidate : NeedNot.NeedNotValidate;
            }
        }
    }
}
