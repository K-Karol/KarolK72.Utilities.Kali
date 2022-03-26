using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.Library.Services
{
    public interface ISqlProvider : IDisposable
    {
        IDbTransaction BeginTransaction();
        //void CommitTranscation();
        Models.KaliLog InsertLog(Models.KaliLog log);
        Task<Models.KaliLog> InsertLogAsync(Models.KaliLog log);
    }
}
