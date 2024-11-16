using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        //builder.HasIndex(x => new { x.StudentId, x.User }).IsUnique();

        //builder.HasOne(x => x.User) // Each Student has one ApplicationUser
        //     .WithMany(u => u.Students) // Each ApplicationUser can have many Students
        //     .HasForeignKey(x => x.UserId) // Specify the foreign key in Student
        //     .OnDelete(DeleteBehavior.Restrict); // Avoid cascading delete

    }

}