namespace ELearning.Data.Contracts.Section;

public class SectionRequestValidator : AbstractValidator<SectionRequest>
{
    public SectionRequestValidator()
    {


        RuleFor(x => x.Title)
            .Length(3, 100)
            .NotEmpty();

        RuleFor(x => x.Description)
            .Length(3, 1000)
            .NotEmpty();

 

        RuleFor(x => x.CourseId)
            .NotEmpty();

    }
}

