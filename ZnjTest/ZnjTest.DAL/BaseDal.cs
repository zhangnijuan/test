using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using ZnjTest.IDAL;
using ZnjTest.Model;
using ZnjTest.Model.DTO;

namespace ZnjTest.DAL
{
  public  class BaseDal<T> : IBaseDal<T> where T : class
    {
        protected readonly ZnjTestContext db;
        private readonly DbSet<T> dbSet;
        public BaseDal(ZnjTestContext db)
        {
            this.db = db;
            this.dbSet = this.db.Set<T>();
        }

        public int Count(Expression<Func<T, bool>> whereLambda)
        {
            return this.dbSet.Count(whereLambda);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await this.dbSet.CountAsync(whereLambda);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> deleteLamdba)
        {

            return await  this.GetList(deleteLamdba, false).DeleteAsync()>0;
        }

        public async Task<T> GetEntityAsync<TS>(TS id)
        {
            return await  this.dbSet.FindAsync(id);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda, bool isNoTracking = true)
        {
            var data= this.GetList(whereLambda, isNoTracking);
            return await data.FirstOrDefaultAsync();
        }

      
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda, bool isNoTracking = true)
        {
            return isNoTracking ? this.dbSet.Where(whereLambda) : this.dbSet.Where(whereLambda).AsNoTracking();
        }

        public PageData<T> GetPageList<Tkey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, Tkey>> orderBy, int pageIndex, int pageSize, bool isOrder = true, bool isNoTracking = true)
        {
            var data = this.GetList(whereLambda,isNoTracking);
            data = isOrder ? data.OrderBy(orderBy) : data.OrderByDescending(orderBy);
            return new PageData<T>()
            {
                Data = data.Skip((pageIndex-1)*pageSize).Take(pageSize),
                TotalCount = this.Count(whereLambda)
            };
        }

        public async Task<T> InsertAsync(T entity)
        {
             this.dbSet.Add(entity);
             await db.SaveChangesAsync() ;
            return entity;
        }

        public async Task<bool> UpdataAsync(T entity)
        {
            db.Set<T>().Attach(entity);
            foreach (System.Reflection.PropertyInfo p in entity.GetType().GetProperties())
            {

                if (p.GetValue(entity) != null && p.PropertyType.Name != "ICollection`1")
                {
                    db.Entry<T>(entity).Property(p.Name).IsModified = true;
                }
            }
            return await this.db.SaveChangesAsync()>0;
           
        }

        public async Task<bool> UpdataAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> updateLambda)
        {
            return await this.GetList(whereLambda,false).UpdateAsync(updateLambda) > 0;
        }
    }
}
