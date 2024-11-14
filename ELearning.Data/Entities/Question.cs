namespace ELearning.Data.Entities;

public class Question : AuditableEntity
{
    public Guid QuestionId { get; set; } = Guid.CreateVersion7();
    public string Text { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = default!;
   public ICollection<Answer> Answers { get; set; } = [];
}



