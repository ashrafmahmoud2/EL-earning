namespace ELearning.Data.Contracts.Quiz;

public record QuizRequest
(
    string Title,
    string? Description,
    Guid LessonId
);
