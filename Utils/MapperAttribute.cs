using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public class MapperAttribute: Attribute
    {
        public string MapperName { get; private set; }
        public bool IgnoreColumn { get; private set; }
        /// <summary>
        /// 只有字段映射的字段名
        /// </summary>
        /// <param name="MapperName">映射名称</param>
        public MapperAttribute(string MapperName)
        {
            this.MapperName = MapperName;
        }
        /// <summary>
        /// 只有字段映射的字段名和是否忽略的构造函数
        /// </summary>
        /// <param name="MapperName">映射名称</param>
        /// <param name="IgnoreColumn">是否忽略</param>
        public MapperAttribute(string MapperName,bool IgnoreColumn)
        {
            this.MapperName = MapperName;
            this.IgnoreColumn = IgnoreColumn;
        }
    }
}
