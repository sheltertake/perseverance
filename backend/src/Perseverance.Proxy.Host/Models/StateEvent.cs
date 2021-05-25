using MediatR;

namespace Perseverance.Proxy.Host.Models
{
    public class StateEvent : INotification
    {
        public string ConnectionId { get; }
        public PerseveranceState State { get; }

        public StateEvent(string connectionId, PerseveranceState state)
        {
            ConnectionId = connectionId;
            State = state;
        }
    }
}