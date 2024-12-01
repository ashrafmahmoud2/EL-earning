using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Document;
using ELearning.Data.Errors;
using ELearning.Data.Contracts.Answer;
namespace ELearning.Service.Service;

public class DocumentService : BaseRepository<Document>, IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DocumentService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DocumentResponse>> GetDocumentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
          var document = await _unitOfWork.Repository<Document>()
                                         .FirstOrDefaultAsync(x => x.DocumentId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Lesson),
                                         cancellationToken);

        if (document is null)
            return Result.Failure<DocumentResponse>(DocumentsErrors.DocumentNotFound);

        var DocumentResponse = document.Adapt<DocumentResponse>();

        return Result.Success(DocumentResponse);
    }

    public async Task<IEnumerable<DocumentResponse>> GetAllDocumentsAsync(CancellationToken cancellationToken = default)
    {
        var Documents = await _unitOfWork.Repository<Document>()
            .FindAsync(
                x =>x.IsActive,
                q => q.Include(x => x.CreatedBy)
                       .Include(x => x.Lesson),
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return Documents.Adapt<IEnumerable<DocumentResponse>>();
    }

    public async Task<Result<DocumentResponse>> CreateDocumentAsync(DocumentRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<DocumentResponse>(LessonsErrors.NotFound);


        if (await _unitOfWork.Repository<Document>().AnyAsync(x => x.Title == request.Title&& x.IsActive))
            return Result.Failure<DocumentResponse>(DocumentErrors.DuplicatedDocument);

        if (request is null)
            Result.Failure(DocumentsErrors.DocumentNotFound);


        var Document = request.Adapt<Document>();
        await _unitOfWork.Repository<Document>().AddAsync(Document, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(Document.Adapt<DocumentResponse>());
    }

    public async Task<Result<DocumentResponse>> UpdateDocumentAsync(Guid DocumentId, DocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<DocumentResponse>(LessonsErrors.NotFound);


        var document = await _unitOfWork.Repository<Document>()
                                         .FirstOrDefaultAsync(x => x.DocumentId == DocumentId && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Lesson),
                                         cancellationToken);

        if (document is null)
            return Result.Failure<DocumentResponse>(DocumentsErrors.DocumentNotFound);


        document.Description = request.Description;
        document.Title = request.Title;
        document.LessonId = request.LessonId;
        document.DocumentPath = request.DocumentPath;

        await _unitOfWork.Repository<Document>().UpdateAsync(document, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(document.Adapt<DocumentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await _unitOfWork.Repository<Document>()
                                           .FirstOrDefaultAsync(x => x.DocumentId == id && x.IsActive);

        if (document is null)
            return Result.Failure(DocumentsErrors.DocumentNotFound);

        document.IsActive = !document.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}



