using ELearning.Data.Abstractions.Const;

namespace ELearning.Data.Contracts.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
          .NotEmpty()
          .Length(3, max: 100);

        RuleFor(x => x.LastName)
        .NotEmpty()
        .Length(3, max: 100);

        RuleFor(x => x.Password)
      .NotEmpty()
       .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");


    }
}
