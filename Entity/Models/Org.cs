using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Org
    {
        public string OrgId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string OrgShortName { get; set; }
        public string OrgCodeUpper { get; set; }
        public string Isinvalid { get; set; }
        public string Isdelete { get; set; }
        public string Remark { get; set; }
        public string OrgIdUpper { get; set; }
    }
}
