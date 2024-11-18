using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record StudentsErrors
{
    public static readonly Error NotFound =
        new("Student.NotFound", "Student is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Student.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedStudent =
        new("Student.DuplicatedStudent", "Another Student with the same name is already exists", StatusCodes.Status409Conflict);

    public static readonly Error ForeignKeyViolation =
        new("Student.ForeignKeyViolation", "The student cannot be deleted because it is associated with other records.", StatusCodes.Status400BadRequest);
    
    
    public static readonly Error UnexpectedError =
        new("Student.UnexpectedError", "The student cannot be deleted because it is associated with other records.", StatusCodes.Status400BadRequest);


}
