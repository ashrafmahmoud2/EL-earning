using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record InstructorsErrors
{
    public static readonly Error InstructorNotFound =
        new("Instructor.InstructorNotFound", "Instructor is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Instructor.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedInstructor =
        new("Instructor.DuplicatedInstructor", "Another Instructor with the same name is already exists", StatusCodes.Status409Conflict);
}
