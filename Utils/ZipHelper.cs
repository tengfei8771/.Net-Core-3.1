
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UIDP.UTILITY
{
    public class ZipHelper
    {
        /// <summary>
        /// 同步方法，将路径下的文件目录放入压缩包中
        /// </summary>
        /// <param name="FilePathList"></param>
        /// <returns></returns>
        public static string CreateZipFile(List<string> FilePathList)
        {
            string BasePath = Directory.GetCurrentDirectory();
            string name = DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";
            if (File.Exists(BasePath + "/UploadFiles/ZipFile/" + name))
            {
                File.Delete(BasePath + "/UploadFiles/ZipFile/" + name);
            }
            using (ZipFile zip = new ZipFile())
            {
               foreach(string FilePath in FilePathList)
                {
                    if (FileExists(FilePath))
                    {
                        zip.AddFile(FilePath, "file");
                    }
                    continue;
                }
                zip.Save(BasePath + "/UploadFiles/ZipFile/" + name); 
            }
            return name;
        }

        /// <summary>
        /// 异步方法，将路径下的文件目录放入压缩包中
        /// </summary>
        /// <param name="FilePathList"></param>
        /// <returns></returns>
        public static async Task<string> CreateZipFileSync(List<string> FilePathList)
        {
            string OutPath = string.Empty;
            await Task.Run(() =>
            {
                string BasePath = Directory.GetCurrentDirectory();
                string name = DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";
                if (File.Exists(BasePath + "/UploadFiles/ZipFile/" + name))
                {
                    File.Delete(BasePath + "/UploadFiles/ZipFile/" + name);
                }
                using (ZipFile zip = new ZipFile())
                {
                    foreach (string FilePath in FilePathList)
                    {
                        if (FileExists(FilePath))
                        {
                            zip.AddFile(FilePath, "file");
                        }
                        continue;
                    }
                    zip.Save(BasePath + "/UploadFiles/ZipFile/" + name);
                }
                OutPath = name;
            });

            return OutPath;
        }

        private static bool FileExists(string FilePath)
        {
            return File.Exists(FilePath) ? true : false;
        }

        
    }
}
