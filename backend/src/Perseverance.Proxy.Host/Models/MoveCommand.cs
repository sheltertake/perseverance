using System;
using MediatR;

namespace Perseverance.Proxy.Host.Models
{
    public class MoveCommand : INotification
    {
        public string ConnectionId { get; }
        public Guid Guid { get; }
        public string Command { get;  }
        public MoveCommand(string connectionId, Guid guid, string command)
        {
            ConnectionId = connectionId;
            Guid = guid;
            Command = command;
        }
    }
}