namespace ELearning.Data.Contracts.Enrollment;

public class EnrollmentUpdateRequestValidator : AbstractValidator<EnrollmentUpdateRequest>
{
    public EnrollmentUpdateRequestValidator()
    {
      //  RuleFor(x => x).NotEmpty().WithMessage("Enrollment ID is required.");
      //  RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Invalid enrollment status.");
    }
}
