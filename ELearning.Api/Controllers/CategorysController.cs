using ELearning.Data.Contracts.Categorys;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategorysController(ICategoryService CategoryService) : ControllerBase
{
    private readonly ICategoryService _CategoryService = CategoryService;


    /// <summary>
    /// Gets a Category by id.
    /// </summary>
    /// <param name="id">The id of the Category.</param>
    /// <returns>The Category.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Categorys/3fa85f64-5717-4562-b3fc-2c963f66afa6
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
    ///         "message": "Category retrieved successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the requested Category</response>
    /// <response code="404">If the Category is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Category = await _CategoryService.GetCategoryByIdAsync(id,cancellationToken);

        return Category.IsSuccess ? Ok(Category.Value) : Category.ToProblem();
    }

  
    /// <summary>
    /// Gets all Categorys.
    /// </summary>
    /// <returns>List of all Categorys.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Categorys
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
    ///         "message": "Categorys retrieved successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the list of Categorys</response>
    [HttpGet("")]
    public async Task<IActionResult> GetAllCategorys()
    {
        var Category = await _CategoryService.GetAllCategorysAsync();

        return Ok(Category);
    }

   
    [HttpPost("")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _CategoryService.CreateCategoryAsync( request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


    /// <summary>
    /// Updates an existing Category.
    /// </summary>
    /// <param name="id">The id of the Category to update.</param>
    /// <param name="command">The update Category command.</param>
    /// <returns>The updated Category.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/Categorys/3fa85f64-5717-4562-b3fc-2c963f66afa6
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
    ///         "message": "Category updated successfully.",
    ///         "statusCode": 200,
    ///         "error": null
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the updated Category</response>
    /// <response code="400">If the item is null or invalid</response>
    /// <response code="404">If the Category is not found</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _CategoryService.UpdateCategoryAsync(id, request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }
    

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _CategoryService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}
