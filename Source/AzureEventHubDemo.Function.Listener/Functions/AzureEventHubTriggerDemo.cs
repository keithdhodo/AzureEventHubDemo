// <copyright file="AzureEventHubTriggerDemo.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Function.Listener
{
    using Azure.Messaging.EventHubs;
    using AzureEventHubDemo.Core.Interfaces;
    using AzureEventHubDemo.Writer.Models;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class AzureEventHubTriggerDemo
    {
        private static IConfiguration Configuration;
        private static IWeatherForecastService WeatherForecastService;
        private static IForecastCache ForecastCache;

        private static IList<Guid> ForecastKeys;

        private static string EventHubListenerConnectionString;
        private static Random random;

        private static int totalEventCount;
        private static Stopwatch stopwatch;

        public AzureEventHubTriggerDemo(
            IConfiguration configuration,
            IWeatherForecastService weatherForecastService,
            IForecastCache forecastCache)
        {
            if (stopwatch == null)
            {
                stopwatch = new Stopwatch();
            }

            if (Configuration == null)
            {
                Configuration = configuration;
            }

            if (WeatherForecastService == null)
            {
                WeatherForecastService = weatherForecastService;
            }

            if (ForecastCache == null)
            {
                if (ForecastCache == null)
                {
                    ForecastCache = forecastCache;
                }
            }

            if (ForecastCache.GetCacheCount() == 0)
            {
                GetForecastsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }

            if (random == null)
            {
                random = new Random();
            }

            if (string.IsNullOrEmpty(EventHubListenerConnectionString))
            {
                EventHubListenerConnectionString = Configuration["EventHubListenCS"];
            }
        }

        [FunctionName("AzureEventHubTriggerDemo")]
        public async Task Run(
                [EventHubTrigger(
            	eventHubName: "eventhubkhododemo",
            	Connection = "EventHubListenConnectionString")]
                EventData[] events,
                //DateTime enqueuedTimeUtc,
                //long sequenceNumber,
                //string offset,
                ILogger log)
        {
            var exceptions = new List<Exception>();
            stopwatch.Start();

            var randomIndex = random.Next(0, ForecastKeys.Count() - 1);
            var randomForecast = ForecastCache.GetItemFromCache(ForecastKeys[randomIndex]);

            if (randomForecast == null)
            {
                await GetForecastsAsync();
                randomForecast = ForecastCache.GetItemFromCache(ForecastKeys[randomIndex]);
            }

            log.LogInformation($"Forecast Id: {randomForecast.ForecastId}");
            log.LogInformation($"Forecast Date: {randomForecast.Date}");

            foreach (EventData eventData in events)
            {
                totalEventCount++;

                try
                {
                    var driverProfile = ConvertFromByteArray(eventData);

                    //log.LogInformation($"Event: {Encoding.UTF8.GetString(eventData.Body)}");
                    log.LogInformation($"Driver Id: {driverProfile.DriverId}");
                    log.LogInformation($"Name: {driverProfile.Name}");
                    log.LogInformation($"Area: {driverProfile.Area}");
                    log.LogInformation($"Bio: {driverProfile.Bio}");
                    log.LogInformation($"Follower Count: {driverProfile.Followers}");
                    log.LogInformation($"Count: {driverProfile.Count}");

                    // log stats
                    log.LogInformation($"Total Events: {totalEventCount}");
                    log.LogInformation($"Total Duration (Seconds): {stopwatch.Elapsed.TotalSeconds}");

                    // Metadata accessed by binding to EventData
                    // log.LogInformation($"EnqueuedTimeUtc={eventData.SystemProperties.EnqueuedTimeUtc}");
                    // log.LogInformation($"SequenceNumber={eventData.SystemProperties.SequenceNumber}");
                    // log.LogInformation($"Offset={eventData.SystemProperties.Offset}");

                    // Metadata accessed by using binding expressions in method parameters
                    //log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
                    //log.LogInformation($"SequenceNumber={sequenceNumber}");
                    //log.LogInformation($"Offset={offset}");

                    // string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    // Replace these two lines with your processing logic.
                    // log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.
            if (exceptions.Count > 1)
            {
                throw new AggregateException(exceptions);
            }

            if (exceptions.Count == 1)
            {
                throw exceptions.Single();
            }

            stopwatch.Stop();
        }

        private static async Task GetForecastsAsync()
        {
            ForecastKeys = new List<Guid>();

            var weatherForecasts = await WeatherForecastService.GetForecasts();
            ForecastCache.PopulateCache(weatherForecasts);
            ForecastKeys = ForecastCache.GetForecastKeys();
        }

        private static DriverProfile ConvertFromByteArray(EventData eventData)
        {
            var bodyString = Encoding.UTF8.GetString(eventData.Body.ToArray());
            return JsonSerializer.Deserialize<DriverProfile>(bodyString);
        }
    }
}
