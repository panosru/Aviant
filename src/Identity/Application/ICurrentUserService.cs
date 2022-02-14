namespace Aviant.Application.Identity;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}
