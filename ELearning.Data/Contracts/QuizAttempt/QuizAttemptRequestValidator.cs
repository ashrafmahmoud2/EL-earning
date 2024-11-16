namespace ELearning.Data.Contracts.QuizAttempt;

public class QuizAttemptRequestValidator : AbstractValidator<QuizAttemptRequest>
{
    public QuizAttemptRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty();
        RuleFor(x => x.StudentId)
          .NotEmpty();
        RuleFor(x => x.QuestionAnswerResponse)
                .NotEmpty();


    }
}