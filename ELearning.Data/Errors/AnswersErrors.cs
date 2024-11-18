﻿using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record AnswersErrors
{
    
    public static readonly Error  NotFound =
        new(" Answer.NotFound", " Answer is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedAnswer =
        new(" Answer.Duplicated Answer", "Another  Answer with the same name is already exists", StatusCodes.Status409Conflict);

    public static readonly Error MissingCorrectAnswer =
      new("Answer.MissingCorrectAnswer", "Each question must have at least one answer marked as correct.", StatusCodes.Status409Conflict);

}