namespace ELearning.Data.Contracts.Answer;

public class AnswerRequestValidator : AbstractValidator<AnswerRequest>
{
    public AnswerRequestValidator()
    {
        RuleFor(x => x.Text)
            .Length(3, 200)
            .NotEmpty();

        RuleFor(x => x.IsCorrect)
            .NotEmpty();

        RuleFor(x => x.QuestionId)
            .NotEmpty();

    }
}