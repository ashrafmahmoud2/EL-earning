using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Data.Entities;
using System.Xml.Linq;

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


