// <copyright file="DeviceReading.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace AzureEventHubDemo.Core.Models
{
    using System.Text.Json.Serialization;

    public class DeviceReading
    {
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("deviceTemperature")]
        public decimal DeviceTemperature { get; set; }

        [JsonPropertyName("damageLevel")]
        public string DamageLevel { get; set; }

        [JsonPropertyName("deviceAgeInDays")]
        public int DeviceAgeInDays { get; set; }
    }
}
