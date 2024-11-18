using ELearning.Data.Contracts.Answer;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswersController(IAnswerService AnswerService) : ControllerBase
{
    private readonly IAnswerService _AnswerService = AnswerService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnswerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var answer = await _AnswerService.GetAnswerByIdAsync(id, cancellationToken);

        return answer.IsSuccess ? Ok(answer.Value) : answer.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllAnswers()
    {
        var answer = await _AnswerService.GetAllAnswersAsync();

        return Ok(answer);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateAnswer([FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var answer = await _AnswerService.CreateAnswerAsync(request, cancellationToken);

        return answer.IsSuccess ? Created() : answer.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnswer([FromRoute] Guid id, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var answer = await  _AnswerService.UpdateAnswerAsync(id, request, cancellationToken);

        return answer.IsSuccess ? Ok(answer.Value) : answer.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var answer = await _AnswerService.ToggleStatusAsync(id, cancellationToken);

        return answer.IsSuccess ? NoContent() : answer.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _AnswerService.DeleteAnswerAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }





}




