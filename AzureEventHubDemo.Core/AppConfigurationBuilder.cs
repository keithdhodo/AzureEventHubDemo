// <copyright file="AppConfigurationBuilder.cs" company="Cascadian Gamers">
// Copyright (c) Cascadian Gamers. All rights reserved.
// </copyright>

namespace ExtraLife.Core
{
    using Azure.Identity;
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// App Configuration Builder extension to register key vault and other custom providers.
    /// </summary>
    public static class AppConfigurationBuilder
    {
        /// <summary>
        /// Configures the Key Vault service as extension.
        /// </summary>
        /// <param name="configurationBuilder">Configuration builder.</param>
        /// <returns>returns the configuration builder.</returns>
        public static IConfigurationBuilder AddKeyVault(this IConfigurationBuilder configurationBuilder)
        {
            var configuration = configurationBuilder.Build();
            string keyVaultUri = configuration["KeyVaultURI"];

            if (string.IsNullOrEmpty(keyVaultUri) == false)
            {
                configurationBuilder.AddAzureKeyVault(
                    new Uri(keyVaultUri),
                    new DefaultAzureCredential());
            }

            return configurationBuilder;
        }
    }
}