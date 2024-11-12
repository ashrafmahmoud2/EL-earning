using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record QuizErrors
{
    public static readonly Error  QuizNotFound =
        new(" Quiz. QuizNotFound", " Quiz is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuiz =
        new(" Quiz.Duplicated Quiz", "Another  Quiz with the same name is already exists", StatusCodes.Status409Conflict);
}
