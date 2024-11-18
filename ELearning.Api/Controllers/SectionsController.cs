using ELearning.Data.Contracts.Section;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionsController(ISectionService SectionService) : ControllerBase
{
    private readonly ISectionService _SectionService = SectionService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSectionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var section = await _SectionService.GetSectionByIdAsync(id, cancellationToken);

        return section.IsSuccess ? Ok(section.Value) : section.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllSections()
    {
        var sections = await _SectionService.GetAllSectionsAsync();

        return Ok(sections);
    }

    [HttpGet("with-lessons")]
    public async Task<IActionResult> GetAllSectionsWithLessons()
    {
        var sections = await _SectionService.GetAllSectionsWithLessonsAsync();
        return Ok(sections);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateSection([FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var section = await _SectionService.CreateSectionAsync(request, cancellationToken);

        return section.IsSuccess ? Created() : section.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSection([FromRoute]Guid id, [FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var section = await _SectionService.UpdateSectionAsync(id, request, cancellationToken);

        return section.IsSuccess ? Ok(section.Value) : section.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusSection([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var section = await _SectionService.ToggleStatusAsync(id, cancellationToken);

        return section.IsSuccess ? NoContent() : section.ToProblem();
    }
}
