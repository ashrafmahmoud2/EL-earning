namespace ELearning.Data.Entities;

public class Comment 
{
    public Guid CommentId { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Guid LessonId { get; set; }
    public DateTime CommentedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedCommentedAt { get; set; }
    public string CommentedByUserId { get; set; }=string.Empty;
    public bool IsEdited { get; set; } = false;

    public ApplicationUser ApplicationUser { get; set; } = default!;
    public Lesson Lesson { get; set; } = default!;
}

