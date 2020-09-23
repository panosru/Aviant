namespace Aviant.DDD.Infrastructure.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Core.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using NetEscapades.Configuration.Yaml;

    public static class DependencyInjectionRegistry
    {
        private static IConfiguration? _configuration;

        private static IConfigurationBuilder? _configurationWithDomains;

        public static IWebHostEnvironment? CurrentEnvironment { get; set; }

        public static IConfigurationBuilder? ConfigurationBuilder { get; set; }

        public static IConfiguration DefaultConfiguration =>
            _configuration
         ?? ServiceLocator.ServiceContainer.GetRequiredService<IConfiguration>(
                typeof(IConfiguration));

        public static IConfiguration ConfigurationWithDomains =>
            _configurationWithDomains?.Build()
         ?? throw new NullReferenceException(typeof(DependencyInjectionRegistry).FullName);

        public static IConfiguration SetConfiguration(IConfigurationBuilder configuration)
        {
            _configurationWithDomains = configuration;
            
            return _configuration = configuration.Build();
        }

        public static IConfiguration GetDomainConfiguration(string domain)
        {
            if (CurrentEnvironment is null
             || ConfigurationBuilder is null)
                throw new NullReferenceException(
                    typeof(DependencyInjectionRegistry).FullName);

            var configurationBuilder = new ConfigurationBuilder();

            ((List<IConfigurationSource>) configurationBuilder.Sources).AddRange(ConfigurationBuilder.Sources);

            // JSON does not override any other format
            LoadConfiguration(
                configurationBuilder, 
                domain, 
                CurrentEnvironment.EnvironmentName,
                ConfigurationFormat.JSON);
            
            // YML overrides JSON
            LoadConfiguration(
                configurationBuilder, 
                domain, 
                CurrentEnvironment.EnvironmentName,
                ConfigurationFormat.YML);
            
            // YAML overrides both YML and JSON
            LoadConfiguration(
                configurationBuilder, 
                domain, 
                CurrentEnvironment.EnvironmentName,
                ConfigurationFormat.YAML);
            
            return configurationBuilder.Build();
        }

        private static void LoadConfiguration(
            IConfigurationBuilder configurationBuilder,
            string domain,
            string environment,
            ConfigurationFormat format)
        {
            string[] configFiles = 
            {
                $"appsettings.{domain}.{Enum.GetName(typeof(ConfigurationFormat),               format)?.ToLower()}",
                $"appsettings.{domain}.{environment}.{Enum.GetName(typeof(ConfigurationFormat), format)?.ToLower()}"
            };

            foreach (var configFile in configFiles)
            {
                if (ConfigurationExists(configFile))
                {
                    switch (format)
                    {
                        case ConfigurationFormat.JSON:
                            _configurationWithDomains?.Sources.Add(GetSource<JsonConfigurationSource>(configFile));
                            configurationBuilder.Sources.Add(GetSource<JsonConfigurationSource>(configFile));
                            break;
                        
                        case ConfigurationFormat.YAML:
                        case ConfigurationFormat.YML:
                            _configurationWithDomains?.Sources.Add(GetSource<YamlConfigurationSource>(configFile));
                            configurationBuilder.Sources.Add(GetSource<YamlConfigurationSource>(configFile));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(format), format, null);
                    }
                }
            }
        }

        private static bool ConfigurationExists(string configFileName)
        {
            return File.Exists(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) 
                 ?? throw new NullReferenceException(Assembly.GetCallingAssembly().FullName),
                    configFileName));
        }

        private static TConfigurationSource GetSource<TConfigurationSource>(string configFileName)
            where TConfigurationSource : FileConfigurationSource, new()
        {
            return new TConfigurationSource
            {
                Path           = configFileName,
                ReloadOnChange = true,
                Optional       = true
            };
        }
    }
}