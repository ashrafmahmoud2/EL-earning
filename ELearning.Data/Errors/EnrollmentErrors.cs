using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record EnrollmentErrors
{
    public static readonly Error EnrollmentNotFound =
        new("Enrollment.EnrollmentNotFound", "The enrollment was not found.", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedEnrollment =
        new("Enrollment.DuplicatedEnrollment", "An enrollment with the same name already exists.", StatusCodes.Status409Conflict);

    public static readonly Error EnrollmentAddFailed =
        new("Enrollment.AddFailed", "Failed to add the enrollment to the repository.", StatusCodes.Status500InternalServerError);

    public static readonly Error CanceledEnrollment =
        new("Enrollment.CanceledEnrollment", "Cannot perform actions on a canceled enrollment.", StatusCodes.Status409Conflict);

    public static readonly Error DuplicateEnrollmentData =
        new("Enrollment.DuplicateEnrollmentData", "The student is already enrolled in this course with the same data.", StatusCodes.Status409Conflict);

    public static readonly Error RefundedEnrollment =
        new(" Enrollment.Refunded Enrollment", "Cannot perform any action on a refunded Enrollment.", StatusCodes.Status409Conflict);

}
