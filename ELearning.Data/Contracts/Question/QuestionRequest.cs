namespace ELearning.Data.Contracts.Question;

public record QuestionRequest
(
    string Text,
    Guid QuizId
);

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Text)
            .Length(2,100)
            .NotEmpty();

        RuleFor(x => x.QuizId)
            .NotEmpty();

    }
}