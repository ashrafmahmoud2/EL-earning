using ELearning.Data.Contracts.Categorys;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategorysController(ICategoryService CategoryService) : ControllerBase
{
    private readonly ICategoryService _CategoryService = CategoryService;
 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Category = await _CategoryService.GetCategoryByIdAsync(id,cancellationToken);

        return Category.IsSuccess ? Ok(Category.Value) : Category.ToProblem();
    } 

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
