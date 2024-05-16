using Demo.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.HostedWindowsServices
{
    public class ServiceProcessor : IProcessor
    {
        readonly ILogger logger = null;

        public int LoopInterval { get; set; } = 3;

        public string Name => GetType().Name;

        public ServiceProcessor(ILogger<ServiceProcessor> logger)
        {
            this.logger = logger;
        }

        public void DoProcess(object obj)
        {
            try
            {
                logger.LogInformation($"DoProcess of {this.Name} has been called");

            }
            catch (Exception ex)
            {

            }

        }

        public void Dispose()
        {
        }
    }
}
