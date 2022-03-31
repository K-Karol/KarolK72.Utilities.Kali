using KarolK72.Utilities.Kali.Server.Library.Models;
using KarolK72.Utilities.Kali.Server.Library.Services;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using Dapper;

namespace KarolK72.Utilities.Kali.Server.PostgreSQL
{
    public class PostgreSQLProvider : ISqlProvider
    {
        private readonly NpgsqlConnection _connection;
        private bool disposedValue;
        private NpgsqlTransaction _transaction;

        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        //public void CommitTranscation()
        //{
        //    _transaction.Commit();
        //}

        public PostgreSQLProvider(NpgsqlConnection connection!!)
        {
            _connection = connection;
        }

        private const string InsertLogSQL = @"  INSERT INTO KaliLogs(
                                                    LogLevel, Category, EventID, EventName, RenderedMessage, Scopes, ExceptionJSON, DateTimeCreated, DateTimeModified, RowVer
                                                    )
                                                VALUES(
	                                                @LogLevel, @Category, @EventID, @EventName, @RenderedMessage, @Scopes, @ExceptionJSON, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @RowVer
                                                ) RETURNING *";

        public KaliLog InsertLog(KaliLog log)
        {
            KaliLog? queryResult = _connection.QueryFirstOrDefault<KaliLog?>(InsertLogSQL, new
            {
                LogLevel = log.LogLevel,
                Category = log.Category,
                EventID = log.EventID,
                EventName = log.EventName,
                RenderedMessage = log.RenderedMessage,
                Scopes = log.Scopes,
                ExceptionJSON = log.ExceptionJSON,
                RowVer = 1
            }, transaction: _transaction);

            if (queryResult is null)
                throw new Exception("Insert query failed. Returned nothing");

            return queryResult;

        }
        public async Task<KaliLog> InsertLogAsync(KaliLog log)
        {
            KaliLog? queryResult = await _connection.QueryFirstOrDefaultAsync<KaliLog?>(InsertLogSQL, new
            {
                LogLevel = log.LogLevel,
                Category = log.Category,
                EventID = log.EventID,
                EventName = log.EventName,
                Scopes = log.Scopes,
                ExceptionJSON = log.ExceptionJSON,
                RowVer = 1
            }, transaction: _transaction);

            if (queryResult is null)
                throw new Exception("Insert query failed. Returned nothing");

            return queryResult;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    if(_transaction != null)
                    {
                        try
                        {
                            _transaction.Rollback();
                        }
                        catch { }
                        finally
                        {   
                            _transaction.Dispose();
                        }
                    }

                    _connection.Close();
                    _connection.Dispose();

                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Application InsertApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task<Application> InsertApplicationAsync(Application application)
        {
            throw new NotImplementedException();
        }

        public Application UpdateApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task<Application> UpdateApplicationAsync(Application application)
        {
            throw new NotImplementedException();
        }

        public List<Application> GetAllApplications()
        {
            throw new NotImplementedException();
        }

        public Task<List<Application>> GetAllApplicationsAsync()
        {
            throw new NotImplementedException();
        }

        public void DeleteApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApplicationAsync(Application application)
        {
            throw new NotImplementedException();
        }
    }
}