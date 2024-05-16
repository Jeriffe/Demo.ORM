using Demo.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.HostedWindowsServices
{
    internal class SingleHostService : BackgroundService
    {
        private IProcessor processor;

        public SingleHostService(IProcessor processor, ILogger<SingleHostService> logger)
        {
            this.processor = processor;
            _logger = logger;
        }

        private readonly ILogger _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("The current time is: {CurrentTime}", DateTimeOffset.UtcNow);

            //    if (processor!=null)
            //    {
            //        processor.DoProcess();
            //    }

            //    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            //}

            _logger.LogInformation($"ExecuteAsync of {this.GetType().Name}: The current time is: {DateTimeOffset.UtcNow}");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            processor?.DoProcess(true);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            processor?.DoProcess(false);
            return base.StopAsync(cancellationToken);
        }
    }
}