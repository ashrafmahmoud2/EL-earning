namespace ELearning.Data.Contracts.Lesson;

public class LessonRequestValidator : AbstractValidator<LessonRequest>
{
    public LessonRequestValidator()
    {
        RuleFor(x => x.Title)
            .Length(2,100)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .Length(2,100)
            .NotEmpty(); 
        
        
        RuleFor(x => x.SectionId)
            .NotEmpty();

    }
}
