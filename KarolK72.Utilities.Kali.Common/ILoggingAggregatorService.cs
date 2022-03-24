using KarolK72.Utilities.Kali.Proto;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Common
{
    public interface ILoggingAggregatorService
    {
        Task<InitialConnectionResponse> EstablishClient(InitialConnectionRequest request);
        Task<LogResponse> Log(LogRequest request);
    }
}