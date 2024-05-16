using System;

namespace Demo.Infrastructure
{
    public interface ILoopTimer : IDisposable
    {
        void Start();
        void Stop();
    }
}
