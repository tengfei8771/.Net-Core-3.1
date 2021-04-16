using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarAndEntity
{
    public class BaseMethod : IBaseMethod
    {
        private SqlSugarClient GetClient()=> new SqlSugarClient(DataBaseConfig._config);

        public ISqlSugarClient Db() => GetClient();

        public IAdo Sql() => GetClient().Ado;
    }
}
