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
            //_logger.LogInformation("PerseveranceLandResponseNotificationHandler - json: " + JsonSerializer.Serialize(notification.State));
            //_logger.LogInformation("PerseveranceLandResponseNotificationHandler - name: " + notification.State.Name);
            return _hubContext.Clients.Clients(notification.ConnectionId).StateResponseAsync(notification.State);
            //return _hubContext.Clients.Clients(notification.ConnectionId).SendAsync("PerseveranceLandResponseAsync", notification.State, cancellationToken);
        }
    }
}
