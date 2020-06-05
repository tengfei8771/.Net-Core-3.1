using EFCore.BulkExtensions;
using IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext dbContext;

        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool Delete(T entity)
        {
            dbContext.Remove(entity);
            return dbContext.SaveChanges()>0;
        }

        public void DeleteAll(List<T> list)
        {
            dbContext.BulkDelete(list);
        }

        public void ExecuteSqlCommand(string sql)
        {
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public dynamic FromSql(string sql)
        {
            return dbContext.Database.ExecuteSqlRaw(sql);
        }

        public T GetInfo(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T entity)
        {
            dbContext.Add(entity);
            return dbContext.SaveChanges() > 0;
        }

        public void InsertAll(List<T> list)
        {
            dbContext.BulkInsert(list);
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
