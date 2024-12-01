using ELearning.Core.DTOs;
using ELearning.Data.Abstractions.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.MediatrHandlers.Comments.Commands.CreateComment;
public class CreateCommentCommand : IRequest<Result<CommentDto>>
{
    public string Title { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public Guid LessonId { get; set; }
    public string CommentedByUserId { get; set; } = string.Empty;
}