using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record QuizsErrors
{
    public static readonly Error  NotFound =
        new(" Quiz. NotFound", " Quiz is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuiz =
        new(" Quiz.Duplicated Quiz", "Another  Quiz with the same name is already exists", StatusCodes.Status409Conflict);
}
