namespace ELearning.Data.Entities;

public class Comment : AuditableEntity
{
    public Guid CommentId { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}

