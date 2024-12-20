﻿using ELearning.Data.Consts;
using ELearning.Data.Contracts.Answer;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[Authorize]
public class AnswersController(IAnswerService AnswerService) : ControllerBase
{
    private readonly IAnswerService _AnswerService = AnswerService;


    [HttpGet("{id}")]
    [DisableRateLimiting()]
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
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateAnswer([FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var answer = await _AnswerService.CreateAnswerAsync(request, cancellationToken);

        return answer.IsSuccess ? Created() : answer.ToProblem();
    }


 
    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateAnswer([FromRoute] Guid id, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var answer = await  _AnswerService.UpdateAnswerAsync(id, request, cancellationToken);

        return answer.IsSuccess ? Ok(answer.Value) : answer.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var answer = await _AnswerService.ToggleStatusAsync(id, cancellationToken);

        return answer.IsSuccess ? NoContent() : answer.ToProblem();
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _AnswerService.DeleteAnswerAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }





}




