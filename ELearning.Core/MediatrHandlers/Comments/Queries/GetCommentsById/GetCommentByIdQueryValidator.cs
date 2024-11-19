using ELearning.Core.DTOs;
using FluentValidation;

namespace ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;

public class GetCommentByIdQueryValidator : AbstractValidator<CommentDto>
{
    public GetCommentByIdQueryValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty();
    }
}
