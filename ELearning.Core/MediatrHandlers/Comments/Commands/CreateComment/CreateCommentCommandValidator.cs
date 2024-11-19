using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.MediatrHandlers.Comments.Commands.CreateComment;
public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 150).WithMessage("Title must be between 3 and 150 characters.");

        RuleFor(x => x.CommentText)
            .NotEmpty().WithMessage("Comment text is required.")
            .Length(1, 1000).WithMessage("Comment text must be between 1 and 1000 characters.");

        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("LessonId is required.");

        RuleFor(x => x.CommentedByUserId)
            .NotEmpty().WithMessage("CommentedByUserId is required.");
    }
}