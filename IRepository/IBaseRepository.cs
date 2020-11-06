using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IRepository
{
    public interface IBaseRepository<T> where T:class
    {
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        List<T> GetInfo(Expression<Func<T, bool>> predicate);
        dynamic FromSql(string sql);
        void ExecuteSqlCommand(string sql);
        void InsertAll(List<T> list);

        void DeleteAll(List<T> list);
        void UpdateAll(List<T> list);
    }
}
