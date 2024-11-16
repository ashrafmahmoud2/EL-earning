using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasIndex(x => new { x.Title, x.DocumentPath }).IsUnique(); ;

        //builder.HasOne(d => d.Lesson)
        //    .WithMany(l => l.Documents)
        //      .HasForeignKey(x => x.LessonId);


    }

}