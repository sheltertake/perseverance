using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Perseverance.Proxy.Host.Models;
using Perseverance.Proxy.Host.Services;

namespace Perseverance.Proxy.Host.Handlers
{
    public class MoveCommandNotificationHandler : INotificationHandler<MoveCommand>
    {
        private readonly IPerseveranceService _roverService;
        private readonly IMediator _mediator;
        private readonly ILogger<MoveCommandNotificationHandler> _logger;

        public MoveCommandNotificationHandler(
            IPerseveranceService roverService,
            IMediator mediator,
            ILogger<MoveCommandNotificationHandler> logger)
        {
            _roverService = roverService;
            _mediator = mediator;
            _logger = logger;
        }

        public Task Handle(MoveCommand notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MoveCommandNotificationHandler - {notificationCommand}", notification.Command);

            return _roverService.MoveAsync(notification.Guid, notification.Command).ContinueWith(x =>
            {
                _mediator.Publish(new StateEvent(notification.ConnectionId, x.Result), cancellationToken);
            }, cancellationToken);
        }
    }
}