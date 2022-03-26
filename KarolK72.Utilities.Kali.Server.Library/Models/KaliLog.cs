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
        public long KaliLogID { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? Category { get; set; }
        //public EventId EventId { get; set; }
        public int EventID { get; set; }
        public string? EventName { get; set; }
        public string RenderedMessage { get; set; }
        public string? Scopes { get; set; }
        public string? ExceptionJSON
        {
            get
            {
                if (Exception != null)
                    return Newtonsoft.Json.JsonConvert.SerializeObject(Exception);
                else
                    return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        Exception = Newtonsoft.Json.JsonConvert.DeserializeObject<Exception?>(value!);
                    }
                    catch
                    {
                        Exception = null;
                    }
                }
                else
                    Exception = null;
            }
        }

        public Exception? Exception { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public int RowVer { get; set; }

    }
}
