using System.Reflection;
using AutoMapper;

namespace Aviant.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile(Assembly assembly)
    {
        ApplyMappingsFromAssembly(assembly);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        List<Type> types = assembly
           .GetExportedTypes()
           .Where(
                t => t.GetInterfaces()
                   .ToList()
                   .Exists(
                        i =>
                            i.IsGenericType
                         && (i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                          || i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
           .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type
                                .GetMethod("Mapping", BindingFlags.Instance | BindingFlags.NonPublic)
                          ?? (type.GetInterfaces()
                                .ToList()
                                .Exists(
                                     i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                                 ? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping")
                                 : type.GetInterface("IMapTo`1")?.GetMethod("Mapping"));

            methodInfo?.Invoke(instance, new object?[] { this });
        }
    }
}
