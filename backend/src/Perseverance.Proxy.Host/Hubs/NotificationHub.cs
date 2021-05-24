using System;
using MediatR;
using Microsoft.Extensions.Logging;
using Perseverance.Proxy.Host.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Perseverance.Proxy.Host.Hubs
{
    public interface IPushNotificationHubClient
    {
        Task StateResponseAsync(PerseveranceState state);
    }
    public interface IInvokeNotificationHubClient
    {
        Task LandRequestAsync();
        Task MoveRequestAsync(Guid guid, string keyboardEventCode);
    }
    public class NotificationHub : Hub<IPushNotificationHubClient>, IInvokeNotificationHubClient
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            IMediator mediator,
            ILogger<NotificationHub> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task LandRequestAsync()
        {
            return _mediator.Publish(new LandCommand(Context.ConnectionId));
        }

        public Task MoveRequestAsync(Guid guid, string keyboardEventCode)
        {
            _logger.LogInformation("MoveRequestAsync {keyboardEventCode}", keyboardEventCode);
            return _mediator.Publish(new MoveCommand(Context.ConnectionId, guid, keyboardEventCode.ToCommand()));
        }
    }

}
