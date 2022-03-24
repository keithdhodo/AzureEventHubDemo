// <copyright file="WeatherForecast.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Writer.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class WeatherForecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("barometricPressure")]
        public float BarometricPressure { get; set; }

        [JsonPropertyName("forecastId")]
        public Guid ForecastId { get; set; }

        [JsonPropertyName("humidity")]
        public float Humidity { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}