using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZnjTest.IBLL
{
    public interface IBaseService<T> where T:class 
    {
        Task<bool> InsertAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        IQueryable<T> GetList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
    }
}
