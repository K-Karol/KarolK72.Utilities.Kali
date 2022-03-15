using KarolK72.Utilities.Kali.Library.Models;
using KarolK72.Utilities.Kali.Library.Protos;
using Microsoft.Extensions.Hosting;
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

        public LoggingAggregatorService()
        {
        }

        public Task<InitialConnectionResponse> EstablishClient(InitialConnectionRequest request)
        {
            string identifier = $"{request.GuidIdentifier}{(request.InstanceID > 0 ? $"{request.InstanceID}" : "")}"; //very basic tbh...
            string key = ComputeSha256Hash(identifier);
            if (_loggerClients.ContainsKey(key))
            {
                return Task.FromResult(new InitialConnectionResponse() { Succesfull = false });
            }

            var loggerClientInfo = new LoggerClientInfo() { SourceName = request.FriendlySourceName, Key = key }; //add salt??

            bool success = _loggerClients.TryAdd(key, loggerClientInfo);
            return Task.FromResult(new InitialConnectionResponse() { Succesfull = success, Key = success ? loggerClientInfo.Key : null });
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
    }
}
