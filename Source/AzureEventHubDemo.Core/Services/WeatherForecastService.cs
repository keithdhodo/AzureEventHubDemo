// <copyright file="WeatherForecastService.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Core.Services
{
    using AzureEventHubDemo.Core.Interfaces;
    using AzureEventHubDemo.Writer.Models;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class WeatherForecastService : IWeatherForecastService
    {
        private HttpClient client;

        private readonly IConfiguration Configuration;

        public WeatherForecastService(IConfiguration configuration)
        {
            Configuration = configuration;
            client = new HttpClient()
            {
                BaseAddress = new Uri(Configuration["EventHubDemoAPIBaseUrl"]),
            };
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecasts()
        {
            var forecasts = new List<WeatherForecast>();
            var response = await client.GetAsync(requestUri: "WeatherForecast/");
            if (response.IsSuccessStatusCode)
            {
                forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(await response.Content.ReadAsStringAsync());
            }

            return forecasts;
        }
    }
}
