using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarolK72.Utilities.Kali.Server.Library.Models
{
    [Table("ApplicationAPIKeys")]
    public class ApplicationAPIKey
    {
        public long ApplicationAPIKeyID { get; set; }
        public long ApplicationID { get; set; }
        public string? KeyName { get; set; }
        public string KeyHash { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public int RowVer { get; set; }
    }
}
