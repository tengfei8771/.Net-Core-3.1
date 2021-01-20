using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TimedTask
{
    //[DisallowConcurrentExecution]
    public class StandardsJob : IJob
    {  
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("定时任务开始执行!");
                string BasePath = Directory.GetCurrentDirectory();
                string TotalPath = $"{BasePath}/test/{Guid.NewGuid()}.txt";
                using(FileStream Fs = File.Create(TotalPath))
                {

                };
            });
        }
    }
}
