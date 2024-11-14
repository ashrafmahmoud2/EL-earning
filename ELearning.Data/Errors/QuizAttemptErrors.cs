using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record QuizAttemptErrors
{
    public static readonly Error  QuizAttemptNotFound =
        new(" QuizAttempt. QuizAttemptNotFound", " QuizAttempt is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuizAttempt =
        new(" QuizAttempt.Duplicated QuizAttempt", "Another  QuizAttempt with the same name is already exists", StatusCodes.Status409Conflict);
}
