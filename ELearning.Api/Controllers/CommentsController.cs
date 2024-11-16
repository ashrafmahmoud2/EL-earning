using ELearning.Data.Contracts.Comment;
using ELearning.Data.Entities;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentService CommentService) : ControllerBase
{
    private readonly ICommentService _CommentService = CommentService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await _CommentService.GetCommentByIdAsync(id, cancellationToken);
        return comment.IsSuccess ? Ok(comment.Value) : comment.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllComments()
    {
        var comment = await _CommentService.GetAllCommentsAsync();

        return comment.IsSuccess ? Ok(comment.Value) : comment.ToProblem();
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateComment([FromBody] CommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _CommentService.CreateCommentAsync(request, cancellationToken);

        return comment.IsSuccess ? Created() : comment.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] CommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _CommentService.UpdateCommentAsync(id, request, cancellationToken);

        return comment.IsSuccess ? Ok(comment.Value) : comment.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusComment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await _CommentService.ToggleStatusAsync(id, cancellationToken);

        return comment.IsSuccess ? NoContent() : comment.ToProblem();
    }



}


