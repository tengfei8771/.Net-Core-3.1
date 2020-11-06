using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class MenuInfo
    {
        public string SysCode { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIdUpper { get; set; }
        public string MenuIcon { get; set; }
        public string ModuleUrl { get; set; }
        public string ModuleRoute { get; set; }
        public string ModuleObj { get; set; }
        public string MenuProp { get; set; }
        public int? MenuOrder { get; set; }
    }
}
