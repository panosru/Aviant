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
        private static IConfiguration? _configuration;

        public static IWebHostEnvironment? CurrentEnvironment { get; set; }

        public static IConfigurationBuilder? ConfigurationBuilder { get; set; }

        public static IConfiguration DefaultConfiguration =>
            _configuration
         ?? ServiceLocator.ServiceContainer.GetRequiredService<IConfiguration>(
                typeof(IConfiguration));

        public static IConfiguration SetConfiguration(IConfiguration configuration)
        {
            return _configuration = configuration;
        }

        public static IConfiguration GetDomainConfiguration(string domain)
        {
            if (CurrentEnvironment is null
             || ConfigurationBuilder is null)
                throw new NullReferenceException(
                    typeof(DependencyInjectionRegistry).FullName);

            var configurationBuilder = new ConfigurationBuilder();

            ((List<IConfigurationSource>) configurationBuilder.Sources).AddRange(ConfigurationBuilder.Sources);
            configurationBuilder.Sources.Add(GetSource(domain));
            configurationBuilder.Sources.Add(GetSource(domain, CurrentEnvironment.EnvironmentName));

            return configurationBuilder.Build();
        }

        private static JsonConfigurationSource GetSource(string domain, string? environment = null) =>
            new JsonConfigurationSource
            {
                Path = environment is null
                    ? $"appsettings.{domain}.json"
                    : $"appsettings.{domain}.{environment}.json",
                ReloadOnChange = true,
                Optional       = true
            };
    }
}