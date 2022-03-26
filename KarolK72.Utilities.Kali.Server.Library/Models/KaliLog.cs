using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.Library.Models
{
    //server entity
    [Table("KaliLogs")]
    public class KaliLog
    {
        public ulong KaliLogID { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? Category { get; set; }
        //public EventId EventId { get; set; }
        public int EventID { get; set; }
        public string? EventName { get; set; }
        
        public string Scopes { get; set; }

        private string _exceptionJSON;
        public string ExceptionJSON
        {
            get => _exceptionJSON;
            set
            {
                _exceptionJSON = value;
                if (!string.IsNullOrWhiteSpace(_exceptionJSON))
                {
                    try
                    {
                        Exception = System.Text.Json.JsonSerializer.Deserialize<Exception>(_exceptionJSON) as Exception;
                    }
                    catch
                    {
                        Exception = null;
                    }
                }
            }
        }

        public Exception? Exception { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public int RowVer { get; set; }

    }
}
