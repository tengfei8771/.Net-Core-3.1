using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimedTask
{
    public static class QuartzServices
    {
        /// <summary>
        /// 启动定时任务方法，放到webapi的startup中(不要调这个方法，这个方法只能IIS托管运行时起作用，调下面的方法，因为AddQuartz的参数是params[])
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        public static void StartJob<TJob>() where TJob : IJob
        {
            var scheduler = new StdSchedulerFactory().GetScheduler().Result;

            var job = JobBuilder.Create<TJob>()
              .WithIdentity("job")
              .Build();

            var trigger1 = TriggerBuilder.Create()
              .WithIdentity("job.trigger")
              .StartNow()
              .WithSchedule(CronScheduleBuilder.CronSchedule("0/20 * * * * ?"))
              .ForJob(job)
              .Build();
            scheduler.AddJob(job, true);
            scheduler.ScheduleJob(job, trigger1);
            scheduler.Start();
        }

        #region 多任务
        /// <summary>
        /// 启动多任务调度方法，放到webapi的startup
        /// 再添加任务的时候添加新的触发器添加到dic中，
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        public static void StartJobs<TJob>() where TJob : IJob
        {
            var scheduler = new StdSchedulerFactory().GetScheduler().Result;

            var job = JobBuilder.Create<TJob>()
              .WithIdentity("jobs")
              .Build();

            var trigger1 = TriggerBuilder.Create()
               .WithIdentity("job.trigger")
               .StartNow()
               .WithSchedule(CronScheduleBuilder.CronSchedule("0/20 * * * * ?"))
               .ForJob(job)
               .Build();

            var trigger2 = TriggerBuilder.Create()
              .WithIdentity("job.trigger2")
              .StartNow()
              .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(11)).RepeatForever())
              .ForJob(job)
              .Build();

            var dictionary = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>
            {
                {job, new HashSet<ITrigger> {trigger1}}
            };
            scheduler.ScheduleJobs(dictionary, true);
            scheduler.Start();
        }
        #endregion

        /// <summary>
        /// 拓展方法，放到webapi的startup中注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jobs"></param>
        public static void AddQuartz(this IServiceCollection services, params Type[] jobs)
        {
            services.AddSingleton<IJobFactory, QuartzFactory>();
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));
            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });
        }

        
    }
}
