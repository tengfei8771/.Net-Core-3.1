using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class NeedNotValidateAttribute:Attribute
    {
        public bool NeedNotValidate = true;
        public NeedNotValidateAttribute(bool NeedNotValidate=true)
        {
            this.NeedNotValidate = NeedNotValidate;
        }
    }
}
