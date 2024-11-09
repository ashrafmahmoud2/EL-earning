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



    /// <summary>
    /// Gets a student by id.
    /// </summary>
    /// <param name="id">The id of the student.</param>
    /// <returns>The student.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/students/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "data": {
    ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///             "firstName": "John",
    ///             "lastName": "Doe",
    ///             "email": "john.doe@example.com"
    ///         },
    ///         "message": "Student retrieved successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the requested student</response>
    /// <response code="404">If the student is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var student = await _studentService.GetStudentByIdAsync(id);

        return student.IsSuccess ? Ok(student.Value) : student.ToProblem();
    }

    /// <summary>
    /// Gets all students.
    /// </summary>
    /// <returns>List of all students.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/students
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "data": [
    ///             {
    ///                 "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///                 "firstName": "John",
    ///                 "lastName": "Doe",
    ///                 "email": "john.doe@example.com"
    ///             },
    ///             {
    ///                 "id": "8a1b9c3d-4e5f-6g7h-8i9j-0k1l2m3n4o5p",
    ///                 "firstName": "Jane",
    ///                 "lastName": "Smith",
    ///                 "email": "jane.smith@example.com"
    ///             }
    ///         ],
    ///         "message": "Students retrieved successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the list of students</response>
    [HttpGet("")]
    public async Task<IActionResult> GetAllStudents()
    {
        var student = await _studentService.GetAllStudentsAsync();

        return Ok(student);
    }

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusStudent([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _studentService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }

    /// <summary>
    /// Updates an existing student.
    /// </summary>
    /// <param name="id">The id of the student to update.</param>
    /// <param name="command">The update student command.</param>
    /// <returns>The updated student.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/students/3fa85f64-5717-4562-b3fc-2c963f66afa6
    ///     {
    ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "firstName": "John",
    ///         "lastName": "Smith",
    ///         "email": "john.smith@example.com"
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "data": {
    ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///             "firstName": "John",
    ///             "lastName": "Smith",
    ///             "email": "john.smith@example.com"
    ///         },
    ///         "message": "Student updated successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the updated student</response>
    /// <response code="400">If the item is null or invalid</response>
    /// <response code="404">If the student is not found</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent([FromRoute]Guid id, [FromBody] StudentRequest request,CancellationToken cancellationToken)
    {
        var Instructor = await _studentService.UpdateStudentAsync(id,request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }




}
