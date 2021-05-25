using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Perseverance.Proxy.Host.Models;
using System.Threading;
using System.Threading.Tasks;
using Perseverance.Proxy.Host.Hubs;

namespace Perseverance.Proxy.Host.Handlers
{
    public class StateEventNotificationHandler : INotificationHandler<StateEvent>
    {
        private readonly IHubContext<NotificationHub, IPushNotificationHubClient> _hubContext;
        private readonly ILogger<StateEventNotificationHandler> _logger;

        public StateEventNotificationHandler(
            IHubContext<NotificationHub, IPushNotificationHubClient> hubContext,
            ILogger<StateEventNotificationHandler> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public Task Handle(StateEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StateEventNotificationHandler - {notificationConnectionId}", notification.ConnectionId);
            return _hubContext.Clients.Clients(notification.ConnectionId).StateResponseAsync(notification.State);
        }
    }
}
