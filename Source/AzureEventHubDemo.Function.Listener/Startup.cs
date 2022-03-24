// <copyright file="Startup.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureEventHubDemo.Function.Listener.Startup))]

namespace AzureEventHubDemo.Function.Listener
{
    using AzureEventHubDemo.Core.Interfaces;
    using AzureEventHubDemo.Core.Models;
    using AzureEventHubDemo.Core.Services;
    using AzureEventHubDemo.Core.Utilities;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;

    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<IWeatherForecastService>(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                return new WeatherForecastService(config);
            });

            builder.Services.AddSingleton<IForecastCache>(sp =>
            {
                var forecastCache = new ForecastCache();
                return forecastCache;
            });

            // builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddKeyVault();
        }
    }
}