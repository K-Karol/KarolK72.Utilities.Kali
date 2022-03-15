using Grpc.Core;
using KarolK72.Utilities.Kali.Library.Models;
using KarolK72.Utilities.Kali.Library.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Library.Services
{
    public class LoggingGRPCServerService : Protos.LoggingGRPCServerService.LoggingGRPCServerServiceBase
    {
        private readonly ILogger<LoggingGRPCServerService> _logger;
        private readonly ILoggingAggregatorService _loggingAggregatorService;
        public LoggingGRPCServerService(ILogger<LoggingGRPCServerService> logger, ILoggingAggregatorService loggingAggregatorService)
        {
            _logger = logger;
            _loggingAggregatorService = loggingAggregatorService;
        }

        public override Task<InitialConnectionResponse> EstablishConnection(InitialConnectionRequest request, ServerCallContext context)
            => _loggingAggregatorService.EstablishClient(request);




    }
}
