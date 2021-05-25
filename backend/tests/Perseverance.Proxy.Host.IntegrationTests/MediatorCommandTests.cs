using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Perseverance.Proxy.Host.Models;
using Perseverance.Proxy.Host.Services;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Perseverance.Proxy.Host.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class MediatorCommandTests
    {

        [Test]
        public async Task ServerShouldReactCommands()
        {
            var configuration = new ConfigurationBuilder().Build();

            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseSerilog().UseStartup<Startup>();
                    webHost.UseConfiguration(configuration);
                    webHost.UseTestServer();
                    webHost.ConfigureTestServices(services =>
                    {
                        services.AddLogging(x => x.AddConsole());
                    });
                });

            var host = await hostBuilder.StartAsync();
            var testServer = host.GetTestServer();

            var mediator = testServer.Services.GetRequiredService<IMediator>();

            var connectionId = Guid.NewGuid().ToString();
            var state = testServer.Services.GetRequiredService<IPerseveranceStateService>();
            await mediator.Publish(new LandCommand(connectionId, new LandOptions()));
            var guid = state.Cache.Keys.First();
            await mediator.Publish(new MoveCommand(connectionId, guid, "F"));

            state.Cache[guid].Should().NotBeNull();

        }
    }
}