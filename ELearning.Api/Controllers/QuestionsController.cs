using ELearning.Data.Contracts.Question;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController(IQuestionService QuestionService) : ControllerBase
{
    private readonly IQuestionService _QuestionService = QuestionService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Question = await _QuestionService.GetQuestionByIdAsync(id, cancellationToken);

        return Question.IsSuccess ? Ok(Question.Value) : Question.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllQuestions()
    {
        var Question = await _QuestionService.GetAllQuestionsAsync();

        return Ok(Question);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _QuestionService.CreateQuestionAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _QuestionService.UpdateQuestionAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusQuestion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _QuestionService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}





