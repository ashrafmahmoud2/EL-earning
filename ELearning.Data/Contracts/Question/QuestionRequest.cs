namespace ELearning.Data.Contracts.Question;

public record QuestionRequest
(
    string Text,
    Guid QuizId
);