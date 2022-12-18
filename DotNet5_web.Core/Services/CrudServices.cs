using DotNet5_web.Core.Interface;
using DotNet5_web.Infra.DBContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Core.Services
{
    public class CrudServices<T> : ICRUD<T> where T : class
    {
        private bool disposedValue;

        private IGenericTypeRepository<T> iGenericTypeRepository;

        public CrudServices(IGenericTypeRepository<T> iGenericTypeRepository) => this.iGenericTypeRepository = iGenericTypeRepository;

        public CrudServices(bool disposedValue, IGenericTypeRepository<T> iGenericTypeRepository)
        {
            this.disposedValue = disposedValue;
            this.iGenericTypeRepository = iGenericTypeRepository;
        }

        public void Create(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Read()
        {
            try
            {
                return iGenericTypeRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~CrudServices()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
