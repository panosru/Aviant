namespace Aviant.Application.Mappings;

using AutoMapper;

public interface IMapFrom<T>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}
