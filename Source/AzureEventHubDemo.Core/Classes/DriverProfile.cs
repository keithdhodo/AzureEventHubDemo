// <copyright file="DriverProfile.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

using System;

namespace AzureEventHubDemo.Writer.Classes
{
    [Serializable]
    public class DriverProfile
    {
        /// <summary>
        /// Driver Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Driver's Followers
        /// </summary>
        public int Followers { get; set; }

        /// <summary>
        /// Driver's Area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Driver's Bio
        /// </summary>
        public string Bio { get; set; }
    }
}
