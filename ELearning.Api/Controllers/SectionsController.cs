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
        var Section = await _SectionService.GetSectionByIdAsync(id, cancellationToken);

        return Section.IsSuccess ? Ok(Section.Value) : Section.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllSections()
    {
        var Section = await _SectionService.GetAllSectionsAsync();

        return Ok(Section);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateSection([FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var Section = await _SectionService.CreateSectionAsync(request, cancellationToken);

        return Section.IsSuccess ? NoContent() : Section.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSection([FromRoute]Guid id, [FromBody] SectionRequest request, CancellationToken cancellationToken)
    {
        var Section = await _SectionService.UpdateSectionAsync(id, request, cancellationToken);

        return Section.IsSuccess ? NoContent() : Section.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusSection([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Section = await _SectionService.ToggleStatusAsync(id, cancellationToken);

        return Section.IsSuccess ? NoContent() : Section.ToProblem();
    }
}
