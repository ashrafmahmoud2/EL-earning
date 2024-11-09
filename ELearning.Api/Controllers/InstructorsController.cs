using ELearning.Data.Contracts.Instrctors;
using ELearning.Infrastructure.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController(IMediator mediator, IUnitOfWork unitOfWork, IInstructorService InstructorService) : ControllerBase
    {
        private readonly IInstructorService _instructorService = InstructorService;

        /// <summary>
        /// Gets a Instructor by id.
        /// </summary>
        /// <param name="id">The id of the Instructor.</param>
        /// <returns>The Instructor.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Instructors/3fa85f64-5717-4562-b3fc-2c963f66afa6
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
        ///         "message": "Instructor retrieved successfully.",
        ///         "statusCode": 200,
        ///         "error": null
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns the requested Instructor</response>
        /// <response code="404">If the Instructor is not found</response>
        [HttpGet( "{id}")]
        public async Task<IActionResult> GetInstructorById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var Instructor = await _instructorService.GetInstructorByIdAsync(id);

            return Instructor.IsSuccess ? Ok(Instructor.Value) : Instructor.ToProblem();
        }

        /// <summary>
        /// Gets all Instructors.
        /// </summary>
        /// <returns>List of all Instructors.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Instructors
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
        ///         "message": "Instructors retrieved successfully.",
        ///         "statusCode": 200,
        ///         "error": null
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns the list of Instructors</response>
        [HttpGet("")]
        public async Task<IActionResult> GetAllInstructors()
        {
            var Instructor = await _instructorService.GetAllInstructorsAsync();

            return Ok(Instructor);
        }

        [HttpPut("Toggle_status/{id}")]
        public async Task<IActionResult> ToggleStatusInstructor([FromRoute] Guid id)
        {
            var Instructor = await _instructorService.ToggleStatusAsync(id);

            return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
        }

        /// <summary>
        /// Updates an existing Instructor.
        /// </summary>
        /// <param name="id">The id of the Instructor to update.</param>
        /// <param name="command">The update Instructor command.</param>
        /// <returns>The updated Instructor.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Instructors/3fa85f64-5717-4562-b3fc-2c963f66afa6
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
        ///         "message": "Instructor updated successfully.",
        ///         "statusCode": 200,
        ///         "error": null
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns the updated Instructor</response>
        /// <response code="400">If the item is null or invalid</response>
        /// <response code="404">If the Instructor is not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(Guid id, [FromBody]InstructorRequest request,CancellationToken cancellationToken)
        {
            var instructor = await _instructorService.UpdateInstructorAsync(id,request,cancellationToken);

            return instructor.IsSuccess ? NoContent() : instructor.ToProblem();
        }

    }
}
