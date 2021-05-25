using Perseverance.Proxy.Host.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceService
    {
        Task<PerseveranceState> LandAsync();
        Task<PerseveranceState> MoveAsync(Guid guid, string command);
    }

    public class PerseveranceService : IPerseveranceService
    {
        private readonly IPerseveranceStateService _stateService;
        private readonly ILogger<PerseveranceService> _logger;

        public PerseveranceService(
            IPerseveranceStateService stateService,
            ILogger<PerseveranceService> logger)
        {
            _stateService = stateService;
            _logger = logger;
        }
        public Task<PerseveranceState> LandAsync()
        {

            var state = RoverFactory.Create(
                    x: 1,
                    y: 1,
                    w: 4,
                    h: 3,
                    obstacles: new[]
                    {
                        new Obstacle{ X = 0, Y = 0},
                        new Obstacle{ X = 2, Y = 2},
                        new Obstacle{ X = 0, Y = 2},
                        new Obstacle{ X = 2, Y = 0}
                    }
                    )
                .ToState(Guid.NewGuid());

            _stateService.Cache.TryAdd(state.Guid, state);

            return Task.FromResult(state);
        }

        public Task<PerseveranceState> MoveAsync(Guid guid, string command)
        {
            _logger.LogInformation("MoveAsync {command}", command);

            var cache = _stateService.Cache[guid];
            _logger.LogInformation(JsonConvert.SerializeObject(cache.Map));

            var rover = cache
                        .ToRover()
                        .Move(command);

            _logger.LogInformation(JsonConvert.SerializeObject(rover.Planet.Map));

            var state = rover.ToState(guid);

            _stateService.Cache[guid] = state;

            return Task.FromResult(state);
        }
    }
}
