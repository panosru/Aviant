namespace Aviant.DDD.Core.Reflection;

using System.Reflection;

/// <summary>
/// This interface is used to get assemblies in the application.
/// It may not return all assemblies, but those are related with modules.
/// </summary>
public interface IAssemblyFinder
{
    /// <summary>
    /// Gets all assemblies.
    /// </summary>
    /// <returns>List of assemblies</returns>
    IEnumerable<Assembly> GetAllAssemblies();
}
