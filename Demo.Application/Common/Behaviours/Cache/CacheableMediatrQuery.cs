using Demo.Application.Common.Behaviours;
using System;

namespace Demo.Application
{
    public class CacheableMediatrQuery : ICacheableMediatrQuery
    {
        public bool BypassCache { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public virtual string CacheKey => this.GetType().FullName;

    }

}
