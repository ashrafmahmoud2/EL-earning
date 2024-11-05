using Microsoft.AspNetCore.Identity;

namespace ELearning.Data.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
        Id = Guid.CreateVersion7().ToString();
    }

    public bool IsDefault { get; set; }//IsDefault it's default role give to any user with specific permission like member in our application
    public bool IsDeleted { get; set; }
}
