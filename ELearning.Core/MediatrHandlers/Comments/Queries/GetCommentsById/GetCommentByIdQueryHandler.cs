using ELearning.Core.DTOs;
using ELearning.Core.MediatrHandlers.Student.Queries.GetCommenByIdQuery;
using ELearning.Data.Entities;
using ELearning.Service.IService;
using ELearning.Service.Service;
using Mapster;
using System.Threading;

namespace ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;

public class GetCommentByIdQueryHandler(ICommentService commentService, ApiResponseHandler responseHandler) : IRequestHandler<GetCommentByIdQuery, ApiResponse<CommentDto>>
{
    private readonly ICommentService _commentService = commentService;
    private readonly ApiResponseHandler _responseHandler = responseHandler;



    public async Task<ApiResponse<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var commentsResult = await _commentService.GetCommentByIdAsync(request.Id, cancellationToken);
        var commentDto = commentsResult.IsSuccess ? commentsResult.Value.Adapt<CommentDto>() : null;
        if (commentDto == null)
            return _responseHandler.NotFound<CommentDto>($"Comment with ID {request.Id} not found.");
        commentDto = commentDto.Adapt<CommentDto>();
        return _responseHandler.Success(commentDto);

       

    }

   
    

}