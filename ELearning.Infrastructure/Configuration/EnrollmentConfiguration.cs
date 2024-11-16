using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

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
