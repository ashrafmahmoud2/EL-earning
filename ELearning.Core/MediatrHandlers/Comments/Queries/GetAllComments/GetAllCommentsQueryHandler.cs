using ELearning.Core.Base.ApiResponse;
using ELearning.Core.DTOs;
using ELearning.Data.Entities;
using ELearning.Service.IService;
using ELearning.Service.Service;
using Mapster;
using System.Threading;

namespace ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;

public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentQuery, ApiResponse<IEnumerable<CommentDto>>>
{
    private readonly ICommentService _commentService;
    private readonly ApiResponseHandler _responseHandler;

    public GetAllCommentsQueryHandler(ICommentService commentService, ApiResponseHandler responseHandler)
    {
        _commentService = commentService;
        _responseHandler = responseHandler;
    }

    public async Task<ApiResponse<IEnumerable<CommentDto>>> Handle(GetAllCommentQuery request, CancellationToken cancellationToken)
    {
        var commentsResult = await _commentService.GetAllCommentsAsync(cancellationToken);
        var commentDtos = commentsResult.IsSuccess ?  commentsResult.Value.Adapt<IEnumerable<CommentDto>>() : null; 
        commentDtos = commentDtos.Adapt<IEnumerable<CommentDto>>();
        return _responseHandler.Success(commentDtos);
    }


}
