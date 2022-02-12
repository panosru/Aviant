namespace Aviant.Foundation.Application.Identity;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}
