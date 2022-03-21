// <copyright file="DriverProfile.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace AzureEventHubDemo.Writer.Models
{
    [Serializable]
    public class DriverProfile
    {
        /// <summary>
        /// Driver Full Name
        /// </summary>
        /// 
        [JsonPropertyName("driverId")]
        public Guid DriverId { get; set; }

        /// <summary>
        /// Driver Full Name
        /// </summary>
        /// 
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Driver's Followers
        /// </summary>
        /// 
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Driver's Area
        /// </summary>
        /// 
        [JsonPropertyName("area")]
        public string Area { get; set; }

        /// <summary>
        /// Driver's Bio
        /// </summary>
        /// 
        [JsonPropertyName("bio")]
        public string Bio { get; set; }
    }
}
