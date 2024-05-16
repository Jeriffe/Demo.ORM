using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.NETConsole
{
    public class SingleService : IHostedService
    {
        IDisposable _schedule;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _schedule = new Timer(Callback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void Callback(object state)
        {
            Console.WriteLine($"{DateTime.Now} - Callback");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _schedule?.Dispose();
            return Task.CompletedTask;
        }
    }
}
