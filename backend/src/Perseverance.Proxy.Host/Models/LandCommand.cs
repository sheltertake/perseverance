using MediatR;

namespace Perseverance.Proxy.Host.Models
{
    public class LandCommand : INotification
    {
        public string ConnectionId { get; }
        public LandOptions Options { get; }

        public LandCommand(string connectionId, LandOptions options)
        {
            ConnectionId = connectionId;
            Options = options;
        }
    }
}