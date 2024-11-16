namespace ELearning.Data.Contracts.Quiz;

public class QuizRequestValidator : AbstractValidator<QuizRequest>
{
    public QuizRequestValidator()
    {
        RuleFor(x => x.Title)
            .Length(3,100)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .Length(3,100)
            .NotEmpty();

        RuleFor(X => X.LessonId)
            .NotEmpty();

    }
}