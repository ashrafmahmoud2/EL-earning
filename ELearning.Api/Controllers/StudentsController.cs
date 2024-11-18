using ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;
using ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;
using ELearning.Api.Base;
using ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;
using ELearning.Infrastructure.Base;
using ELearning.Service.IService;
using ELearning.Service.Service;
using ELearning.Data.Contracts.Students;
using System.Threading;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    private readonly IStudentService _studentService = studentService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var student = await _studentService.GetStudentByIdAsync(id);

        return student.IsSuccess ? Ok(student.Value) : student.ToProblem();
    }

    
    [HttpGet("")]
    public async Task<IActionResult> GetAllStudents()
    {
        var student = await _studentService.GetAllStudentsAsync();

        return Ok(student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, [FromBody] StudentRequest request, CancellationToken cancellationToken)
    {
        var student = await _studentService.UpdateStudentAsync(id, request, cancellationToken);

        return student.IsSuccess ? NoContent() : student.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusStudent([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var student = await _studentService.ToggleStatusAsync(id, cancellationToken);

        return student.IsSuccess ? NoContent() : student.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _studentService.DeleteStudentAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }



}
