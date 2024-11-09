using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record StudentErrors
{
    public static readonly Error StudentNotFound =
        new("Student.StudentNotFound", "Student is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Student.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedStudent =
        new("Student.DuplicatedStudent", "Another Student with the same name is already exists", StatusCodes.Status409Conflict);
}
