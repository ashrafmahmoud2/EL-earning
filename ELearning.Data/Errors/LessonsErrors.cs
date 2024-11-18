using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record LessonsErrors
{
    public static readonly Error  NotFound =
        new(" Lesson. NotFound", " Lesson is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedLesson =
        new(" Lesson.Duplicated Lesson", "Another  Lesson with the same name is already exists", StatusCodes.Status409Conflict);
}
