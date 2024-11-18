using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record QuestionsErrors
{
    public static readonly Error  NotFound =
        new(" Question. NotFound", " Question is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedQuestion =
        new(" Question.Duplicated Question", "Another  Question with the same name is already exists", StatusCodes.Status409Conflict);
}
