﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 反射帮助类，通过反射实现的一些通用方法全部集中在此。
    /// </summary>
    public static class ReflectionConvertHelper
    {
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
        /// 根据标注的Mapper标注名和属性名，将listA转化为listB
        /// </summary>
        /// <typeparam name="A">数据源类型A</typeparam>
        /// <typeparam name="B">被转化的类型B</typeparam>
        /// <param name="Source">数据源listA</param>
        /// <returns>结果集listB</returns>
        public static List<B> ConvertAToB <A,B>(List<A> Source) where A:class,new() where B : class, new()
        {
            List<B> list = new List<B>();
            Type typeB = typeof(B);
            Type typeA = typeof(A);
            foreach (A Sourceitem in Source)
            {
                B item = new B();
                foreach(var prop in typeB.GetProperties())
                {
                    string MapperName = GetMapperName(prop);
                    if (string.IsNullOrEmpty(MapperName))
                    {
                        MapperName = prop.Name;
                    }
                    var propA = typeA.GetProperty(MapperName);
                    if (propA != null)
                    {
                        prop.SetValue(item,propA.GetValue(Sourceitem));
                    }                   
                }
                list.Add(item);
            }
            return list;
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
            list = ConvertDatatableToObject<T>(dt);
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
            Type t = typeof(T);
            foreach (DataRow dr in source.Rows)
            {
                T item = new T();
                foreach (var prop in t.GetProperties())
                {
                    string MapperName = GetMapperName(prop);
                    if (string.IsNullOrEmpty(MapperName))
                    {
                        MapperName = prop.Name;
                    }
                    if (dr.Table.Columns.Contains(MapperName) && !IsDBNull(dr[MapperName]))
                    {
                        //反射赋值
                        prop.SetValue(item, Convert.ChangeType(dr[MapperName], prop.PropertyType));
                        //表达树赋值
                        //ExpressionHelper.GetSetter<T>(prop)(item, dr[MapperName]);
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
            DataTable dt = GetNewDataTable(source,WhereCondition);
            return ConvertDatatableToObject<T>(dt);
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
            DataTable dt = await source;
            return ConvertDatatableToObject<T>(GetNewDataTable(dt, WhereCondition));
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
            Type t = typeof(T);
            foreach (DataRow dr in Source.Select($"{CombineRelationName} IS NULL OR {CombineRelationName}=''"))
            {
                T item = new T();
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


        /// <summary>
        /// 把根据一个实体List映射到另一个实体List中，并根据条件将其转为树形结构
        /// </summary>
        /// <typeparam name="T">数据源</typeparam>
        /// <typeparam name="T1">映射到的实体</typeparam>
        /// <param name="Source">TList</param>
        /// <param name="ParentField">父级标识字段</param>
        /// <param name="ChildrenField">子级标识字段</param>
        /// <returns></returns>
        public static List<T1> ConvertObjectToTreeList<T,T1>(List<T> Source, Expression<Func<T, object>> ParentField, Expression<Func<T, object>> ChildrenField) 
            where T : class, new() 
            where T1:class,new()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            BinaryExpression ParentExpLeft = ExpressionHelper.CreateWhereConditionBySelector(ChildrenField, "", parameter);
            BinaryExpression ParentExpRight = ExpressionHelper.CreateWhereConditionBySelector(ChildrenField, null, parameter);
            var WhereCondition = Expression.Lambda<Func<T, bool>>(Expression.Or(ParentExpLeft, ParentExpRight), parameter);
            List<T> ParentList = Source.Where(WhereCondition.Compile()).ToList();
            List<T1> list = new List<T1>();
            Type ItemType = typeof(T1);
            Type SourceItemType = typeof(T);
            foreach (T SourceItem in ParentList)
            {
                T1 Item = new T1();
                
                foreach (var prop in ItemType.GetProperties())
                {
                    var SourceProp = SourceItemType.GetProperty(prop.Name);
                    if (SourceProp != null)
                    {
                        //给T1赋值
                        prop.SetValue(Item, SourceProp.GetValue(SourceItem));
                    }
                    if (prop.PropertyType == typeof(List<T1>))
                    {
                        CreateObjectChildrenNode(Source, Item, ParentField, ChildrenField, prop.Name);
                    }
                }
                list.Add(Item);
            }
            return list;
        }

        private static void CreateObjectChildrenNode<T, T1>(List<T> Source, T1 ParentItem, Expression<Func<T, object>> ParentField, Expression<Func<T, object>> ChildrenField, string ChildrenName)
            where T : class, new()
            where T1 : class, new()
        {
            string ParentFieldName = ExpressionHelper.GetPropertyName(ParentField);
            Type ParentProp = typeof(T1);
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            BinaryExpression ChildrenOrigin = ExpressionHelper.CreateWhereConditionBySelector(ChildrenField, ParentProp.GetProperty(ParentFieldName).GetValue(ParentItem),parameter);
            var ChildrenExp = Expression.Lambda<Func<T, bool>>(ChildrenOrigin, parameter);
            List<T> SoureChildrenList = Source.Where(ChildrenExp.Compile()).ToList();
            List<T1> ChildrenList = new List<T1>();
            Type ItemType = typeof(T1);
            Type SourceItemType = typeof(T);
            foreach (T SourceChildrenItem in SoureChildrenList)
            {
                T1 Item = new T1();
                foreach (var prop in ItemType.GetProperties())
                {
                    var SourceProp = SourceItemType.GetProperty(prop.Name);
                    if (SourceProp != null)
                    {
                        //给T1赋值
                        prop.SetValue(Item, SourceProp.GetValue(SourceChildrenItem));
                    }
                    if (prop.PropertyType == typeof(List<T1>))
                    {
                        CreateObjectChildrenNode(Source, Item, ParentField, ChildrenField, prop.Name);
                    }             
                }
                ChildrenList.Add(Item);
            }
            var ChildrenProp = ParentProp.GetProperty(ChildrenName);
            ChildrenProp.SetValue(ParentItem, ChildrenList);
        }
        /// <summary>
        /// 获取标注的属性名
        /// </summary>
        /// <param name="prop">属性</param>
        /// <returns>属性名</returns>
        private static string GetMapperName(PropertyInfo prop)
        {
            var Attribute = prop.GetCustomAttribute<MapperAttribute>();
            if (Attribute == null)
            {
                return "";
            }
            if (Attribute.IgnoreColumn)
            {
                return "";
            }
            else
            {
                return Attribute.MapperName;
            }
        }
        /// <summary>
        /// 将dt select的结果从dr[] 变成dt
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        private static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            for (int i = 0; i < dr.Length; i++)
            {
                newdt.ImportRow((DataRow)dr[i]);
            }
            return newdt;//返回的查询结果
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
