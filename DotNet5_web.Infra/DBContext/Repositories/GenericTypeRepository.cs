using DotNet5_web.Infra.DBContext.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext.Repositories
{
    public class GenericTypeRepository<T> : IGenericTypeRepository<T>, IEntityCRUD<T> where T : class
    {
        private bool disposedValue;
        private readonly DemoDbContext _context;

        internal DbSet<T> _entities;

        public GenericTypeRepository(DemoDbContext context) { 
            _context = context;
        }

        public virtual IQueryable<T> Table {
            get { return _entities; }
        }

        public virtual DbSet<T> Entities {
            get { return _entities ??= _context.Set<T>(); }
        }

        public DbContext GetDBContext => _context;
        
        #region Stored procdeure
        public IEnumerable<T> ExeSpReturnObj(string SP_Name, params object[] parameters)
        {
            if (string.IsNullOrEmpty(SP_Name))
                throw new("SP_Name is empty or null.");
            else if(parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            try
            {
                return _context.Set<T>().FromSqlRaw(SP_Name, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DatabaseDispose();
            }            
        }

        public IEnumerable<T> ExeSpReturnObj_ImportSQLInjection(string SP_Name, params object[] parameters)
        {
            if (string.IsNullOrEmpty(SP_Name))
                throw new("SP_Name is empty or null.");
            else if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            try
            {
                return _context.Set<T>().FromSqlInterpolated($"EXEC {SP_Name} {parameters}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DatabaseDispose();
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get all of data in the Table(T)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }
        #endregion

        #region GetAll(Where condition)
        /// <summary>
        /// Get all data with "where" condition.
        /// </summary>
        /// <param name="predicate">Where condition</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            return _context.Set<T>().Where(predicate).AsEnumerable();
        }
        #endregion

        #region GetById
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">PK of Table</param>
        /// <returns></returns>
        public T GetById(int id)
        {
            try
            {
                if (id != 0)
                {
                    return _context.Set<T>().Find(id);
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
                DatabaseDispose();
            }
        }
        #endregion

        #region GetById(params object[] param parameters )
        /// <summary>
        /// Get by object array.
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public T GetById(params object[] parameters)
        {
            try
            {
                if (parameters is not null)
                {
                    return _context.Set<T>().Find(parameters);
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
                DatabaseDispose();
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert the entity object(T).
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
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
                DatabaseDispose();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update the entity object(T) in Table(T)
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            if(entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                _context.Entry<T>(entity).State = EntityState.Modified;
                _context.SaveChanges();
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
                DatabaseDispose();
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete the entity object(T) in the table(T)
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delete(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                _context.Entry<T>(entity).State = EntityState.Deleted;
                _context.SaveChanges();
                dbTrasction.Commit();
            }
            catch (Exception ex)
            {
                dbTrasction.RollbackToSavepoint(MethodBase.GetCurrentMethod().Name);
                throw;
            }
            finally
            {
                dbTrasction.ReleaseSavepoint(MethodBase.GetCurrentMethod().Name);
                dbTrasction.Dispose();
                DatabaseDispose();
            }
        }
        #endregion

        #region GetAllEntity
        /// <summary>
        /// Get all data object in the Table by the entity table variable.
        /// </summary>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T> GetAllEntity()
        {
            return Entities.AsEnumerable();
        }
        #endregion

        #region GetByIdEntity
        /// <summary>
        /// Get the entity data by ID which is by the entity table variables.
        /// </summary>
        /// <param name="parameters">object array</param>
        /// <returns> T entity </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T GetByIdEntity(params object[] parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            try
            {
                if (parameters is not null)
                {
                    return Entities.Find(parameters);
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
                DatabaseDispose();
            }
        }
        #endregion

        #region InsertEntity
        /// <summary>
        /// Insert the entity data to the table(T) which is by the entity table variable.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void InsertEntity(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                Entities.Add(entity);
                _context.SaveChanges();
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
                DatabaseDispose();
            }
        }
        #endregion

        #region UpdateEntity
        /// <summary>
        /// Update the entity data in the Table(T) by the entity table variable.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateEntity(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                Entities.Update(entity);
                _context.SaveChanges();
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
                DatabaseDispose();
            }
        }
        #endregion

        #region UpdateRangeEntity
        /// <summary>
        /// Update the entity data range in the entity table(T)
        /// </summary>
        /// <param name="entity">T array</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateRangeEntity(params T[] entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                Entities.UpdateRange(entity);
                _context.SaveChanges();
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
                DatabaseDispose();
            }
        }
        #endregion

        #region DeleteEntity
        /// <summary>
        /// Delete the entity data in the entity table(T) by the entity table variable.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeleteEntity(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                Entities.Remove(entity);
                _context.SaveChanges();
                dbTrasction.Commit();
            }
            catch (Exception ex)
            {
                dbTrasction.RollbackToSavepoint(MethodBase.GetCurrentMethod().Name);
                throw;
            }
            finally
            {
                dbTrasction.ReleaseSavepoint(MethodBase.GetCurrentMethod().Name);
                dbTrasction.Dispose();
                DatabaseDispose();
            }
        }
        #endregion

        #region DeleteRangeEntity
        /// <summary>
        /// Delete the enttiy data range in the entity table(T)
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeleteRangeEntity(params T[] entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            IDbContextTransaction dbTrasction = _context.Database.BeginTransaction();

            try
            {
                dbTrasction.CreateSavepoint(MethodBase.GetCurrentMethod().Name);
                Entities.RemoveRange(entity);
                _context.SaveChanges();
                dbTrasction.Commit();
            }
            catch (Exception ex)
            {
                dbTrasction.RollbackToSavepoint(MethodBase.GetCurrentMethod().Name);
                throw;
            }
            finally
            {
                dbTrasction.ReleaseSavepoint(MethodBase.GetCurrentMethod().Name);
                dbTrasction.Dispose();
                DatabaseDispose();
            }
        }
        #endregion

        #region Database dispose
        public void DatabaseDispose()
        {
            try
            {
                _context.Database.CloseConnection();
                //_context.Dispose();
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
        
        #region Dispose
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
        ~GenericTypeRepository()
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
