using System.Reflection;
using Castle.Core.Logging;
using Aviant.Core.Collections.Extensions;

namespace Aviant.Core.Reflection;

public class TypeFinder : ITypeFinder
{
    private readonly IAssemblyFinder _assemblyFinder;

    private readonly object _syncObj = new();

    private Type[]? _types;

    public TypeFinder(IAssemblyFinder assemblyFinder)
    {
        _assemblyFinder = assemblyFinder;
        Logger          = NullLogger.Instance;
    }

    public ILogger Logger { get; set; }

    #region ITypeFinder Members

    public Type[] Find(Func<Type, bool> predicate) => (GetAllTypes() ?? Type.EmptyTypes).Where(predicate).ToArray();

    public Type[] FindAll() => (GetAllTypes() ?? Type.EmptyTypes).ToArray();

    #endregion

    private Type[]? GetAllTypes()
    {
        if (_types is not null)
            return _types;

        lock (_syncObj)
        {
            _types ??= CreateTypeList().ToArray();
        }

        return _types;
    }

    private List<Type> CreateTypeList()
    {
        var allTypes = new List<Type>();

        IEnumerable<Assembly> assemblies = _assemblyFinder.GetAllAssemblies().Distinct();

        foreach (var assembly in assemblies)
            try
            {
                Type?[] typesInThisAssembly;

                try
                {
                    typesInThisAssembly = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInThisAssembly = ex.Types;
                }

                if (typesInThisAssembly.IsNullOrEmpty())
                    continue;

                allTypes.AddRange(typesInThisAssembly.Where(type => type is not null)!);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }

        return allTypes;
    }
}
