using DotNet5_web.Core.Domain.Interface;
using DotNet5_web.Core.DTO;
using DotNet5_web.Core.Interface;
using DotNet5_web.Infra.DBContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Core.Domain.Services
{
    public class DomainServices : IDomain
    {
        private bool disposedValue = false;

        private ICRUD<Enterprise_MVC_Core> iCRUD;

        public DomainServices(ICRUD<Enterprise_MVC_Core> iCRUD)
        {
            this.iCRUD = iCRUD;
        }

        public DomainServices(bool disposedValue, ICRUD<Enterprise_MVC_Core> iCRUD)
        {
            this.disposedValue = disposedValue;
            this.iCRUD = iCRUD;
        }

        public void DomainExecute(out IEnumerable<Enterprise_MVC_CoreDTO> returnObj)
        {
            try
            {
                var Data = iCRUD.Read();

                if (Data is null)
                {
                    returnObj = new List<Enterprise_MVC_CoreDTO>();
                    return;
                }
                else
                {
                    returnObj = Data.Select(s => (Enterprise_MVC_CoreDTO)s);
                    return;
                }
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

        #region Dispose pattern
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
        ~DomainServices()
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
        #endregion
    }
}
