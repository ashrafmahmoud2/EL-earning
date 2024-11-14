using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasIndex(c => c.Title).IsUnique();


        builder.HasOne(c => c.Instructor)
               .WithMany(i => i.Courses)
               .HasForeignKey(c => c.InstructorId);


        builder.HasOne(c => c.Category)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.CategoryId);
    }

}

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.EnrollmentId);

        builder.HasOne(c => c.course)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.CourseId);




        builder.HasOne(c => c.student)
            .WithMany(x => x.Enrollments)
            .HasForeignKey(x => x.StudentId);
            
    }

}
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

