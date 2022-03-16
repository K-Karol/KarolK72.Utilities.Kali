using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Extensions
{
    public class KaliLoggerOptions
    {
        public string IPAddress { get; set; } = "https://localhost";
        public int Port { get; set; } = 6001;
        public string SourceName { get; set; } = Assembly.GetCallingAssembly()?.GetName().Name ?? "";
        public Guid SourceGUID { get; set; } = Guid.Empty;
    }
}
