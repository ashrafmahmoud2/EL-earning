using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record SectionErrors
{
    public static readonly Error  SectionNotFound =
        new(" Section. SectionNotFound", " Section is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedSection =
        new(" Section.Duplicated Section", "Another  Section with the same name is already exists", StatusCodes.Status409Conflict);
}
