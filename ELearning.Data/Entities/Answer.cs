namespace ELearning.Data.Entities;

public class Answer : AuditableEntity
{
    public Guid AnswerId { get; set; } = Guid.CreateVersion7();
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;
}




