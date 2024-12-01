using ELearning.Data.Contracts.Instrctors;
using ELearning.Data.Entities;
using ELearning.Data.Enums;
using ELearning.Infrastructure.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = UserRole.Admin)]
public class InstructorsController(IMediator mediator, IUnitOfWork unitOfWork, IInstructorService InstructorService) : ControllerBase
{
    //create instructor in AuthService :: RegisterAsync();

    private readonly IInstructorService _instructorService = InstructorService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstructorById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var instructor = await _instructorService.GetInstructorByIdAsync(id);
        return instructor.IsSuccess ? Ok(instructor.Value) : instructor.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllInstructors()
    {
        var instructors = await _instructorService.GetAllInstructorsAsync();
        return instructors.IsSuccess ? Ok(instructors.Value) : instructors.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInstructor(Guid id, [FromBody] InstructorRequest request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorService.UpdateInstructorAsync(id, request, cancellationToken);
        return instructor.IsSuccess ? NoContent() : instructor.ToProblem();
    }

    [HttpPut("Toggle_status/{id}")]
    public async Task<IActionResult> ToggleStatusInstructor([FromRoute] Guid id)
    {
        var instructor = await _instructorService.ToggleStatusAsync(id);
        return instructor.IsSuccess ? NoContent() : instructor.ToProblem();
    }
  
}