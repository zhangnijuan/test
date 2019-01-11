using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZnjTest.Model.DTO;

namespace ZnjTest.IDAL
{
    public interface IBaseDal<T> where T : class
    {
        Task<bool> InsertAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        Task<bool> UpdataAsync(T entity);
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda, bool isNoTracking = true);

        Task<T> GetEntityAsync<TS>(TS id);
        Task<T> GetEntityAsync(Expression<Func<T, bool>> whereLambda, bool isNoTracking = true);
        Task<int> CountAsync(Expression<Func<T, bool>> whereLambda);
        int Count(Expression<Func<T, bool>> whereLambda);
        PageData<T> GetPageList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isOrder = true, bool isNoTracking = true);
    }
}
