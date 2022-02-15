namespace Aviant.Core.DDD.Domain;

using Microsoft.Extensions.Configuration;

public interface IDomainConfigurationContainer
{
    public IConfiguration Configuration();

    public string GetValue(string path);
}
