using ELearning.Data.Contracts.Quiz;
using ELearning.Data.Entities;
using ELearning.Data.Enums;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuizsController(IQuizService QuizService) : ControllerBase
{
    private readonly IQuizService _QuizService = QuizService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var quiz = await _QuizService.GetQuizByIdAsync(id, cancellationToken);

        return quiz.IsSuccess ? Ok(quiz.Value) : quiz.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllQuizs()
    {
        var quizs = await _QuizService.GetAllQuizsAsync();

        return Ok(quizs);
    }


    [HttpPost("")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateQuiz([FromBody] QuizRequest request, CancellationToken cancellationToken)
    {
        var quiz = await _QuizService.CreateQuizAsync(request, cancellationToken);

        return quiz.IsSuccess ? Created() : quiz.ToProblem();
    }


 
    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateQuiz([FromRoute] Guid id, [FromBody] QuizRequest request, CancellationToken cancellationToken)
    {
        var quiz =await  _QuizService.UpdateQuizAsync(id, request, cancellationToken);

        return quiz.IsSuccess ? Ok(quiz.Value) : quiz.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusQuiz([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var quiz = await _QuizService.ToggleStatusAsync(id, cancellationToken);

        return quiz.IsSuccess ? NoContent() : quiz.ToProblem();
    }



}
