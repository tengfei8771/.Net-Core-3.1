using Entity.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utils;
using System.Collections.Generic;
using System.Linq;

namespace Entity
{
    public static class EFCoreExtensions
    {
        public static DBConfig config = null;
        /// <summary>
        /// 把DBContext按照数据库类型注入到服务中
        /// </summary>
        /// <param name="service"></param>
        public static void AddContext(this IServiceCollection service)
        {
            GetConfig();
            service.AddDbContext<AppDBContext>(options =>
            {
                switch (config.DBType.ToUpper())
                {
                    case "SQLSEVER":
                        //options.UseSqlServer("server=localhost;user id=sa;pwd=sa;database=AppDB");
                        options.UseSqlServer(config.MasterConnetion);
                        break;
                    case "ORACLE":
                        options.UseOracle(config.MasterConnetion);
                        break;
                    case "MYSQL":
                        options.UseMySQL(config.MasterConnetion);
                        break;
                    default:
                        throw new Exception("未知的数据库类型！");
                }

            });
        }

        /// <summary>
        /// 获取DBConfig的配置实体，并根据从库的数量重新对从库模型进行赋值
        /// </summary>
        public static void GetConfig()
        {
            JsonReaderHelper reader = new JsonReaderHelper();
            config = reader.GetConfig<DBConfig>("DBConfig");
            List<Slaveconnetion> slaveconnetions = new List<Slaveconnetion>();
            config.SlaveConnetions=config.SlaveConnetions.Take(config.SlaveCount).ToList();
            int MaxHitLimit = config.SlaveConnetions.Sum(t => t.HitRate);
            foreach (Slaveconnetion JsonConfig in config.SlaveConnetions)
            {
                int NextIndex = config.SlaveConnetions.IndexOf(JsonConfig) - 1;
                if (NextIndex < 0)
                {
                    JsonConfig.HitLimit = JsonConfig.HitRate;
                }
                else 
                {
                    JsonConfig.HitLimit = config.SlaveConnetions[NextIndex].HitLimit + JsonConfig.HitRate;
                }
                //if(config.SlaveConnetions.IndexOf(JsonConfig)== config.SlaveConnetions.Count - 1)
                //{
                //    MaxHitLimit += JsonConfig.HitRate;
                //}
                slaveconnetions.Add(JsonConfig);
            }
            config.MaxHitLimit = MaxHitLimit;
            config.SlaveConnetions = slaveconnetions;
        }
        /// <summary>
        /// 根据从库的hitrate进行随机选取数据库
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            Random ran = new Random();
            int RanNum = ran.Next(0, config.MaxHitLimit);
            Slaveconnetion slaveconnetion = config.SlaveConnetions.Where(t => t.HitLimit == RanNum).OrderBy(t => t.HitLimit).FirstOrDefault();
            return slaveconnetion.ConnectionString;
        }
    }
}
