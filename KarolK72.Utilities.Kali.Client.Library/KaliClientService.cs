using Grpc.Core;
using KarolK72.Utilities.Kali.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Client.Library
{
    public class KaliClientService : KaliService.KaliServiceClient
    {

        private string _key = null;
        public KaliClientService(ChannelBase channel) : base(channel)
        {
        }

        public KaliClientService(CallInvoker callInvoker) : base(callInvoker)
        {
        }

        protected KaliClientService()
        {
        }

        protected KaliClientService(ClientBaseConfiguration configuration) : base(configuration)
        {
        }


        public bool EstablishConnection(InitialConnectionRequest initialConnectionRequest)
        {
            var response = EstablishConnection(initialConnectionRequest, new CallOptions(deadline: DateTime.UtcNow.AddSeconds(20)));
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
