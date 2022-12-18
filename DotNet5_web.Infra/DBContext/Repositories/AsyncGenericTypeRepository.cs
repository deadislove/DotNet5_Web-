using DotNet5_web.Infra.DBContext.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Repositories
{
    internal class AsyncGenericTypeRepository<T> : IAsyncGenericTypeRepository<T> where T : class
    {
        private bool disposedValue;
        private readonly DemoDbContext _context;

        internal DbSet<T> _entities;

        public AsyncGenericTypeRepository(DemoDbContext context) { 
            _context = context; 
        }

        public virtual DbSet<T> Entities
        {
            get { return _entities ??= _context.Set<T>(); }
        }

        public DbContext GetDBContext => _context;


        public async Task<IEnumerable<T>> AsyncExeSpReturnObj(string SP_Name, params object[] parameters)
        {
            if (string.IsNullOrEmpty(SP_Name))
                throw new("SP_Name is empty or null.");
            else if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            try
            {
                return await Task.FromResult(_context.Set<T>().FromSqlRaw(SP_Name, parameters).AsQueryable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await DatabaseDisposeAsync();
            }
        }

        public Task<IEnumerable<T>> AsyncExeSpReturnObj_ImportSQLInjection(string SP_Name, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> AsyncGetAll()
        {
            return await Task.FromResult(_context.Set<T>().AsEnumerable());
        }

        public async Task<T> AsyncGetById(params object[] paramters)
        {
            try
            {
                if (paramters is not null)
                {
                    return await _context.Set<T>().FindAsync(paramters);
                }
                else
                    throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await DatabaseDisposeAsync();
            }
        }

        public async Task AsyncInsert(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                dbTrasction.Commit();

            }
            catch (Exception ex)
            {
                dbTrasction.RollbackToSavepoint(MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            finally
            {
                dbTrasction.ReleaseSavepoint(MethodBase.GetCurrentMethod().Name);
                dbTrasction.Dispose();
                await DatabaseDisposeAsync();
            }
        }

        public async Task AsyncUpdate(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                _context.Entry<T>(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                dbTrasction.Commit();
            }
            catch (Exception ex)
            {
                dbTrasction.RollbackToSavepoint(MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            finally
            {
                dbTrasction.ReleaseSavepoint(MethodBase.GetCurrentMethod().Name);
                dbTrasction.Dispose();
                await DatabaseDisposeAsync();
            }
        }

        #region Async Delete
        /// <summary>
        /// Async Delete the entity object(T) in the table(T).
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AsyncDelete(T entity)
        {
            try
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
                int iResult = await _context.SaveChangesAsync();
                return iResult == 1;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await DatabaseDisposeAsync();
            }
        }
        #endregion        

        #region Database dispose (Async)
        public async Task<bool> DatabaseDisposeAsync()
        {
            try
            {
                await _context.Database.CloseConnectionAsync();
                //await _context.DisposeAsync();
                return true;
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
        #endregion

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
        ~AsyncGenericTypeRepository()
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
