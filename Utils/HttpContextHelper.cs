using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class HttpContextHelper
    {
        /// <summary>
        /// 获取路由终结点方法
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <returns></returns>
        public static Endpoint GetEndPoint(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Features.Get<IEndpointFeature>()?.Endpoint;
        }

        /// <summary>
        /// 获取自定义特性类方法
        /// </summary>
        /// <typeparam name="T">自定义控制器特性类</typeparam>
        /// <param name="context">数据库上下文</param>
        /// <returns></returns>
        public static T GetMetadata<T>(HttpContext context) where T:class
        {
            Endpoint EndPoint = GetEndPoint(context);
            if (EndPoint == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return EndPoint.Metadata.GetMetadata<T>();
        }
    }
}
