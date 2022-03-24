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
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class AzureEventHubTriggerDemo
    {
        private static IConfiguration Configuration;
        private static string EventHubListenerConnectionString;
        private static IWeatherForecastService WeatherForecastService;
        private static IForecastCache ForecastCache;

        public AzureEventHubTriggerDemo(
            IConfiguration configuration,
            IWeatherForecastService weatherForecastService,
            IForecastCache forecastCache)
        {
            Configuration = configuration;

            if (weatherForecastService == null)
            {
                WeatherForecastService = weatherForecastService;
            }

            if (ForecastCache.GetCacheCount() == 0)
            {
                var weatherForecasts = WeatherForecastService.GetForecasts().Result;
                ForecastCache.PopulateCache(weatherForecasts);
            }

            if (string.IsNullOrEmpty(EventHubListenerConnectionString))
            {
                EventHubListenerConnectionString = Configuration["EventHubListenCS"];
            }
        }

        [FunctionName("AzureEventHubTriggerDemo")]
        public static async Task Run(
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

            foreach (EventData eventData in events)
            {
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
        }

        private static DriverProfile ConvertFromByteArray(EventData eventData)
        {
            var bodyString = Encoding.UTF8.GetString(eventData.Body.ToArray());
            return JsonSerializer.Deserialize<DriverProfile>(bodyString);
        }
    }
}
