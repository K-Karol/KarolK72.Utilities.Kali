using Grpc.Core;
using KarolK72.Utilities.Kali.Library.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Library.Services
{
    public class LoggingGRPCClientService : Protos.LoggingGRPCServerService.LoggingGRPCServerServiceClient
    {

        private string _key = null;
        public LoggingGRPCClientService(ChannelBase channel) : base(channel)
        {
        }

        public LoggingGRPCClientService(CallInvoker callInvoker) : base(callInvoker)
        {
        }

        protected LoggingGRPCClientService()
        {
        }

        protected LoggingGRPCClientService(ClientBaseConfiguration configuration) : base(configuration)
        {
        }


        public bool EstablishConnection(InitialConnectionRequest initialConnectionRequest)
        {
            var response = EstablishConnection(initialConnectionRequest, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(20)));
            if (response == null || !response.Succesful)
                return false;
            _key = response.Key;
            return true;
        }

        public void Log(LogRequest logRequest)
        {
            logRequest.Key = _key;
            var response = SendLog(logRequest);
        }
    }
}
