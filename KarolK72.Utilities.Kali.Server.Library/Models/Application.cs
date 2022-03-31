using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarolK72.Utilities.Kali.Server.Library.Models
{
    [Table("Applications")]
    public class Application
    {
        public long ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public Guid ApplicationGUID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }
        public int RowVer { get; set; }
    }
}
