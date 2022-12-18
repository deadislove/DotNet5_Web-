using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Core.Interface
{
    public interface ICRUD<T> : IDisposable where T : class
    {
        void Create(T entity);
        IEnumerable<T> Read();
        void Update(T entity);
        void Delete(T entity);
    }
}
