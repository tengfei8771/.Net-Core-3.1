using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Untils
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// 根据预期模型使用get方法请求网址，返回一个模型
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="BaseUrl">请求网址</param>
        /// <param name="QueryData">请求的数据</param>
        /// <param name="HeaderInfo">请求头数据</param>
        /// <returns></returns>
        public static T ClientGet<T>(string BaseUrl,Dictionary<string,string> QueryData,Dictionary<string,string> HeaderInfo=null) where T : new()
        {
            RestClient Client = new RestClient();
            Client.BaseUrl =new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.GET);
            if (HeaderInfo != null)
            {
                foreach(string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            foreach(string key in QueryData.Keys)
            {
                request.AddQueryParameter(key, QueryData[key]);
            }
            var result = Client.Execute<T>(request);
            return result.Data;
        }
        /// <summary>
        /// 根据地址使用get方法返回一个弱类型的json数据
        /// </summary>
        /// <param name="BaseUrl">请求网址</param>
        /// <param name="QueryData">请求数据</param>
        /// <param name="HeaderInfo">头数据</param>
        /// <returns></returns>
        public static JObject ClientGet(string BaseUrl, Dictionary<string, string> QueryData, Dictionary<string, string> HeaderInfo = null)
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.GET);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            foreach (string key in QueryData.Keys)
            {
                request.AddQueryParameter(key, QueryData[key]);
            }
            var result = Client.Execute(request).Content;
            return JObject.Parse(result);
        }

        /// <summary>
        /// 根据预期模型使用Post方法请求网址，返回一个模型
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="BaseUrl">请求地址</param>
        /// <param name="PostData">发送数据</param>
        /// <param name="HeaderInfo">请求头数据</param>
        /// <returns></returns>
        public static T ClientPost<T>(string BaseUrl, Dictionary<string, object> PostData, Dictionary<string, string> HeaderInfo = null) where T : new()
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.POST);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            request.AddJsonBody(PostData);
            var result = Client.Execute<T>(request);
            return result.Data;
        }
        /// <summary>
        /// 根据地址使用post方法返回一个弱类型的json数据
        /// </summary>
        /// <param name="BaseUrl"></param>
        /// <param name="PostData"></param>
        /// <param name="HeaderInfo"></param>
        /// <returns></returns>
        public static JObject ClientPost(string BaseUrl, Dictionary<string, object> PostData, Dictionary<string, string> HeaderInfo = null)
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.POST);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            request.AddJsonBody(PostData);
            var result = Client.Execute(request).Content;
            return JObject.Parse(result);
        }

        /// <summary>
        /// 根据预期模型使用get方法请求网址，返回一个模型 异步方法
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="BaseUrl">请求网址</param>
        /// <param name="QueryData">请求的数据</param>
        /// <param name="HeaderInfo">请求头数据</param>
        /// <returns></returns>
        public static async Task<T> ClientGetSync<T>(string BaseUrl, Dictionary<string, string> QueryData, Dictionary<string, string> HeaderInfo = null) where T : new()
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.GET);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            foreach (string key in QueryData.Keys)
            {
                request.AddQueryParameter(key, QueryData[key]);
            }
            var result = await Client.ExecuteAsync<T>(request);
            return result.Data;
        }
        /// <summary>
        /// 根据地址使用get方法返回一个弱类型的json数据(异步方法)
        /// </summary>
        /// <param name="BaseUrl">请求网址</param>
        /// <param name="QueryData">请求数据</param>
        /// <param name="HeaderInfo">头数据</param>
        /// <returns></returns>

        public static async Task<JObject> ClientGetSync(string BaseUrl, Dictionary<string, string> QueryData, Dictionary<string, string> HeaderInfo = null)
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.GET);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            foreach (string key in QueryData.Keys)
            {
                request.AddQueryParameter(key, QueryData[key]);
            }
            var result =await Client.ExecuteAsync(request);
            return JObject.Parse(result.Content);
        }


        /// <summary>
        /// 根据预期模型使用Post方法请求网址，返回一个模型(异步方法)
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="BaseUrl">请求地址</param>
        /// <param name="PostData">发送数据</param>
        /// <param name="HeaderInfo">请求头数据</param>
        /// <returns></returns>
        public static async Task<T> ClientPostSync<T>(string BaseUrl, Dictionary<string, object> PostData, Dictionary<string, string> HeaderInfo = null) where T : new()
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.POST);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            request.AddJsonBody(PostData);
            var result =await Client.ExecuteAsync<T>(request);
            return result.Data;
        }
        /// <summary>
        /// 根据地址使用post方法返回一个弱类型的json数据(异步)
        /// </summary>
        /// <param name="BaseUrl"></param>
        /// <param name="PostData"></param>
        /// <param name="HeaderInfo"></param>
        /// <returns></returns>
        public static async Task<JObject> ClientPostSync(string BaseUrl, Dictionary<string, object> PostData, Dictionary<string, string> HeaderInfo = null)
        {
            RestClient Client = new RestClient();
            Client.BaseUrl = new Uri(BaseUrl);
            Client.Timeout = 50000;
            RestRequest request = new RestRequest(Method.POST);
            if (HeaderInfo != null)
            {
                foreach (string key in HeaderInfo.Keys)
                {
                    request.AddHeader(key, HeaderInfo[key]);
                }
            }
            request.AddJsonBody(PostData);
            var result =await Client.ExecuteAsync(request);
            return JObject.Parse(result.Content);
        }

    }
}
