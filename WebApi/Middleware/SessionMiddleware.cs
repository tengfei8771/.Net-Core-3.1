using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicWebApi.Common.Validator
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!WhiteListHelper.IsPass(httpContext))
            {
                HttpRequest request = httpContext.Request;
                //UserInfoHelper userhelp = new UserInfoHelper();
                if (request.Headers.TryGetValue("X-Token", out var apiKeyHeaderValues))
                {
                    string[] StrArr = apiKeyHeaderValues.ToString().Split('.');
                    string Second = Base64UrlEncoder.Decode(StrArr[1]);
                    JObject JSecond = JObject.Parse(Second);
                    var str = JSecond.Value<string>("sub");
                    string userdomain = JObject.Parse(str).Value<string>("usercode");
                    //if (userhelp.GetUserInfo(userdomain, out string json))
                    //{
                    //    if (!httpContext.Session.TryGetValue(userdomain, out byte[] data))
                    //    {
                    //        httpContext.Session.SetString(userdomain, json);
                    //    }
                    //    //  var content = httpContext.Session.GetString(apiKeyHeaderValues.ToString());
                    //}
                    //else
                    //{
                    //    httpContext.Response.ContentType = "application/json";
                    //    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //    var a = new
                    //    {
                    //        success = false,
                    //        msg = "此用户不存在!",
                    //        cause = "此用户不存在"
                    //    };
                    //    httpContext.Response.WriteAsync(JsonConvert.SerializeObject(a));
                    //    return Task.FromResult(0);
                    //}

                }
                else
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    var a = new
                    {
                        success = false,
                        msg = "此请求未包含JWT令牌,禁止访问!!",
                        cause = "此请求未包含JWT令牌,禁止访问!"
                    };
                    httpContext.Response.WriteAsync(JsonConvert.SerializeObject(a));
                    return Task.FromResult(0);
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddleware>();
        }
    }
}
