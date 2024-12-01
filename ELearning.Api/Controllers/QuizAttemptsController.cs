using ELearning.Data.Consts;
using ELearning.Data.Contracts.QuizAttempt;
using ELearning.Data.Entities;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.RateLimiting;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.Concurrency)]
[Authorize]
public class QuizAttemptsController(IQuizAttemptService QuizAttemptService) : ControllerBase
{
    private readonly IQuizAttemptService _QuizAttemptService = QuizAttemptService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizAttemptById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var quizAttempt = await _QuizAttemptService.GetQuizAttemptByIdAsync(id, cancellationToken);

        return quizAttempt.IsSuccess ? Ok(quizAttempt.Value) : quizAttempt.ToProblem();
    }


    [HttpGet("")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> GetAllQuizAttempts()
    {
        var quizAttempts = await _QuizAttemptService.GetAllQuizAttemptsAsync();

        return Ok(quizAttempts);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateQuizAttempt([FromBody] QuizAttemptRequest request, CancellationToken cancellationToken)
    {
        var quizAttempt = await _QuizAttemptService.CreateQuizAttemptAsync(request, cancellationToken);

        return quizAttempt.IsSuccess ? Created() : quizAttempt.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuizAttempt([FromRoute] Guid id, [FromBody] QuizAttemptRequest request, CancellationToken cancellationToken)
    {
        var quizAttempt = await  _QuizAttemptService.UpdateQuizAttemptAsync(id, request, cancellationToken);

        return quizAttempt.IsSuccess ? Ok(quizAttempt.Value) : quizAttempt.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusQuizAttempt([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var quizAttempt = await _QuizAttemptService.ToggleStatusAsync(id, cancellationToken);

        return quizAttempt.IsSuccess ? NoContent() : quizAttempt.ToProblem();
    }



}





