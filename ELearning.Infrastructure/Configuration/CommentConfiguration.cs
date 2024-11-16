using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasIndex(x => new { x.ApplicationUserId, x.CommentText, x.LessonId })
                      .IsUnique();

        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId);


        builder.HasOne<Lesson>()
           .WithMany()
           .HasForeignKey(x => x.ApplicationUserId);


        builder.Property(x => x.Title).HasMaxLength(255);
        builder.Property(x => x.CommentText).HasMaxLength(1000);


    }

}