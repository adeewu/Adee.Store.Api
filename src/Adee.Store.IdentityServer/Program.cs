using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;

namespace Adee.Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(build =>
                {
                    build.AddJsonFile("appsettings.secrets.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom
                        .Configuration(context.Configuration)
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File("Logs/log-.log", rollingInterval: RollingInterval.Day))
                        .WriteTo.Async(c => c.Console());
                });
    }
}
