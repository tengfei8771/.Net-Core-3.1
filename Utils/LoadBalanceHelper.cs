using Newtonsoft.Json.Linq;
using PublicWebApi.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class LoadBalanceHelper
    {
        public static ConnectConfig connectConfig = null;
        static LoadBalanceHelper()
        {
            JsonReaderHelper jsonReaderHelper = new JsonReaderHelper();
            JObject config = jsonReaderHelper.GetConfig("appsettings");
            connectConfig = config.GetValue("DBConfig").ToObject<ConnectConfig>();
        }
        public static string GetSlaveConnectionString(LoadBalanceType type)
        {
            if (connectConfig.DBCount == 1)
            {
                return connectConfig.MasterConnectionString;
            }
            else
            {
                return "";
            }
        }
        private static string GetSlaveConnectionStringByRoundRobin()
        {
            return "";
        }

        private static string GetSlaveConnectionStringWeight()
        {
            return "";
        }
    }

    public class ConnectConfig
    {
        public string DBType { get; set; }
        public int DBCount { get; set; }
        public string MasterConnectionString { get; set; }
        public List<Slave> Slave { get; set; }
    }
    public class Slave
    {
        public string ConnectionString { get; set; }
        public decimal weight { get; set; }
    }
    public enum LoadBalanceType
    {
        RoundRobin,
        Weight
    }
}
