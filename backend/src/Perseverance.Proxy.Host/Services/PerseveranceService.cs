using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Perseverance.Proxy.Host.Models;
using System;
using System.Threading.Tasks;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceService
    {
        Task<PerseveranceState> LandAsync(LandOptions options);
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
        public Task<PerseveranceState> LandAsync(LandOptions options)
        {
            var state = RoverFactory.Create(
                    x: options.X,
                    y: options.Y,
                    w: options.W,
                    h: options.H,
                    obstacles: options.RandomObstacles()
                    )
                .ToState(Guid.NewGuid());

            _stateService.Cache.TryAdd(state.Guid, state);

            return Task.FromResult(state);
        }

        public Task<PerseveranceState> MoveAsync(Guid guid, string command)
        {
            _logger.LogInformation("MoveAsync {command}", command);

            var cache = _stateService.Cache[guid];

            var rover = cache
                        .ToRover()
                        .Move(command);

            var state = rover.ToState(guid);

            _stateService.Cache[guid] = state;

            return Task.FromResult(state);
        }
    }
}
