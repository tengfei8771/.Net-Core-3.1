using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Utils;

namespace Services
{
    public class test
    {
        private DataTable Source { get; set; }
        public test()
        {
            Source = CreateTestData();
        }
        public DataTable CreateTestData()
        {
            DataTable dt = new DataTable();
            for(int i = 5; i <= 10; i++)
            {
                dt.Columns.Add($"test{i}");
            }
            dt.Columns.Add("test1",typeof(int));
            dt.Columns.Add("test2", typeof(decimal));
            dt.Columns.Add("test3", typeof(DateTime));
            dt.Columns.Add("test4", typeof(long));
            for(int i = 0; i <= 100000; i++)
            {
                DataRow dr = dt.NewRow();
                dr["test1"] = 1;
                dr["test2"] = 2.0;
                dr["test3"] = DateTime.Now;
                dr["test4"] = 3;
                for (int j = 5; j <= 10; j++)
                {
                    dr[$"test{j}"] = $"model{i},{j}";
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public List<TestRelectionAndExpressionModel> CreateModelByRelection()
        {
            return ReflectionConvertHelper.ConvertDatatableToObject<TestRelectionAndExpressionModel>(Source);
        }

        public List<TestRelectionAndExpressionModel> CreateModelByExpression()
        {
            return ExpressionHelper.ConvertDataTableToObject<TestRelectionAndExpressionModel>(Source);
        }

        public object GetExpressionValue<T>(T item, PropertyInfo prop)
        {
            Type t = typeof(T);
            var value = ExpressionHelper.GetGetter<T>(prop)(item);
            return value;

        }
    }
}
