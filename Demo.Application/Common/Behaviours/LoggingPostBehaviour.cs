using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Common.Behaviours
{
    public class LoggingPostBehaviour<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingPostBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Post Behaviour Request: {Name}  {@Request}", requestName, request);
        }
    }
}
