using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Application.Common.Behaviours
{
    public interface ICacheableMediatrQuery
    {
        bool BypassCache { get; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
