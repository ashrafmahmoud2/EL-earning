using ELearning.Core.Base.ApiResponse;
using ELearning.Core.DTOs;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Errors;
using ELearning.Service.IService;
using Mapster;

namespace ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;

public class UpdateCommentCommandHandler(ICommentService commentService) : IRequestHandler<UpdateCommentCommand, Result<CommentDto>>
{
    private readonly ICommentService _commentService = commentService;

    public async Task<Result<CommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {

        var commentsResult = await _commentService.GetCommentByIdAsync(request.id, cancellationToken);
        var commentDto = commentsResult.IsSuccess ? commentsResult.Value.Adapt<CommentDto>() : null;
        if (commentDto == null)
            return Result.Failure<CommentDto>(CommentErrors.CommentNotFound);

        var comment = commentDto.Adapt<CommentRequest>();
        var updatedComment = await _commentService.UpdateCommentAsync(commentDto.CommentId, comment, cancellationToken);
        



        
        commentDto = commentDto.Adapt<CommentDto>();
        return Result.Success(commentDto);
    }
}
