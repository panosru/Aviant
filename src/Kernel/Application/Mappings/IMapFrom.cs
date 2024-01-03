using AutoMapper;

namespace Aviant.Application.Mappings;

public interface IMapFrom<T>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}
