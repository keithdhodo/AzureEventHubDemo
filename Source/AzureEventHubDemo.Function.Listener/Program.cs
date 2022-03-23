// <copyright file="Program.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Function.Listener
{
    using AzureEventHubDemo.Core.Interfaces;
    using AzureEventHubDemo.Core.Models;
    using AzureEventHubDemo.Core.Services;
    using AzureEventHubDemo.Core.Utilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.IO;

    public class Program
    {
        public static void Main()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var host = new HostBuilder()
                .ConfigureAppConfiguration(config => config
                    .SetBasePath(currentDirectory)
                    .AddJsonFile("local.appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables())
                    // .AddKeyVault())
                // .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IWeatherForecastService>(sp =>
                    {
                        var config = sp.GetService<IConfiguration>();
                        return new WeatherForecastService(config);
                    });
                    services.AddSingleton<IForecastCache>(sp =>
                    {
                        var weatherForecastService = sp.GetService<IWeatherForecastService>();
                        var weatherForecasts = weatherForecastService.GetForecasts().Result;
                        var forecastCache = new ForecastCache();
                        forecastCache.PopulateCache(weatherForecasts);
                        return forecastCache;
                    });
                    //services.AddTransient<IServiceWrapper>(sp =>
                    //{
                    //    var config = sp.GetService<IConfiguration>();
                    //    return new ServiceWrapper(new SqlWrapper(config[WebConstants.KEY_VAULT_SQL_CONNECTIONSTRING]));
                    //});
                    //services.AddSingleton<IQueueClient>(sp =>
                    //{
                    //    var config = sp.GetService<IConfiguration>();
                    //    return new QueueClient(
                    //        config[ExtraLifeInboundETL.Constants.KeyVaultConstants.ServiceBusConnectionString],
                    //        ExtraLifeInboundETL.Constants.ServiceBusConstants.ParticipantsQueue);
                    //});
                })
                .Build();

            host.Run();
        }
    }
}
