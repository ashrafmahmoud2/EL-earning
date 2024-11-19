using ELearning.Core.Base.ApiResponse;
using ELearning.Core.DTOs;
using ELearning.Data.Contracts.Comment;
using ELearning.Service.IService;
using Mapster;

namespace ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;

public class UpdateCommentCommandHandler(ICommentService commentService, ApiResponseHandler responseHandler) : IRequestHandler<UpdateCommentCommand, ApiResponse<CommentDto>>
{
    private readonly ICommentService _commentService = commentService;
    private readonly ApiResponseHandler _responseHandler = responseHandler;

    public async Task<ApiResponse<CommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {

        var commentsResult = await _commentService.GetCommentByIdAsync(request.id, cancellationToken);
        var commentDto = commentsResult.IsSuccess ? commentsResult.Value.Adapt<CommentDto>() : null;
        if (commentDto == null)
            return _responseHandler.NotFound<CommentDto>($"Comment with ID {request.id} not found.");

        var comment = commentDto.Adapt<CommentRequest>();
        var updatedComment = await _commentService.UpdateCommentAsync(commentDto.CommentId, comment, cancellationToken);
        

        return _responseHandler.Success(updatedComment.Adapt<CommentDto>(), "Comment updated successfully.");


        
        commentDto = commentDto.Adapt<CommentDto>();
        return _responseHandler.Success(commentDto);
    }
}
