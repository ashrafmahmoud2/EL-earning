namespace ELearning.Data.Entities;

public class QuizAttempt : AuditableEntity
{
    public Guid QuizAttemptId { get; set; } = Guid.CreateVersion7();
    public bool HasPassed { get; set; }
    public Guid QuizId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime AttemptedOn { get; set; } = DateTime.UtcNow;
    public double ScorePercentage { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswersCount { get; set; }
    public int IncorrectAnswersCount { get; set; }
    public int NotAnswersQuestionsCount { get; set; }
    public bool IsActive { get; set; } = true;


    public Student student { get; set; } = default!;
    public Quiz Quiz { get; set; } = default!;


    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<Answer> SelectedAnswers { get; set; } = [];


}




