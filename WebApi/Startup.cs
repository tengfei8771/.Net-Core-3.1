using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        //public IConfiguration JsonContent => GetJsonContent();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // ��ʹ���շ�
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // ����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #region ��ϵ�����ݿ�����(������)
            //if (this.Configuration.GetSection("RDBMSConfig").GetValue<bool>("UseingStatus"))
            //{
            //    string DBtype = this.Configuration.GetSection("RDBMSConfig").GetSection("DataBaseType").Value;
            //    switch (DBtype.ToUpper())
            //    {
            //        case "SQLSEVER":
            //            services.AddDbContext<Models.AppContext>(options => options.UseSqlServer(this.Configuration.GetSection("RDBMSConfig").GetSection("DBtype").Value));
            //            break;
            //        case "ORACLE":
            //            services.AddDbContext<Models.AppContext>(options => options.UseOracle(this.Configuration.GetSection("RDBMSConfig").GetSection("DBtype").Value));
            //            break;
            //        case "MYSQL":
            //            services.AddDbContext<Models.AppContext>(options => options.UseMySQL(this.Configuration.GetSection("RDBMSConfig").GetSection("DBtype").Value));
            //            break;
            //        default:
            //            throw new Exception("�����ļ��е����ݿ���������!���޸ĺ���������");
            //    }
            //}
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Services"))//ע���������еķ���������Ӧ�Ľӿ�
                .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
        }

        //private IConfiguration GetJsonContent()
        //{
        //    var builder = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json");
        //    IConfiguration Configuration = builder.Build();
        //    return Configuration;
        //}
    }
}
