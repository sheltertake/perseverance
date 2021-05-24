using MediatR;

namespace Perseverance.Proxy.Host.Models
{
    public class LandCommand : INotification
    {
        public string ConnectionId { get; }
        
        public LandCommand(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}