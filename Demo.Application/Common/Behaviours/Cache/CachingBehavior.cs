using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;
        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache;
            _logger = logger;
            _settings = settings.Value;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            if (request.BypassCache)
                return await next();

          
            var cachedResponse = await _cache.GetAsync((string)request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }
            return response;


            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();

                var slidingExpiration = request.SlidingExpiration == null ? TimeSpan.FromHours(_settings.SlidingExpiration) : request.SlidingExpiration;

                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                };
                var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));

                await _cache.SetAsync((string)request.CacheKey, serializedData, options, cancellationToken);

                return response;
            }
        }
    }
}
