using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Perseverance.Proxy.Host.Models;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Perseverance.Proxy.Host.Services;

namespace Perseverance.Proxy.Host.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class MediatorCommandTests
    {

        [Test]
        public async Task ServerShouldReactCommands()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "development");

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
            var state = testServer.Services.GetRequiredService<IPerseveranceStateService>();

            var connectionId = Guid.NewGuid().ToString();
            var newGuid = Guid.NewGuid();
            await mediator.Publish(new LandCommand(connectionId, new LandOptions()
            {
                Guid = newGuid,
                X = 0,
                Y = 0,
                H = 5,
                W = 5,
                O = 5
            }));
            await mediator.Publish(new MoveCommand(connectionId, newGuid, "F"));

            var cache = await state.GetStateAsync(newGuid, CancellationToken.None);
            cache.Should().NotBeNull();
            cache.W.Should().Be(4);
            cache.H.Should().Be(4);
            cache.Obstacles.Should().HaveCount(5);
        }
    }
}