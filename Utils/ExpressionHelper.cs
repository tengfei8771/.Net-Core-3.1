using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Utils
{
    public static class ExpressionHelper
    {
        //private static IMemoryCache Cache { get; set; }
        private static Dictionary<string,object> Cache { get; set; }
        static ExpressionHelper()
        {
            //Cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            Cache = new Dictionary<string, object>();
        }
        /// <summary>
        /// 使用表达式树对对象进行赋值，比反射性能强
        /// </summary>
        /// <typeparam name="T">要赋值的实体类</typeparam>
        /// <typeparam name="object">要赋值的实体类</typeparam>
        /// <param name="property">要赋值的属性</param>
        /// <returns></returns>
        public static Action<T, object> GetSetter<T>(PropertyInfo property)
        {
            Action<T, object> result = null;
            Type type = typeof(T);
            string key = type.AssemblyQualifiedName + "_set_" + property.Name;
            if (Cache.TryGetValue(key,out object CacheValue))
            {
                result = CacheValue as Action<T, object>;
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(type, "t");
                ParameterExpression value = Expression.Parameter(typeof(object), "propertyValue");
                MethodInfo setter = type.GetMethod("set_" + property.Name);
                MethodCallExpression call = Expression.Call(parameter, setter, Expression.Convert(value, property.PropertyType));
                Expression<Action<T,object>> lambda = Expression.Lambda<Action<T, object>>(call, parameter, value);
                //此方法非常吃性能，必须缓存起来提高速度
                result = lambda.Compile();
                Cache.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// 使用表达式树赋值，速度不如反射快，建议不用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Func<T,object> GetGetter<T>(PropertyInfo property)
        {
            Func<T, object> result = null;
            Type type = typeof(T);
            string key = type.AssemblyQualifiedName + "_get_" + property.Name;
            if (Cache.TryGetValue(key, out object CacheValue))
            {
                result = CacheValue as Func<T, object>;
            }
            else
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                MemberExpression member = Expression.PropertyOrField(parameter, property.Name);
                UnaryExpression convertExpression = Expression.Convert(member, typeof(object));
                Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(convertExpression, parameter);
                result = lambda.Compile();
                Cache.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// 高性能版构造对象，速度性能显著提升
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToObject<T>(DataTable source) where T : class, new()
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
                        GetSetter<T>(prop)(item, dr[MapperName]);
                    }
                }
                list.Add(item);
            }
            return list;
        }


        /// <summary>
        /// 根据selector条件和value生成p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <typeparam name="TResult">委托返回类型</typeparam>
        /// <param name="Selector">p=>p.propertyName</param>
        /// <param name="value">等式右值</param>
        /// <returns></returns>
        public static BinaryExpression CreateWhereConditionBySelector<T>(Expression<Func<T,object>> Selector,object value,ParameterExpression parameter)
        {
            string key = typeof(T).AssemblyQualifiedName;
            string FieldName = GetPropertyName(Selector); 
            MemberExpression member = Expression.PropertyOrField(parameter, FieldName);
            ConstantExpression constant = Expression.Constant(value);//创建常数
            return Expression.Equal(member, constant);
        }

        /// <summary>
        /// 根据表达式获取属性的名称
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
        {
            var Name = "";
            var body = exp.Body;
            if (body is UnaryExpression)
            {
                Name = ((MemberExpression)((UnaryExpression)body).Operand).Member.Name;
            }
            else if (body is MemberExpression)
            {
                Name = ((MemberExpression)body).Member.Name;
            }
            else if (body is ParameterExpression)
            {
                Name = ((ParameterExpression)body).Type.Name;
            }
            return Name;
        }

        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName < propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName <= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, string propertyValue, Type type)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        //// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.StartsWith(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetStartsWith<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }
        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.EndsWith(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">等式右值</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetEndsWith<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }
        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.CompareTo(propertyValue)
        /// sqlsugar不支持CompareTo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> StringGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            BinaryExpression siteNoExpression = Expression.GreaterThanOrEqual(Expression.Call(member, method, constant), Expression.Constant(0, typeof(int)));
            return Expression.Lambda<Func<T, bool>>(siteNoExpression, parameter);
        }


        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
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

        private static bool IsDBNull(object t)
        {
            return t is DBNull;
        }
    }
}
