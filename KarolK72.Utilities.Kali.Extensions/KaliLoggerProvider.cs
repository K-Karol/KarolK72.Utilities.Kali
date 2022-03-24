using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using KarolK72.Utilities.Kali.Client.Library;
using KarolK72.Utilities.Kali.Proto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Extensions
{
    [ProviderAlias("Kali")]
    public class KaliLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly IOptionsMonitor<KaliLoggerOptions> _options;
        private readonly ConcurrentDictionary<string, KaliLogger> _loggers;
        private readonly KaliLogProcessor _messageQueue;
        private readonly KaliClientService _client;

        private IDisposable _optionsReloadToken;
        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;


        public KaliLoggerProvider(IOptionsMonitor<KaliLoggerOptions> options)
        {
            _options = options;
            HttpMessageHandler httpHandler = null;
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            httpHandler = handler;
            // Return `true` to allow certificates that are untrusted/invalid
            
#else
            var innerHandler = new HttpClientHandler();
            innerHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var handler = new GrpcWebHandler(innerHandler);
            httpHandler = handler;
#endif

            GrpcChannel channel = GrpcChannel.ForAddress($"{options.CurrentValue.IPAddress}:{options.CurrentValue.Port}", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new KaliClientService(channel);

            var success = _client.EstablishConnection(new InitialConnectionRequest() { FriendlySourceName = options.CurrentValue.SourceName, GuidIdentifier = options.CurrentValue.SourceGUID.ToString(), InstanceID = 0 });

            if (!success)
                throw new Exception("Could not establish connection to the Kali logging service");

            _loggers = new ConcurrentDictionary<string, KaliLogger>();

            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);

            _messageQueue = new KaliLogProcessor(_client);
        }

        public ILogger CreateLogger(string name)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return _loggers.TryGetValue(name, out KaliLogger logger) ?
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                logger :
                _loggers.GetOrAdd(name, new KaliLogger(name, _messageQueue)
                {
                    Options = _options.CurrentValue,
                    ScopeProvider = _scopeProvider,
                });
        }

        private void ReloadLoggerOptions(KaliLoggerOptions options)
        {
            foreach (KeyValuePair<string, KaliLogger> logger in _loggers)
            {
                logger.Value.Options = options;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _messageQueue.Dispose();
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;

            foreach (KeyValuePair<string, KaliLogger> logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }
    }


}
