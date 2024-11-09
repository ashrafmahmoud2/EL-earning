namespace ELearning.Data.Contracts.Instrctors;

public class InstructorRequestValidator : AbstractValidator<InstructorRequest>
{
    public InstructorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Expertise)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Biography)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}




