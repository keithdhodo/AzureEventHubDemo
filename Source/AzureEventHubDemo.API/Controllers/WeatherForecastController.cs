// <copyright file="WeatherForecastController.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.API.Controllers
{
    using AzureEventHubDemo.Writer.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetForecast()
        {
            return Enumerable.Range(1, 1000).Select(index => new WeatherForecast
            {
                ForecastId = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(index),
                BarometricPressure = Faker.RandomNumber.Next(),
                Humidity = Faker.RandomNumber.Next(),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            })
            .ToArray();
        }
    }
}