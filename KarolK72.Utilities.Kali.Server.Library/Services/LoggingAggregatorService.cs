using KarolK72.Data.Common;
using KarolK72.Utilities.Kali.Common;
using KarolK72.Utilities.Kali.Common.Models;
using KarolK72.Utilities.Kali.Proto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.Library.Services
{
    public class LoggingAggregatorService : ILoggingAggregatorService
    {
        private ConcurrentDictionary<string, LoggerClientInfo> _loggerClients = new ConcurrentDictionary<string, LoggerClientInfo>();
        private readonly IUnitOfWorkFactory<ISqlProvider> _unitOfWorkFactory;
        private readonly ILogger _logger;
        //private StringBuilder _loggerStringBuilder = new StringBuilder();

        public LoggingAggregatorService(ILogger<LoggingAggregatorService> logger,IUnitOfWorkFactory<ISqlProvider> unitOfWorkFactory)
        {
            _logger = logger;

            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public Task<InitialConnectionResponse> EstablishClient(InitialConnectionRequest request)
        {
            string identifier = $"{request.GuidIdentifier}{(request.InstanceID > 0 ? $"{request.InstanceID}" : "")}"; //very basic tbh...
            string key = ComputeSha256Hash(identifier);
            if (_loggerClients.ContainsKey(key))
            {
                return Task.FromResult(new InitialConnectionResponse() { Succesful = false });
            }

            var loggerClientInfo = new LoggerClientInfo() { SourceName = request.FriendlySourceName, Key = key }; //add salt??

            bool success = _loggerClients.TryAdd(key, loggerClientInfo);
            return Task.FromResult(new InitialConnectionResponse() { Succesful = success, Key = success ? loggerClientInfo.Key : null });
        }
        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<LogResponse> Log(LogRequest request)
        {
            #region old
            //StringBuilder loggerStringBuilder = new StringBuilder();

            //loggerStringBuilder.Append($"[{DateTime.Now:s}]");
            //loggerStringBuilder.Append($"[{lci.SourceName}]");
            //loggerStringBuilder.Append($"[{request.Category}]");
            //LogLevel logLevel = (LogLevel)request.LogLevel;
            //switch (logLevel)
            //{
            //    case LogLevel.Trace:
            //        loggerStringBuilder.Append("[Trace]");
            //        break;
            //    case LogLevel.Debug:
            //        loggerStringBuilder.Append("[Debug]");
            //        break;
            //    case LogLevel.Information:
            //        loggerStringBuilder.Append("[Info ]");
            //        break;
            //    case LogLevel.Warning:
            //        loggerStringBuilder.Append("[Warn ]");
            //        break;
            //    case LogLevel.Error:
            //        loggerStringBuilder.Append("[Error]");
            //        break;
            //    case LogLevel.Critical:
            //        loggerStringBuilder.Append("[Crit ]");
            //        break;
            //    case LogLevel.None:
            //        loggerStringBuilder.Append("[     ]");
            //        break;
            //    default:
            //        loggerStringBuilder.Append("[     ]");
            //        break;
            //}

            //string[] scopes = System.Text.Json.JsonSerializer.Deserialize<string[]>(request.Scopes) ?? new string[0];
            //loggerStringBuilder.Append($"[{string.Join(">>>", scopes)}]");
            //loggerStringBuilder.Append($" {request.RenderedMessage}");
            //Exception? exception = System.Text.Json.JsonSerializer.Deserialize<Exception>(request.Exception) ?? null;

            //if (exception != null)
            //{
            //    loggerStringBuilder.Append($"{Environment.NewLine}{exception}");
            //}
            //Console.WriteLine(loggerStringBuilder.ToString());
            #endregion

            if (string.IsNullOrWhiteSpace(request.Key) || !_loggerClients.TryGetValue(request.Key, out LoggerClientInfo lci))
            {
                return new LogResponse() { Succesful = false };
            }

            Models.KaliLog kaliLog = new Models.KaliLog()
            {
                Category = request.Category,
                EventID = request.EventId,
                LogLevel = (LogLevel)request.LogLevel,
                EventName = request.EventName,
                RenderedMessage = request.RenderedMessage,
                ExceptionJSON = request.Exception,
                Scopes = request.Scopes
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            var uow = _unitOfWorkFactory.CreateNew();
            try
            {
                await uow.Work.InsertLogAsync(kaliLog);
                await uow.SaveAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error whilst adding log to db");
            }
            finally
            {
                uow.Dispose();
            }
            stopwatch.Stop();
            Console.WriteLine($"It took {stopwatch.ElapsedMilliseconds}ms  to add to db");
            return new LogResponse() { Succesful = true };
        }
    }
}
