namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService QuestionService) : ControllerBase
{
    private readonly IQuestionService _QuestionService = QuestionService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var question = await _QuestionService.GetQuestionByIdAsync(id, cancellationToken);

        return question.IsSuccess ? Ok(question.Value) : question.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllQuestions()
    {
        var questions = await _QuestionService.GetAllQuestionsAsync();

        return Ok(questions);
    }

    [HttpPost("")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await _QuestionService.CreateQuestionAsync(request, cancellationToken);

        return question.IsSuccess ? Created() : question.ToProblem();
    }
 
    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await  _QuestionService.UpdateQuestionAsync(id, request, cancellationToken);

        return question.IsSuccess ? Ok(question.Value) : question.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]

    public async Task<IActionResult> ToggleStatusQuestion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var question = await _QuestionService.ToggleStatusAsync(id, cancellationToken);

        return question.IsSuccess ? NoContent() : question.ToProblem();
    }

}





