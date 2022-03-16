using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Extensions
{
    internal class KaliLog
    {
        public LogLevel LogLevel
        {
            get;
        }
        public string Category
        {
            get;
        }
        public EventId EventId
        {
            get;
        }

        public Exception? Exception
        {
            get;
        }

        public string RenderedMessage
        {
            get;
        }

        public string[] Scopes
        {
            get;
        }
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