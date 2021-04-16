using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace SqlSugarAndEntity
{
    public static class DataBaseConfig
    {
        public static ConnectionConfig _config = null;
        static DataBaseConfig()
        {
            GetConfig();
        }
        private static void GetConfig()
        {
            var _builder = new ConfigurationBuilder();
            var config = _builder.Add(new JsonConfigurationSource { Path = "DBConfig.json", Optional = false, ReloadOnChange = true }).Build();
            _config = new ConnectionConfig();
            _config.ConnectionString = config.GetSection($"MasterConnetion").Value;
            _config.IsAutoCloseConnection = true;
            string DBType = config.GetSection("DBType").ToString().ToUpper();
            int SlaveCount = Convert.ToInt32(config.GetSection("SlaveCount"));
            switch (DBType)
            {
                case "SQLSERVER":
                    _config.DbType = DbType.SqlServer;
                    break;
                case "MYSQL":
                    _config.DbType = DbType.MySql;
                    break;
                case "ORACLE":
                    _config.DbType = DbType.Oracle;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (SlaveCount > 0)
            {
                _config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>();
                for(int i=0;i< SlaveCount; i++)
                {
                    SlaveConnectionConfig slaveConnectionConfig = new SlaveConnectionConfig()
                    {
                        HitRate = config.GetSection($"SlaveConnetions:{i}").GetValue<int>("HitRate"),
                        ConnectionString = config.GetSection($"SlaveConnetions:{i}").GetValue<string>("ConnectionString")
                    };
                    _config.SlaveConnectionConfigs.Add(slaveConnectionConfig);
                }
            }
        }
    }
}
