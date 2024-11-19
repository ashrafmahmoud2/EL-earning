using ELearning.Core.DTOs;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Entities;
using ELearning.Service.IService;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.MediatrHandlers.Comments.Commands.CreateComment;
public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ApiResponse<CommentDto>>
{
    private readonly ICommentService _commentService;
    private readonly ApiResponseHandler _responseHandler;

    public CreateCommentCommandHandler(ICommentService commentService, ApiResponseHandler responseHandler)
    {
        _commentService = commentService;
        _responseHandler = responseHandler;
    }

    public async Task<ApiResponse<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = request.Adapt<Comment>();
        var createdComment = await _commentService.CreateCommentAsync(comment.Adapt<CommentRequest>(), cancellationToken);

        // Map the created entity to the DTO
        var commentDto = createdComment.Adapt<CommentDto>();

        return _responseHandler.Success(commentDto, "Comment created successfully.");
    }
}
