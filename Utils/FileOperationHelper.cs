using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UIDP.UTILITY
{
    public class FileOperationHelper
    {
        /// <summary>
        /// 同步方法上传文件
        /// </summary>
        /// <param name="formCollection">控制器接收格式</param>
        /// <param name="SavePath">保存地址</param>
        /// <returns></returns>
        public static JArray UploadFile(IFormCollection formCollection,string SavePath)
        {
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            string BasePath = Directory.GetCurrentDirectory();
            JArray arr = new JArray();
            foreach(var file in fileCollection)
            {
                JObject obj = new JObject();
                //获取文件格式名称
                string FileFormat = Path.GetExtension(file.FileName);
                string name =file.Name+DateTime.Now.ToString("yyyyMMddHHmmss");
                while (File.Exists($"{BasePath}{SavePath}{name}{FileFormat}"))
                {
                    name = file.Name+DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                using (FileStream fs= File.Create($"{BasePath}{SavePath}{name}{FileFormat}"))
                {
                    file.CopyTo(fs);
                }
                obj.Add("ID", Guid.NewGuid());
                obj.Add("FILE_NAME", file.FileName);
                obj.Add("FILE_URL", $"{name}{FileFormat}");
                arr.Add(obj);
            }
            return arr;
        }
        /// <summary>
        /// 异步方法，上传文件
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="SavePath"></param>
        /// <returns></returns>
        public static async Task<JArray> UploadFileSync(IFormCollection formCollection, string SavePath)
        {
            JArray arr = new JArray();
            await Task.Run(() =>
            {
                arr = UploadFile(formCollection, SavePath);
            });
            return arr;
        }
        /// <summary>
        /// 同步方法，删除文件
        /// </summary>
        /// <param name="FilePathList"></param>
        public static void DeleteFile(List<string> FilePathList)
        {
            foreach(string path in FilePathList)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        /// <summary>
        /// 异步方法，删除文件(不返回执行结果)
        /// </summary>
        /// <param name="FilePathList"></param>
        /// <returns></returns>
        public static async void DeleteFileSync(List<string> FilePathList)
        {
            await Task.Run(() =>
            {
                DeleteFile(FilePathList);
            });
        }
    }
}
