namespace ELearning.Data.Contracts.QuizAttempt;

public record QuizAttemptRequest(
    Guid QuizId,
    Guid StudentId,
    List<QuestionAnswerResponse> QuestionAnswerResponse
);
