namespace ELearning.Data.Contracts.Students;

public class StudentRequestValidator : AbstractValidator<StudentRequest>
{
    public StudentRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(2, 50)
            .NotEmpty();

        RuleFor(x => x.LastName)
           .Length(2, 50)
           .NotEmpty();
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
    }
}
