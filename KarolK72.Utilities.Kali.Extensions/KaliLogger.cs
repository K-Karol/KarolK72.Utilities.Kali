using System;
using System.IO;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace KarolK72.Utilities.Kali.Extensions
{
    internal sealed class KaliLogger : ILogger
    {
        private readonly string _name;
        internal IExternalScopeProvider ScopeProvider { get; set; }
        private readonly KaliLogProcessor _queueProcessor;

        internal KaliLoggerOptions Options { get; set; }

        internal KaliLogger(string name, KaliLogProcessor queueProcessor)
        {
            _name = name;
            _queueProcessor = queueProcessor;
        }


        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            List<string> scopes = new List<string>();

            ScopeProvider.ForEachScope((obj, state) => scopes.Add(obj?.ToString() ?? ""), state);

            KaliLog kalilog = new KaliLog(logLevel, _name, eventId, exception, scopes.ToArray(), formatter(state, null));
            _queueProcessor.EnqueueMessage(kalilog);
        }

        public IDisposable BeginScope<TState>(TState state) =>
            ScopeProvider?.Push(state) ?? NullScope.Instance;
    }


    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    internal sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }

}
