using ELearning.Data.Contracts.QuizAttempt;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizAttemptsController(IQuizAttemptService QuizAttemptService) : ControllerBase
{
    private readonly IQuizAttemptService _QuizAttemptService = QuizAttemptService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizAttemptById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var QuizAttempt = await _QuizAttemptService.GetQuizAttemptByIdAsync(id, cancellationToken);

        return QuizAttempt.IsSuccess ? Ok(QuizAttempt.Value) : QuizAttempt.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllQuizAttempts()
    {
        var QuizAttempt = await _QuizAttemptService.GetAllQuizAttemptsAsync();

        return Ok(QuizAttempt);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateQuizAttempt([FromBody] QuizAttemptRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _QuizAttemptService.CreateQuizAttemptAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuizAttempt([FromRoute] Guid id, [FromBody] QuizAttemptRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _QuizAttemptService.UpdateQuizAttemptAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusQuizAttempt([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _QuizAttemptService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}





