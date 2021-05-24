using Perseverance.Proxy.Host.Models;
using System;
using System.Threading.Tasks;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceService
    {
        Task<PerseveranceState> LandAsync();
        Task<PerseveranceState> MoveAsync(Guid guid, string command);
    }

    public class PerseveranceService : IPerseveranceService
    {
        private readonly PerseveranceStateService _stateService;

        public PerseveranceService(PerseveranceStateService stateService)
        {
            _stateService = stateService;
        }
        public Task<PerseveranceState> LandAsync()
        {
            
            var state = RoverFactory.Create()
                                    .ToState(Guid.NewGuid());

            _stateService.Cache.TryAdd(state.Guid, state);

            return Task.FromResult(state);
        }

        public Task<PerseveranceState> MoveAsync(Guid guid, string command)
        {
            var state = _stateService.Cache[guid]
                                    .ToRover()
                                    .Move(command)
                                    .ToState(guid);

            _stateService.Cache[guid] = state;

            return Task.FromResult(state);
        }
    }
}
