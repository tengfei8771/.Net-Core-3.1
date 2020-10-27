using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class NeedValidateAttribute:Attribute
    {
        public bool NeedValidate;
        public NeedValidateAttribute(bool NeedValidate = true)
        {
            this.NeedValidate = NeedValidate;
        }
    }
}
