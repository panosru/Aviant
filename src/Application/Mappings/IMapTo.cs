namespace Aviant.DDD.Application.Mappings
{
    #region

    using AutoMapper;

    #endregion

    public interface IMapTo<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T));
        }
    }
}