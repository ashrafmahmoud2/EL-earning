namespace ELearning.Core.DTOs;

public class CommentDto
{

    public Guid CommentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid LessonId { get; set; }
    public DateTime CommentedAt { get; set; }
    public DateTime? UpdatedCommentedAt { get; set; }
    public string CommentedByUserId { get; set; } = string.Empty;
    public string CommentedBy { get; set; } = string.Empty;
    public bool IsEdited { get; set; }
}
