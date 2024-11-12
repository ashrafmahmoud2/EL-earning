using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record DocumentErrors
{
    public static readonly Error  DocumentNotFound =
        new(" Document. DocumentNotFound", " Document is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedDocument =
        new(" Document.Duplicated Document", "Another  Document with the same name is already exists", StatusCodes.Status409Conflict);
}