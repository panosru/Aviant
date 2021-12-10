namespace Aviant.DDD.Application.Identity;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}
