using EFCore.BulkExtensions;
using Entity.Models;
using IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDBContext _appDBContext;

        public BaseRepository(AppDBContext _appDBContext)
        {
            this._appDBContext = _appDBContext;
        }
        public bool Delete(T entity)
        {
            _appDBContext.Remove(entity);
            return _appDBContext.SaveChanges()>0;
        }

        public void DeleteAll(List<T> list)
        {
            _appDBContext.BulkDelete(list);
        }

        public void ExecuteSqlCommand(string sql)
        {
            _appDBContext.Database.ExecuteSqlRaw(sql);
        }

        public dynamic FromSql(string sql)
        {
            return _appDBContext.Database.ExecuteSqlRaw(sql);
        }

        public List<T> GetInfo(Expression<Func<T, bool>> predicate)
        {
            return _appDBContext.Set<T>().Where(predicate).ToList();
        }

        public bool Insert(T entity)
        {
            _appDBContext.Add(entity);
            return _appDBContext.SaveChanges() > 0;
        }

        public void InsertAll(List<T> list)
        {
            _appDBContext.BulkInsert(list);
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAll(List<T> list)
        {
            throw new NotImplementedException();
        }
    }
}
