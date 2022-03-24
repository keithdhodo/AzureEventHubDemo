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
        public static MemoryCache WeatherForecasts { get; set; }

        public static List<Guid> ForecastKeys { get; set; }

        public ForecastCache()
        {
            var memoryCacheOptions = new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(60),
            };

            WeatherForecasts = new MemoryCache(memoryCacheOptions);

            ForecastKeys = new List<Guid>();
        }

        public int GetCacheCount()
        {
            return WeatherForecasts.Count;
        }

        public List<Guid> GetForecastKeys()
        {
            return ForecastKeys;
        }

        public WeatherForecast GetItemFromCache(Guid itemKey)
        {
            return WeatherForecasts.Get<WeatherForecast>(itemKey);
        }

        public bool PopulateCache(IEnumerable<WeatherForecast> forecasts)
        {
            if (ForecastKeys.Count > 0)
            {
                ForecastKeys.Clear();
            }

            foreach(var forecast in forecasts)
            {
                WeatherForecasts.TryGetValue(forecast.ForecastId, out var foundForecast);

                if (foundForecast == null)
                {
                    WeatherForecasts.Set(forecast.ForecastId, forecast, DateTimeOffset.UtcNow.AddMinutes(1));
                    ForecastKeys.Add(forecast.ForecastId);
                }
            }

            return true;
        }
    }
}
