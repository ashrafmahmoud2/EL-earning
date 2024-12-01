using ELearning.Data.Contracts.Document;
using ELearning.Data.Enums;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentsController(IDocumentService DocumentService) : ControllerBase
{
    private readonly IDocumentService _DocumentService = DocumentService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var document = await _DocumentService.GetDocumentByIdAsync(id, cancellationToken);

        return document.IsSuccess ? Ok(document.Value) : document.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllDocuments()
    {
        var document = await _DocumentService.GetAllDocumentsAsync();

        return Ok(document);
    }


    [HttpPost("")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateDocument([FromBody] DocumentRequest request, CancellationToken cancellationToken)
    {
        var document = await _DocumentService.CreateDocumentAsync(request, cancellationToken);

        return document.IsSuccess ? Created() : document.ToProblem();
    }


 
    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateDocument([FromRoute] Guid id, [FromBody] DocumentRequest request, CancellationToken cancellationToken)
    {
        var document = await  _DocumentService.UpdateDocumentAsync(id, request, cancellationToken);

        return document.IsSuccess ? Ok(document.Value) : document.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusDocument([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var document = await _DocumentService.ToggleStatusAsync(id, cancellationToken);

        return document.IsSuccess ? NoContent() : document.ToProblem();
    }



}
