namespace ELearning.Data.Contracts.QuizAttempt;

public record QuestionAnswerResponse(
    Guid QuestionId,
    List<Guid> SelectedAnswersIds
);
