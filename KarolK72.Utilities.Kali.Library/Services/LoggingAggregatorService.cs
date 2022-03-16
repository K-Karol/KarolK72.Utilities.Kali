using KarolK72.Utilities.Kali.Library.Models;
using KarolK72.Utilities.Kali.Library.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Library.Services
{
    public class LoggingAggregatorService : ILoggingAggregatorService
    {
        private ConcurrentDictionary<string, LoggerClientInfo> _loggerClients = new ConcurrentDictionary<string, LoggerClientInfo>();

        //private StringBuilder _loggerStringBuilder = new StringBuilder();

        public LoggingAggregatorService()
        {
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

        public Task LogMessage(string message)
        {
            Console.WriteLine("Log Received: " + message);
            return Task.CompletedTask;
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

        public Task<LogResponse> Log(LogRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Key) || !_loggerClients.TryGetValue(request.Key, out LoggerClientInfo lci))
            {
                return Task.FromResult(new LogResponse() { Succesful = false });
            }

            StringBuilder loggerStringBuilder = new StringBuilder();

            loggerStringBuilder.Append($"[{DateTime.Now:s}]");
            loggerStringBuilder.Append($"[{lci.SourceName}]");
            loggerStringBuilder.Append($"[{request.Category}]");
            LogLevel logLevel = (LogLevel)request.LogLevel;
            switch (logLevel)
            {
                case LogLevel.Trace:
                    loggerStringBuilder.Append("[Trace]");
                    break;
                case LogLevel.Debug:
                    loggerStringBuilder.Append("[Debug]");
                    break;
                case LogLevel.Information:
                    loggerStringBuilder.Append("[Info ]");
                    break;
                case LogLevel.Warning:
                    loggerStringBuilder.Append("[Warn ]");
                    break;
                case LogLevel.Error:
                    loggerStringBuilder.Append("[Error]");
                    break;
                case LogLevel.Critical:
                    loggerStringBuilder.Append("[Crit ]");
                    break;
                case LogLevel.None:
                    loggerStringBuilder.Append("[     ]");
                    break;
                default:
                    loggerStringBuilder.Append("[     ]");
                    break;
            }

            string[] scopes = System.Text.Json.JsonSerializer.Deserialize<string[]>(request.Scopes) ?? new string[0];
            loggerStringBuilder.Append($"[{string.Join(">>>", scopes)}]");
            loggerStringBuilder.Append($" {request.RenderedMessage}");
            Exception? exception = System.Text.Json.JsonSerializer.Deserialize<Exception>(request.Exception) ?? null;

            if(exception != null)
            {
                loggerStringBuilder.Append($"{Environment.NewLine}{exception}");
            }
            Console.WriteLine(loggerStringBuilder.ToString());

            return Task.FromResult(new LogResponse() { Succesful = true });
        }
    }
}
