using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Categorys;
using ELearning.Data.Enums;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {

        var categoryResult = await _CategoryService.CreateCategoryAsync(request, cancellationToken);

        return categoryResult.IsSuccess? Created(): categoryResult.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _CategoryService.UpdateCategoryAsync(id, request, cancellationToken);

        return category.IsSuccess ? Ok(category.Value) : category.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var category = await _CategoryService.ToggleStatusAsync(id, cancellationToken);

        return category.IsSuccess ? NoContent() : category.ToProblem();
    }
}
