using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Interface
{
    internal interface IAsyncGenericTypeRepository<T> : IDisposable where T : class
    {
        DbContext GetDBContext { get; }

        Task<IEnumerable<T>> AsyncGetAll();
        Task<T> AsyncGetById(params object[] parametets);
        Task AsyncInsert(T entity);
        Task AsyncUpdate(T entity);
        Task<bool> AsyncDelete(T entity);

        // Execute the stored procedure.
        Task<IEnumerable<T>> AsyncExeSpReturnObj(string SP_Name, params object[] parameters);
        Task<IEnumerable<T>> AsyncExeSpReturnObj_ImportSQLInjection(string SP_Name, params object[] parameters);
    }
}
