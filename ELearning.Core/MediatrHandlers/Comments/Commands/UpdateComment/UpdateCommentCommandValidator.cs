using FluentValidation;

namespace ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Title)
              .Length(3, 150)
              .NotEmpty();

        RuleFor(x => x.CommentText)
        .Length(1, 1000)
           .NotEmpty();

        RuleFor(x => x.LessonId)
           .NotEmpty();

        RuleFor(x => x.ApplicationUserID)
           .NotEmpty();
    }
}