using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;
using ELearning.Data.Abstractions.Const;

namespace ELearning.Infrastructure.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
       
    }
}

