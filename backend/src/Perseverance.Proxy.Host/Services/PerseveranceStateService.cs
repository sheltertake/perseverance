using Perseverance.Proxy.Host.Models;
using System;
using System.Collections.Concurrent;

namespace Perseverance.Proxy.Host.Services
{
    public interface IPerseveranceStateService
    {
        ConcurrentDictionary<Guid, PerseveranceState> Cache { get; set; }
    }

    public class PerseveranceStateService : IPerseveranceStateService
    {
        public ConcurrentDictionary<Guid, PerseveranceState> Cache { get; set; } = new();
    }
}