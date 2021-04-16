using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarAndEntity
{
    public interface IBaseMethod
    {
        IAdo Sql();

        ISqlSugarClient Db();
    }
}
