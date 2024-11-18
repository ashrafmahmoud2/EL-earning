﻿using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record CoursesErrors
{
    public static readonly Error NotFound =
        new("Course.NotFound", "Course is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Course.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedCourse =
        new("Course.DuplicatedCourse", "Another Course with the same name is already exists", StatusCodes.Status409Conflict);
}