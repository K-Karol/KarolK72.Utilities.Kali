using KarolK72.Utilities.Kali.Server.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using KarolK72.Utilities.Kali.Server.Library.Models;
using Dapper;

namespace KarolK72.Utilities.Kali.Server.SQLite
{
    public class SQLiteProvider : ISqlProvider
    {
        private readonly SQLiteConnection _connection;
        private bool disposedValue;
        private SQLiteTransaction _transaction;
        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }
        public SQLiteProvider(SQLiteConnection connection)
        {
            _connection = connection;
        }
        private const string InsertLogSQL = @"  INSERT INTO KaliLogs(
                                                    LogLevel, Category, EventID, EventName, RenderedMessage, Scopes, ExceptionJSON, DateTimeCreated, DateTimeModified, RowVer
                                                    )
                                                VALUES(
	                                                @LogLevel, @Category, @EventID, @EventName, @RenderedMessage, @Scopes, @ExceptionJSON, datetime('now'), datetime('now'), @RowVer
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
