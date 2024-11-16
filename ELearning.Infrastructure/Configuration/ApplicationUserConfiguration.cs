using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(u => u.Comments)
               .WithOne(c => c.ApplicationUser)
               .HasForeignKey(c => c.ApplicationUserId)
               .HasPrincipalKey(u => u.Id); // Specify the principal key explicitly
    }
}



