using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record LessonErrors
{
    public static readonly Error  LessonNotFound =
        new(" Lesson. LessonNotFound", " Lesson is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedLesson =
        new(" Lesson.Duplicated Lesson", "Another  Lesson with the same name is already exists", StatusCodes.Status409Conflict);
}
