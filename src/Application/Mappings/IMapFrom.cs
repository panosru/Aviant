namespace Aviant.DDD.Application.Mappings
{
    #region

    using AutoMapper;

    #endregion

    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}