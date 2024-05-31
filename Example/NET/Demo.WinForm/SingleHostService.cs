using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.NETWinForm
{
    internal class SingleHostService : BackgroundService
    {

        public SingleHostService( ILogger<SingleHostService> logger)
        {
            _logger = logger;
        }

        private readonly ILogger _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("The current time is: {CurrentTime}", DateTimeOffset.UtcNow);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogInformation($"ExecuteAsync of {this.GetType().Name}: The current time is: {DateTimeOffset.UtcNow}");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}