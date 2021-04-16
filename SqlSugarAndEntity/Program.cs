using SqlSugar;
using System;
using System.IO;

namespace SqlSugarAndEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始生成类库文件...");
                SqlSugarClient db = new SqlSugarClient(DataBaseConfig._config);
                var TotalPath = Directory.GetCurrentDirectory();
                string[] PathArr = TotalPath.Replace("\\", "/").Split(new string[] { "/bin/" }, StringSplitOptions.None);
                db.DbFirst.Where(t=>!t.Contains('.')).CreateClassFile($"{PathArr[0]}/Entity", "SqlSugarAndEntity");
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            Console.WriteLine("生成成功!");
            Console.ReadKey();
        }
    }
}
