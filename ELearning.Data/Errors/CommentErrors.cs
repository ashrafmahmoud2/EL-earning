using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record CommentErrors
{
    public static readonly Error  CommentNotFound =
        new(" Comment. CommentNotFound", " Comment is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedComment =
        new(" Comment.Duplicated Comment", "Another  Comment with the same name is already exists", StatusCodes.Status409Conflict);
}
