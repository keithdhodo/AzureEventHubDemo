// <copyright file="Program.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

namespace ExtraLife.Web.Admin
{
    using AzureEventHubDemo.API;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}