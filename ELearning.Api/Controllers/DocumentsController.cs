using ELearning.Data.Contracts.Document;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController(IDocumentService DocumentService) : ControllerBase
{
    private readonly IDocumentService _DocumentService = DocumentService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Document = await _DocumentService.GetDocumentByIdAsync(id, cancellationToken);

        return Document.IsSuccess ? Ok(Document.Value) : Document.ToProblem();
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAllDocuments()
    {
        var Document = await _DocumentService.GetAllDocumentsAsync();

        return Ok(Document);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateDocument([FromBody] DocumentRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _DocumentService.CreateDocumentAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocument([FromRoute] Guid id, [FromBody] DocumentRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _DocumentService.UpdateDocumentAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusDocument([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _DocumentService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }



}
