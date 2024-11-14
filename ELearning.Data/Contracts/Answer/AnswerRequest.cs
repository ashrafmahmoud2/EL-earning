namespace ELearning.Data.Contracts.Answer;

public record AnswerRequest
(
    string Text,
    bool IsCorrect,
    Guid QuestionId
);
