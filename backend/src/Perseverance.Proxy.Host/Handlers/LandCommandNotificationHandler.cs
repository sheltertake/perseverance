﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Perseverance.Proxy.Host.Models;
using Perseverance.Proxy.Host.Services;

namespace Perseverance.Proxy.Host.Handlers
{
    public class LandCommandNotificationHandler : INotificationHandler<LandCommand>
    {
        private readonly IPerseveranceService _roverService;
        private readonly IMediator _mediator;
        private readonly ILogger<LandCommandNotificationHandler> _logger;

        public LandCommandNotificationHandler(
            IPerseveranceService roverService,
            IMediator mediator,
            ILogger<LandCommandNotificationHandler> logger)
        {
            _roverService = roverService;
            _mediator = mediator;
            _logger = logger;
        }

        public Task Handle(LandCommand notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("LandCommandNotificationHandler - {notificationConnectionId}", notification.ConnectionId);

            return _roverService.LandAsync().ContinueWith(x =>
            {
                _mediator.Publish(new StateEvent(notification.ConnectionId, x.Result), cancellationToken);
            }, cancellationToken);
        }
    }
}