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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder
                    //.AllowAnyOrigin()//2.0写法 升级之后必须加入参数new string[]指定信任的域+端口
                    .SetIsOriginAllowed(_ => true)//3.0新写法
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });//注册跨域
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //启用内存缓存(该步骤需在AddSession()调用前使用)
            services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            //services.AddSession();
            services.AddSession(options =>
            {
                options.Cookie.Name = "PublicWebApi.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);//设置session的过期时间
                                                                 //options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值

            });
            services.AddSwaggerGen(options =>
            {
                // 添加文档信息
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicWebApi", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                options.IncludeXmlComments(xmlPath, true);
                options.AddSecurityDefinition("X-Token", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token(不需要输入前缀)",
                    Name = "X-Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "X-Token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "X-Token"}
                           },new string[] { }
                        }
                    });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 不使用驼峰
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #region 关系型数据库连接(不用了)
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
            //            throw new Exception("配置文件中的数据库类型有误!请修改后启动程序！");
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
            app.UseCors("any");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "PublicWebApi v1");

            });
            app.UseSession();
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
            builder.RegisterAssemblyTypes(Assembly.Load("Repository"))
                .Where(x => x.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.Load("Services"))//注册服务层所有的服务类和其对应的接口
                .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces();
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
