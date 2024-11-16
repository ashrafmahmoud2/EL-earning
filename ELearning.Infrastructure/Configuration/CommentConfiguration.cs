using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.Lesson) // Each Comment belongs to one Lesson
            .WithMany(l => l.Comments) // A Lesson can have many Comments
            .HasForeignKey(c => c.LessonId) // Foreign key in Comment
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for safety

        // Configure relationship with ApplicationUser (CommentedByUserId)
        builder.HasOne(c => c.ApplicationUser) // Each Comment is made by one User
            .WithMany(u => u.Comments) // A User can have many Comments
            .HasForeignKey(c => c.CommentedByUserId) // Foreign key in Comment
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for safety

        // Set max lengths for properties
        builder.Property(c => c.Title).HasMaxLength(255);
        builder.Property(c => c.CommentText).HasMaxLength(1000);

        // Additional configurations (if needed)
        builder.Property(c => c.CommentedAt).IsRequired();
        builder.Property(c => c.IsActive).HasDefaultValue(true);

    }

}