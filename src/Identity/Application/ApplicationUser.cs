using Microsoft.AspNetCore.Identity;

namespace Aviant.Application.Identity;

public abstract class ApplicationUser : IdentityUser<Guid>
{
    public DateTime LastAccessed { get; set; }
}
