using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Entities;

namespace ELearning.Infrastructure.Configuration;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasIndex(X => new { X.Title, X.QuizId }).IsUnique(); 
        builder.HasIndex(X => new { X.QuizId, X.LessonId }).IsUnique(); 
        builder.HasIndex(x => x.Title).IsUnique();
    }

}