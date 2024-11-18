using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record QuizAttemptsErrors
{
    public static readonly Error  NotFound =
        new(" QuizAttempt. NotFound", " QuizAttempt is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuizAttempt =
        new(" QuizAttempt.Duplicated QuizAttempt", "Another  QuizAttempt with the same name is already exists", StatusCodes.Status409Conflict);
}
