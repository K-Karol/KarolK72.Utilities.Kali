using Grpc.Core;
using KarolK72.Utilities.Kali.Common;
using KarolK72.Utilities.Kali.Proto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.Library.Services
{
    public class KaliServerService : KaliService.KaliServiceBase
    {
        private readonly ILogger<KaliServerService> _logger;
        private readonly ILoggingAggregatorService _loggingAggregatorService;
        public KaliServerService(ILogger<KaliServerService> logger, ILoggingAggregatorService loggingAggregatorService)
        {
            _logger = logger;
            _loggingAggregatorService = loggingAggregatorService;
        }

        public override Task<InitialConnectionResponse> EstablishConnection(InitialConnectionRequest request, ServerCallContext context)
            => _loggingAggregatorService.EstablishClient(request);

        public override Task<LogResponse> SendLog(LogRequest request, ServerCallContext context) => _loggingAggregatorService.Log(request);
    }
}
