// <copyright file="IWeatherForecastService.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Core.Interfaces
{
	using AzureEventHubDemo.Writer.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetForecasts();
    }
}
