using Demo.Infrastructure;
using Serilog;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.NETConsole
{

    public class LoopHostService : ILoopTimer
    {
        readonly ILogger logger = Log.ForContext(MethodBase.GetCurrentMethod().DeclaringType);

        private Task task;
        private CancellationTokenSource cts;
        private IProcessor processor = null;

        public LoopHostService(IProcessor processor)
        {
            this.processor = processor;
        }

        public void Start()
        {
            while (cts != null && cts.Token.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }

            if (task == null)
            {
                cts = new CancellationTokenSource();

                task = new Task(() => DoProcess(cts.Token), cts.Token);
                task.Start();
            }
            else if (task.IsCanceled)
            {
                task.Start();
            }
        }
        public void DoProcess(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        logger.Information($"{processor.Name} has been canceled.");

                        break;
                    }

                    try
                    {
                        logger.Information($"{processor.Name} Loop Start....");

                        processor.DoProcess();

                        Thread.Sleep(processor.LoopInterval * 1000);
                        //   Thread.Sleep(5 * 60 * 1000);

                        logger.Information($"{processor.Name} Loop End....");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, ex.Message);
                    }
                }
            }
            catch (AggregateException ex)
            {
                logger.Error(ex, ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            finally
            {
                //log end
                Dispose();

            }
        }

        public void Stop()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        public void Dispose()
        {
                Stop();

                cts = null;
                task = null;
        }
    }
}
