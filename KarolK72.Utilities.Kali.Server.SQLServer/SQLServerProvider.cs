using Dapper;
using KarolK72.Utilities.Kali.Server.Library.Models;
using KarolK72.Utilities.Kali.Server.Library.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KarolK72.Utilities.Kali.Server.SQLServer
{
    public class SQLServerProvider : ISqlProvider
    {
        private readonly SqlConnection _connection;
        private bool disposedValue;
        private SqlTransaction _transaction;
        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }
        public SQLServerProvider(SqlConnection connection)
        {
            _connection = connection;
        }
        private const string InsertLogSQL = @"  INSERT INTO KaliLogs(
                                                    LogLevel, Category, EventID, EventName, RenderedMessage, Scopes, ExceptionJSON, DateTimeCreated, DateTimeModified, RowVer
                                                    )
                                                OUTPUT INSERTED.*
                                                VALUES(
                                                    @LogLevel, @Category, @EventID, @EventName, @RenderedMessage, @Scopes, @ExceptionJSON, GETDATE() , GETDATE() , @RowVer
                                                )";

        public KaliLog InsertLog(KaliLog log)
        {
            KaliLog? queryResult = _connection.QueryFirstOrDefault<KaliLog?>(InsertLogSQL, new
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
        public async Task<KaliLog> InsertLogAsync(KaliLog log)
        {
            KaliLog? queryResult = await _connection.QueryFirstOrDefaultAsync<KaliLog?>(InsertLogSQL, new
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

        private const string InsertApplicationSQL = @"  INSERT INTO Applications(
                                                            ApplicationName, ApplicationGUID, DateTimeCreated, DateTimeModified, RowVer
                                                        )
                                                        OUTPUT INSERTED.*
                                                        VALUES(
                                                            @ApplicationName, @ApplicationGUID, GETDATE(), GETDATE(), 1
                                                        )";

        public Application InsertApplication(Application application)
        {
            Application? queryResult = _connection.QueryFirstOrDefault<Application?>(InsertApplicationSQL, new
            {
                ApplicationName = application.ApplicationName,
                ApplicationGUID = application.ApplicationGUID

            }, transaction: _transaction);

            if (queryResult is null)
                throw new Exception("Insert query failed. Returned nothing");

            return queryResult;
        }

        public async Task<Application> InsertApplicationAsync(Application application)
        {
            Application? queryResult = await _connection.QueryFirstOrDefaultAsync<Application?>(InsertApplicationSQL, new
            {
                ApplicationName = application.ApplicationName,
                ApplicationGUID = application.ApplicationGUID

            }, transaction: _transaction);

            if (queryResult is null)
                throw new Exception("Insert query failed. Returned nothing");

            return queryResult;
        }

        public Application UpdateApplication(Application application)
        {
            throw new NotImplementedException();

        }

        public Task<Application> UpdateApplicationAsync(Application application)
        {
            throw new NotImplementedException();
        }

        private const string DeleteApplicationSQL = @"DELETE FROM Applications WHERE ApplicationID = @ApplicationID AND RowVer = @RowVer SELECT @@ROWCOUNT"; //NEED TO HANDLE API KEYS

        public void DeleteApplication(Application application)
        {
            int queryResult = _connection.ExecuteScalar<int>(DeleteApplicationSQL, new
            {
                ApplicationID = application.ApplicationName,
                RowVer = application.RowVer

            }, transaction: _transaction);

            if (queryResult == 0)
                throw new DBConcurrencyException();
        }

        public async Task DeleteApplicationAsync(Application application)
        {
            int queryResult = await _connection.ExecuteScalarAsync<int>(DeleteApplicationSQL, new
            {
                ApplicationID = application.ApplicationID,
                RowVer = application.RowVer

            }, transaction: _transaction);

            if (queryResult == 0)
                throw new DBConcurrencyException();
        }

        private const string SelectAllApplicationsSQL = @"SELECT * FROM Applications";

        public List<Application> GetAllApplications()
        {
            List<Application> queryResult = _connection.Query<Application>(SelectAllApplicationsSQL, transaction: _transaction).ToList();

            if (queryResult is null)
                throw new Exception("Insert query failed. Returned nothing");

            return queryResult;
        }

        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            List<Application> queryResult = (await _connection.QueryAsync<Application>(SelectAllApplicationsSQL, transaction: _transaction)).ToList();

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

                    if (_transaction != null)
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

        
    }
}

