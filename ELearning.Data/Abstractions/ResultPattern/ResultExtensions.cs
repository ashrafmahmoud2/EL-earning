using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace ELearning.Data.Abstractions.ResultPattern;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("cannot convert success result to problem");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;


        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors",new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };

        return new ObjectResult(problemDetails);
    }
}
