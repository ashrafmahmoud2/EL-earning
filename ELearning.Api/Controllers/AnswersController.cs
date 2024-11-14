using ELearning.Data.Contracts.Answer;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswersController(IAnswerService AnswerService) : ControllerBase
{
    private readonly IAnswerService _AnswerService = AnswerService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnswerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Answer = await _AnswerService.GetAnswerByIdAsync(id, cancellationToken);

        return Answer.IsSuccess ? Ok(Answer.Value) : Answer.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllAnswers()
    {
        var Answer = await _AnswerService.GetAllAnswersAsync();

        return Ok(Answer);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateAnswer([FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _AnswerService.CreateAnswerAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnswer([FromRoute] Guid id, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _AnswerService.UpdateAnswerAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _AnswerService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}




