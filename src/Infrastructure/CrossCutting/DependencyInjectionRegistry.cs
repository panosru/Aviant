namespace Aviant.DDD.Infrastructure.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using Core.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;

    public static class DependencyInjectionRegistry
    {
        public static IWebHostEnvironment? CurrentEnvironment { get; set; }
        
        public static IConfigurationBuilder? ConfigurationBuilder { get; set; }

        public static IConfiguration DefaultConfiguration { get; set; }

        public static IConfiguration GetDomainConfiguration(string domain)
        {
            if (CurrentEnvironment is null || ConfigurationBuilder is null)
                throw new NullReferenceException(
                    typeof(DependencyInjectionRegistry).FullName);

            var configurationBuilder = new ConfigurationBuilder();

            ((List<IConfigurationSource>)configurationBuilder.Sources).AddRange(ConfigurationBuilder.Sources);
            configurationBuilder.Sources.Add(GetSource(domain));
            configurationBuilder.Sources.Add(GetSource(domain, CurrentEnvironment.EnvironmentName));

            return configurationBuilder.Build();
        }
        
        private static JsonConfigurationSource GetSource(string domain, string? environment = null)
        {
            return new JsonConfigurationSource
            {
                Path = environment is null 
                    ? $"appsettings.{domain}.json" 
                    : $"appsettings.{domain}.{environment}.json",
                ReloadOnChange = true,
                Optional       = true
            };
        }
    }
}