using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PublicWebApi.Common
{
    public class JsonReaderHelper
    {
        /// <summary>
        /// 将整个json文件读取成为一个Jobject返回
        /// </summary>
        /// <param name="JsonFileName">json文件名称</param>
        /// <returns></returns>
        public JObject GetConfig(string JsonFileName)
        {
            using (StreamReader file =File.OpenText(Directory.GetCurrentDirectory() + "\\"+ JsonFileName + ".json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    return o;
                }
                
            }
        }
        /// <summary>
        /// 根据传入的类型返回JSON文件指定类型的键的值
        /// </summary>
        /// <typeparam name="T">预期返回的类型</typeparam>
        /// <param name="JsonFileName">json文件名称</param>
        /// <param name="key">所获取的值的键</param>
        /// <returns></returns>
        public T GetValue<T>(string JsonFileName,string key)
        {
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "\\"+ JsonFileName+".json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    var item = o.GetValue(key);
                    return item.ToObject<T>();
                }

            }
        }
    }
}
