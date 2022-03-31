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

        #region KaliLog
        Models.KaliLog InsertLog(Models.KaliLog log);
        Task<Models.KaliLog> InsertLogAsync(Models.KaliLog log);
        #endregion

        #region Application
        Models.Application InsertApplication(Models.Application application);
        Task<Models.Application> InsertApplicationAsync(Models.Application application);

        Models.Application UpdateApplication(Models.Application application);
        Task<Models.Application> UpdateApplicationAsync(Models.Application application);

        List<Models.Application> GetAllApplications();
        Task<List<Models.Application>> GetAllApplicationsAsync();


        #endregion
    }
}
