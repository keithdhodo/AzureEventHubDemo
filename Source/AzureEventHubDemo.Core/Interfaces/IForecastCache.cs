// <copyright file="IForecastCache.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Core.Interfaces
{
	using AzureEventHubDemo.Writer.Models;
	using System;
	using System.Collections.Generic;

	public interface IForecastCache
    {
        int GetCacheCount();

        List<Guid> GetForecastKeys();

        WeatherForecast GetItemFromCache(Guid itemKey);

        bool PopulateCache(IEnumerable<WeatherForecast> forecasts);
    }
}
