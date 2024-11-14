using ELearning.Data.Contracts.Comment;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentService CommentService) : ControllerBase
{
    private readonly ICommentService _CommentService = CommentService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Comment = await _CommentService.GetCommentByIdAsync(id, cancellationToken);

        return Comment.IsSuccess ? Ok(Comment.Value) : Comment.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllComments()
    {
        var Comment = await _CommentService.GetAllCommentsAsync();

        return Ok(Comment);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateComment([FromBody] CommentRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _CommentService.CreateCommentAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] CommentRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _CommentService.UpdateCommentAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusComment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _CommentService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}


