using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Perseverance.Proxy.Host
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {

        public static void Main(string[] args)
        {

            Log.Logger = LoggerConfiguration.CreateLogger();

            try
            {
                Log.Information("Starting web host");
                Log.Information(Environment.GetEnvironmentVariable("USE_AZURE_SIGNALR"));
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static LoggerConfiguration LoggerConfiguration => new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseSerilog().UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostEnvironment env = builderContext.HostingEnvironment;
                    Console.WriteLine($"Env: {env.EnvironmentName}");
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .SetBasePath(Directory.GetCurrentDirectory());

                    Log.Logger = LoggerConfiguration.ReadFrom.Configuration(config.Build()).CreateLogger();

                });
    }
}
