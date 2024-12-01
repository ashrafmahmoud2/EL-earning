using ELearning.Api.Base;
using ELearning.Core.MediatrHandlers.Comments.Commands.CreateComment;
using ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;
using ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;
using ELearning.Core.MediatrHandlers.Student.Queries.GetCommenByIdQuery;
using ELearning.Data.Consts;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Entities;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.RateLimiting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[Authorize]
public class CommentsController(ICommentService CommentService) : AppControllerBase
{
    private readonly ICommentService _CommentService = CommentService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCommentByIdQuery { Id = id });

        return CreateResponse(result);
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllComments()
    {
        var response = await Mediator.Send(new GetAllCommentQuery());
        return CreateResponse(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand comment, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(comment,cancellationToken);

        return response.IsSuccess ? Created() : response.ToProblem();

    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateComment( [FromBody] UpdateCommentCommand comment, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(comment, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusComment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await _CommentService.ToggleStatusAsync(id, cancellationToken);

        return comment.IsSuccess ? NoContent() : comment.ToProblem();
    }



}


