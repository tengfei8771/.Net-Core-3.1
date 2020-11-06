using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Accesstoken
    {
        public string UserId { get; set; }
        public string AccessToken1 { get; set; }
        public DateTime? ExpiredTime { get; set; }
    }
}
