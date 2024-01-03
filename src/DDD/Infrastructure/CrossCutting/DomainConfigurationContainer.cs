using Aviant.Core.DDD.Domain;
using Microsoft.Extensions.Configuration;

namespace Aviant.Infrastructure.DDD.CrossCutting;

public class DomainConfigurationContainer : IDomainConfigurationContainer
{
    private readonly IConfiguration _configuration;

    protected DomainConfigurationContainer(IConfiguration configuration) => _configuration = configuration;

    public IConfiguration Configuration() => _configuration;

    public string GetValue(string path) => _configuration[path] ?? throw new NullReferenceException();
}
