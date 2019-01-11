using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZnjTest.IBLL;
using ZnjTest.IDAL;

namespace ZnjTest.BLL
{
   public class BaseService<T>:IBaseService<T>where T:class
   {
       private readonly IBaseDal<T> baseDal;

       public BaseService(IBaseDal<T> baseDal)
       {
           this.baseDal = baseDal;
       }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await baseDal.DeleteAsync(entity);
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return baseDal.GetList(whereLambda);
        }

        public async Task<bool> InsertAsync(T entity)
        {
            return await baseDal.InsertAsync(entity);
        }
    }
}
