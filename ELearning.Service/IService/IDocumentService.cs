using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Document;

namespace ELearning.Service.IService;

public interface IDocumentService
{
    Task<Result<DocumentResponse>> GetDocumentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentResponse>> GetAllDocumentsAsync(CancellationToken cancellationToken = default);
    Task<Result<DocumentResponse>> CreateDocumentAsync(DocumentRequest request, CancellationToken cancellationToken = default);
    Task<Result<DocumentResponse>> UpdateDocumentAsync(Guid id, DocumentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

