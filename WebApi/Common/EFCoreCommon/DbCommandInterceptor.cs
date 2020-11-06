using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.EFCoreCommon
{
    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        private const string masterConnectionString = "server=localhost;user id=sa;pwd=sa;database=AppDB";
        private const string slaveConnectionString = "server=localhost;user id=sa;pwd=sa;database=AppDB1";
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;
                var executeMethod = ((CommandEventData)value.Value).ExecuteMethod;
                Console.WriteLine(command.CommandText);
                if (executeMethod == DbCommandMethod.ExecuteNonQuery)
                {
                    ResetConnection(command, masterConnectionString);
                }
                else if (executeMethod == DbCommandMethod.ExecuteScalar)
                {
                    ResetConnection(command, slaveConnectionString);
                }
                else if (executeMethod == DbCommandMethod.ExecuteReader)
                {
                    ResetConnection(command, slaveConnectionString);
                }
                
            }
        }

        void ResetConnection(DbCommand command, string connectionString)
        {
            if (command.Connection.State == ConnectionState.Open)
            {
                if (!command.CommandText.Contains("@@ROWCOUNT"))
                {
                    command.Connection.Close();
                    command.Connection.ConnectionString = connectionString;
                }
            }
            if (command.Connection.State == ConnectionState.Closed)
            {
                try
                {
                    command.Connection.Open();
                }
                catch(Exception e)
                { 
                    throw new Exception($"数据库打开失败,请检查数据库连接!{command.Connection.ConnectionString}");
                }
            }
        }

    }
}
