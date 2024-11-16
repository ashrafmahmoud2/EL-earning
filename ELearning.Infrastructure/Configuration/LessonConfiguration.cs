using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {

        //builder.HasOne<Section>()
        //    .WithMany(s => s.Lessons)
        //    .HasForeignKey(x => x.SectionId);



    }

}