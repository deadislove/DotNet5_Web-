using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Interface
{
    public interface IGenericTypeRepository<T> : IDisposable where T : class
    {
        DbContext GetDBContext { get; }

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Execute the stored procedure.
        IEnumerable<T> ExeSpReturnObj(string SP_Name, params object[] parameters);
        IEnumerable<T> ExeSpReturnObj_ImportSQLInjection(string SP_Name, params object[] parameters);
    }
}
