using ELearning.Data.Contracts.Section;
using ELearning.Data.Enums;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionsController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionsController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRole.Admin + "," + UserRole.Instructor)] // Only Admin and Instructor can access this
    public async Task<IActionResult> GetSectionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var section = await _sectionService.GetSectionByIdAsync(id, cancellationToken);
        return section.IsSuccess ? Ok(section.Value) : section.ToProblem();
    }

    [HttpGet("")]
    [Authorize(Roles = UserRole.Admin + "," + UserRole.Instructor)] // Only Admin and Instructor can access this
    public async Task<IActionResult> GetAllSections()
    {
        var sections = await _sectionService.GetAllSectionsAsync();
        return Ok(sections);
    }

    [HttpGet("with-lessons")]
    [Authorize(Roles = UserRole.Admin + "," + UserRole.Instructor)] // Only Admin and Instructor can access this
    public async Task<IActionResult> GetAllSectionsWithLessons()
    {
        var sections = await _sectionService.GetAllSectionsWithLessonsAsync();
        return Ok(sections);
    }

    [HttpPost("")]
    [Authorize(Roles = UserRole.Admin)] 
    public async Task<IActionResult> CreateSection([FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var section = await _sectionService.CreateSectionAsync(request, cancellationToken);

        return section.IsSuccess? Created(): section.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRole.Admin + "," + UserRole.Instructor)] // Admin and Instructor can update sections
    public async Task<IActionResult> UpdateSection([FromRoute] Guid id, [FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var section = await _sectionService.UpdateSectionAsync(id, request, cancellationToken);
        return section.IsSuccess ? Ok(section.Value) : section.ToProblem();
    }

    [HttpPut("Toggle_status/{id}")]
    [Authorize(Roles = UserRole.Admin)] 
    public async Task<IActionResult> ToggleStatusSection([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var section = await _sectionService.ToggleStatusAsync(id, cancellationToken);
        return section.IsSuccess ? NoContent() : section.ToProblem();
    }
}