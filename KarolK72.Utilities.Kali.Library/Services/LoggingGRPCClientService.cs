using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Library.Services
{
    public class LoggingGRPCClientService : Protos.LoggingGRPCServerService.LoggingGRPCServerServiceClient
    {
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
    }
}
