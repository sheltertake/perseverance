using Microsoft.Extensions.Logging;
using Perseverance.Proxy.Host.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceService
    {
        Task<PerseveranceState> LandAsync(LandOptions options, CancellationToken cancellationToken);
        Task<PerseveranceState> MoveAsync(Guid guid, string command, CancellationToken cancellationToken);
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
        public async Task<PerseveranceState> LandAsync(LandOptions options, CancellationToken cancellationToken)
        {
            var guid = options.Guid ?? Guid.NewGuid();
            var state = RoverFactory.Create(
                    x: options.X,
                    y: options.Y,
                    w: options.W,
                    h: options.H,
                    obstacles: options.RandomObstacles()
                    )
                .ToState(guid);

            await _stateService.SetAsync(state.Guid, state, cancellationToken);

            return state;
        }

        public async Task<PerseveranceState> MoveAsync(Guid guid, string command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MoveAsync {command}", command);

            var cache = await _stateService.GetStateAsync(guid, cancellationToken);

            var rover = cache
                        .ToRover()
                        .Move(command);

            var state = rover.ToState(guid);

            await _stateService.SetAsync(guid, state, cancellationToken);

            return state;
        }
    }
}
