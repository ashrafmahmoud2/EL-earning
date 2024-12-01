using ELearning.Core.DTOs;
using ELearning.Data.Abstractions.ResultPattern;
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
public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
{
    private readonly ICommentService _commentService;

    public CreateCommentCommandHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = request.Adapt<Comment>();
        var createdComment = await _commentService.CreateCommentAsync(comment.Adapt<CommentRequest>(), cancellationToken);
        var commentDto = createdComment.Adapt<CommentDto>();

        return Result.Success(commentDto);
    }
}
