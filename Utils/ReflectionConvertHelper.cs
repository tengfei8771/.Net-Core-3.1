using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 反射帮助类，通过反射实现的一些通用方法全部集中在此。
    /// </summary>
    public static class ReflectionConvertHelper
    {
        /// <summary>
        /// 根据反射属性类型转换对应的值
        /// </summary>
        /// <param name="PropName">属性名称</param>
        /// <param name="value">属性的值</param>
        /// <returns></returns>
        public static dynamic ConvertByAttribute(string PropName,object value)
        {
            try
            {
                switch (PropName.ToLower())
                {
                    case "string":
                        return value.ToString();
                    case "decimal":
                        return Convert.ToDecimal(value);
                    case "boolean":
                        return Convert.ToBoolean(value);
                    case "datetime":
                        return Convert.ToDateTime(value);
                    case "int":
                        return Convert.ToInt32(value);
                    case "short":
                        return Convert.ToInt16(value);
                    case "long":
                        return Convert.ToInt64(value);
                    default:
                        return value;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public static T ConvertType<T>(object val)
        {
            if (val == null) return default(T);//返回类型的默认值
            Type tp = typeof(T);
            //泛型Nullable判断，取其中的类型
            if (tp.IsGenericType)
            {
                tp = tp.GetGenericArguments()[0];
            }
            //string直接返回转换
            if (tp.Name.ToLower() == "string")
            {
                return (T)val;
            }
            //反射获取TryParse方法
            var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                            new Type[] { typeof(string), tp.MakeByRefType() },
                                            new ParameterModifier[] { new ParameterModifier(2) });
            var parameters = new object[] { val, Activator.CreateInstance(tp) };
            bool success = (bool)TryParse.Invoke(null, parameters);
            //成功返回转换后的值，否则返回类型的默认值
            if (success)
            {
                return (T)parameters[1];
            }
            return default(T);
        }

        /// <summary>
        /// 异步方法,根据异步获取的datatable生成实体和where条件对实体进行赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">异步获取datata的task</param>
        /// <returns></returns>
        public static async Task<List<T>> ConvertDatatableToObject<T>(Task<DataTable> source) where T:class,new()
        {
            List<T> list = new List<T>();
            DataTable dt = await source;
            foreach (DataRow dr in dt.Rows)
            {
                T item = new T();
                Type t = item.GetType();
                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        prop.SetValue(item, Convert.ChangeType(dr[prop.Name.ToUpper()], prop.PropertyType));
                    }
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 根据传入的实体和datatable对实体进行赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> ConvertDatatableToObject<T>(DataTable source) where T : class, new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in source.Rows)
            {
                T item = new T();
                Type t = item.GetType();
                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        prop.SetValue(item, Convert.ChangeType(dr[prop.Name.ToUpper()], prop.PropertyType));
                    }
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 高性能版构造对象，比使用反射赋值速度快50%
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ConvertDatatableToObjectByExpression<T>(DataTable source) where T : class, new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in source.Rows)
            {
                T item = new T();
                Type t = item.GetType();

                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        ExpressionHelper.GetSetter<T>(prop)(item, dr[prop.Name]);
                    }
                }
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 根据传入的实体和datatable和where条件对实体进行赋值
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="WhereCondition">分类条件</param>
        /// <returns></returns>
        public static List<T> ConvertDatatableToObject<T>(DataTable source,string WhereCondition) where T : class, new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in source.Select(WhereCondition))
            {
                T item = new T();
                Type t = item.GetType();
                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        prop.SetValue(item, Convert.ChangeType(dr[prop.Name.ToUpper()], prop.PropertyType));
                    }
                }
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 异步方法,根据异步获取的datatable生成实体和where条件对实体进行赋值
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="source">异步获取datata的task</param>
        /// <param name="WhereCondition">分类条件</param>
        /// <returns></returns>
        public static async Task<List<T>> ConvertDatatableToObject<T>(Task<DataTable> source, string WhereCondition) where T : class, new()
        {
            List<T> list = new List<T>();
            DataTable dt = await source;
            foreach (DataRow dr in dt.Select(WhereCondition))
            {
                T item = new T();
                Type t = item.GetType();
                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        prop.SetValue(item, Convert.ChangeType(dr[prop.Name.ToUpper()], prop.PropertyType));
                    }
                }
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 将数据源转换为一个树形结构的实体
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="Source">数据源</param>
        /// <param name="MainName">父级被子级记录的属性名称</param>
        /// <param name="CombineRelationName">子级记录父级的属性名称</param>
        /// <returns></returns>
        public static List<T> ConvertDatatableToTreeList<T>(DataTable Source,string MainName,string CombineRelationName) where T:class,new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in Source.Select($"{CombineRelationName} IS NULL OR {CombineRelationName}=''"))
            {
                T item = new T();
                Type t = item.GetType();
                foreach (var prop in t.GetProperties())
                {
                    if (dr.Table.Columns.Contains(prop.Name.ToUpper()) && !IsDBNull(dr[prop.Name.ToUpper()]))
                    {
                        prop.SetValue(item, Convert.ChangeType(dr[prop.Name.ToUpper()], prop.PropertyType));
                    }
                    if (prop.PropertyType == typeof(List<T>))
                    {
                        CreateChildrenNode(item, Source, MainName, CombineRelationName, prop.Name);
                    }

                }
                list.Add(item);
            }
            return list;
        }
        private static void CreateChildrenNode<T>(T item,DataTable Source, string MainName,string CombineRelationName,string ChildrenPropName) where T : class, new()
        {
            List<T> ChildrenList = new List<T>();
            Type t = item.GetType();
            var prop = t.GetProperty(MainName);
            foreach (DataRow dr in Source.Select($"{CombineRelationName}='{prop.GetValue(item)}'"))
            {
                T ChildrenItem = new T();
                Type ChildrenType = ChildrenItem.GetType();
                foreach (var ChildrenProp in ChildrenType.GetProperties())
                {
                    if (dr.Table.Columns.Contains(ChildrenProp.Name.ToUpper()) && !IsDBNull(dr[ChildrenProp.Name.ToUpper()]))
                    {
                        ChildrenProp.SetValue(ChildrenItem, Convert.ChangeType(dr[ChildrenProp.Name.ToUpper()], ChildrenProp.PropertyType));
                    }
                    if (ChildrenProp.PropertyType == typeof(List<T>))
                    {
                        CreateChildrenNode(ChildrenItem, Source, MainName, CombineRelationName, ChildrenPropName);
                    }  
                }
                ChildrenList.Add(ChildrenItem);
            }
            prop = t.GetProperty(ChildrenPropName);
            prop.SetValue(item, ChildrenList);
        }
        /// <summary>
        /// 将数据源转为一个弱类型的jarray,此方法的好处就是不用设计实体类
        /// </summary>
        /// <param name="Source">数据源</param>
        /// <param name="MainName">父级被子级记录的属性名称</param>
        /// <param name="CombineRelationName">子级记录父级的属性名称</param>
        /// <param name="ChildrenName">生成子级树的名称</param>
        /// <returns></returns>
        public static JArray ConvertDatatableToJArrayTreeList(DataTable Source, string MainName, string CombineRelationName,string ChildrenName="children")
        {
            JArray arr = new JArray();
            foreach (DataRow dr in Source.Select($"{CombineRelationName} IS NULL OR {CombineRelationName}=''"))
            {
                JObject obj = new JObject();
                for (int i = 0; i < Source.Columns.Count; i++)
                {
                    string ColumnName = Source.Columns[i].ColumnName;
                    obj.Add(ColumnName, JToken.FromObject(dr[ColumnName]));
                }
                CreateJArrayChildrenNode(obj, Source, MainName, CombineRelationName, ChildrenName);
                arr.Add(obj);
            }
            return arr;
        }

        private static void CreateJArrayChildrenNode(JObject obj, DataTable Source, string MainName, string CombineRelationName,string ChildrenName)
        {
            JArray arr = new JArray();
            foreach (DataRow dr in Source.Select($"{CombineRelationName}='{obj.Value<string>(MainName)}'"))
            {
                JObject ChildrenObj = new JObject();
                for (int i = 0; i < Source.Columns.Count; i++)
                {
                    string ColumnName = Source.Columns[i].ColumnName;
                    ChildrenObj.Add(ColumnName, JToken.FromObject(dr[ColumnName]));
                }
                CreateJArrayChildrenNode(ChildrenObj, Source, MainName, CombineRelationName, ChildrenName);
                arr.Add(ChildrenObj);
            }
            obj.Add(ChildrenName, arr);
        }

        private static bool IsDBNull(object t)
        {
            return t is DBNull;
        }
        public static bool IsNumericType(this Type o)
        {
            return !o.IsClass && !o.IsInterface && o.GetInterfaces().Any(q => q == typeof(IFormattable));
        }
    }
}
