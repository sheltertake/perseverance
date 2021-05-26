using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Perseverance.Proxy.Host.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceStateService
    {
        Task<PerseveranceState> GetStateAsync(Guid guid, CancellationToken cancellationToken);
        Task SetAsync(Guid guid, PerseveranceState state, CancellationToken cancellationToken);
    }

    public class PerseveranceStateService : IPerseveranceStateService
    {
        private readonly IDistributedCache _distributedCache;
        public PerseveranceStateService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<PerseveranceState> GetStateAsync(Guid guid, CancellationToken cancellationToken)
        {
            var cache = await _distributedCache.GetStringAsync(guid.ToString(), token: cancellationToken);
            return JsonConvert.DeserializeObject<PerseveranceState>(cache);
        }

        public Task SetAsync(Guid guid, PerseveranceState state, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
            return _distributedCache.SetStringAsync(guid.ToString(), JsonConvert.SerializeObject(state), options, cancellationToken);
        }
        
    }
}