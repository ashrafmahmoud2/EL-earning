namespace ELearning.Data.Contracts.Enrollment;

public class EnrollmentAddRequestValidator : AbstractValidator<EnrollmentAddRequest>
{
    public EnrollmentAddRequestValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student ID is required.");
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course ID is required.");
    }
}
