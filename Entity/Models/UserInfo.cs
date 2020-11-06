using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class UserInfo
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public int? UserSex { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneOffice { get; set; }
        public string UserEmail { get; set; }
        public DateTime? RegTime { get; set; }
        public int? Flag { get; set; }
        public string UserDomain { get; set; }
        public string Remark { get; set; }
        public string AssociatedAccount { get; set; }
        public int? AuthenticationType { get; set; }
        public int? UserType { get; set; }
        public string UserIp { get; set; }
    }
}
