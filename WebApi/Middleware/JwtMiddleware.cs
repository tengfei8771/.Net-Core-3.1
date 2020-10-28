using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UIDP.UTILITY.JWTHelper;
using Utils;
using WebApi.Common;

namespace PublicWebApi.Common.Validator
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        { 
            if (GetNeedOrNeedNotFlag.GetFlag(httpContext))
            {
                HttpRequest request = httpContext.Request;
                if (!request.Headers.TryGetValue("X-Token", out var apiKeyHeaderValues))
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
                else
                {
                    JwtHelper helper = new JwtHelper();
                    request.EnableBuffering();//可以多次多次读取http内包含的数据
                    if (!helper.ValidateJwt(apiKeyHeaderValues.ToString(), out string Msg))
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var a = new
                        {
                            success = false,
                            msg = Msg,
                            cause = Msg
                        };
                        httpContext.Response.WriteAsync(JsonConvert.SerializeObject(a));
                        return Task.FromResult(0);
                    }
                }
            }
            return _next(httpContext);
        }

    }
    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
