using ELearning.Data.Contracts.Quiz;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizsController(IQuizService QuizService) : ControllerBase
{
    private readonly IQuizService _QuizService = QuizService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Quiz = await _QuizService.GetQuizByIdAsync(id, cancellationToken);

        return Quiz.IsSuccess ? Ok(Quiz.Value) : Quiz.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllQuizs()
    {
        var Quiz = await _QuizService.GetAllQuizsAsync();

        return Ok(Quiz);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateQuiz([FromBody] QuizRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _QuizService.CreateQuizAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz([FromRoute] Guid id, [FromBody] QuizRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _QuizService.UpdateQuizAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusQuiz([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _QuizService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}
