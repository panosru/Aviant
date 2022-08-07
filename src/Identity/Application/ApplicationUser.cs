namespace Aviant.Application.Identity;

using Microsoft.AspNetCore.Identity;

public abstract class ApplicationUser : IdentityUser<Guid>
{
    public DateTime LastAccessed { get; set; }
}
