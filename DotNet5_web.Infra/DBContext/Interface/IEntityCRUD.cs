using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Interface
{
    internal interface IEntityCRUD<T> where T : class
    {
        IEnumerable<T> GetAllEntity();
        T GetByIdEntity(params object[] paramters);
        void InsertEntity(T entity);
        void UpdateEntity(T entity);
        void UpdateRangeEntity(params T[] entity);
        void DeleteEntity(T entity);
        void DeleteRangeEntity(params T[] entity);
    }
}
