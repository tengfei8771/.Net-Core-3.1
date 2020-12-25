using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 反射帮助类，通过反射实现的一些通用方法全部集中在此。
    /// </summary>
    public class ReflectionConvertHelper
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
                throw new Exception(e.Message);
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

        private static bool IsDBNull(object t)
        {
            return t is DBNull;
        }
    }
}
