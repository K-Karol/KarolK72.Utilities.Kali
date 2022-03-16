using KarolK72.Utilities.Kali.Library.Protos;

namespace KarolK72.Utilities.Kali.Library.Services
{
    public interface ILoggingAggregatorService
    {
        Task<InitialConnectionResponse> EstablishClient(InitialConnectionRequest request);
        Task<LogResponse> Log(LogRequest request);
    }
}