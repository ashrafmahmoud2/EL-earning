using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Entities;

namespace ELearning.Service.IService;

public interface ICommentService
{
    Task<Result<CommentResponse>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CommentResponse>>> GetAllCommentsAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateCommentAsync(CommentRequest request, CancellationToken cancellationToken = default);
    Task<Result<CommentResponse>> UpdateCommentAsync(Guid id, CommentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<int>> CountCommentsForLesson(Guid lessonId, CancellationToken cancellationToken = default);
}

