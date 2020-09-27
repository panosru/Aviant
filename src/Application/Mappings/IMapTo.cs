namespace Aviant.DDD.Application.Mappings
{
    using AutoMapper;

    public interface IMapTo<T>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T));
        }
    }
}