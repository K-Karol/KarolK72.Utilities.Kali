using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarolK72.Utilities.Kali.Client.Library.Models
{
    public class KaliLog
    {
        public LogLevel LogLevel { get; }
        public string Category { get; }
        public EventId EventId { get; }
        public Exception? Exception { get; }
        public string RenderedMessage { get; }
        public string[] Scopes { get; }
        public KaliLog(LogLevel logLevel, string category, EventId eventId, Exception? exception, string[] scopes, string renderedMessage)
        {
            LogLevel = logLevel;
            Category = category;
            EventId = eventId;
            Exception = exception;
            Scopes = scopes;
            RenderedMessage = renderedMessage;
        }
    }
}
