using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Unhandled Exception Behaviour Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                //NuGet\Install-Package Ben.Demystifier
                ex.Demystify();

                throw;
            }
        }
    }

    //public class RequestUnhandledExceptionHandler<TRequest, TResponse> : IRequestExceptionHandler<TRequest, TResponse>
    //{
    //    public Task Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            //TODO
    //            return Task.CompletedTask;
    //        }
    //        catch (Exception ex)
    //        {
    //            var requestName = typeof(TRequest).Name;

    //            //NuGet\Install-Package Ben.Demystifier
    //            ex.Demystify();

    //            throw;
    //        }
    //    }
    //}


}