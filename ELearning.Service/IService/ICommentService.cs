using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Comment;

namespace ELearning.Service.IService;

public interface ICommentService
{
    Task<Result<CommentResponse>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CommentResponse>> GetAllCommentsAsync(CancellationToken cancellationToken = default);
    Task<Result<CommentResponse>> CreateCommentAsync(CommentRequest request, CancellationToken cancellationToken = default);
    Task<Result<CommentResponse>> UpdateCommentAsync(Guid id, CommentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

