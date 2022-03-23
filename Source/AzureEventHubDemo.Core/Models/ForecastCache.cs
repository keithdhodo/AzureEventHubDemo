// <copyright file="ForecastCache.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Core.Models
{
    using AzureEventHubDemo.Core.Interfaces;
    using AzureEventHubDemo.Writer.Models;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;

    public class ForecastCache : IForecastCache
    {
        static MemoryCache WeatherForecasts { get; set; }

        public ForecastCache()
        {
            var memoryCacheOptions = new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(60),
            };

            WeatherForecasts = new MemoryCache(memoryCacheOptions);
        }

        public int GetCacheCount()
        {
            return WeatherForecasts.Count;
        }

        public bool PopulateCache(IEnumerable<WeatherForecast> forecasts)
        {
            foreach(var forecast in forecasts)
            {
                WeatherForecasts.TryGetValue(forecast.ForecastId, out var foundForecast);

                if (foundForecast == null)
                {
                    WeatherForecasts.Set(forecast.ForecastId, forecast, DateTimeOffset.UtcNow.AddMinutes(1));
                }
            }

            return true;
        }
    }
}
