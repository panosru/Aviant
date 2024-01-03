using Microsoft.Extensions.Configuration;

namespace Aviant.Core.DDD.Domain;

public interface IDomainConfigurationContainer
{
    public IConfiguration Configuration();

    public string GetValue(string path);
}
