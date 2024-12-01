namespace ELearning.Data.Contracts.Comment;

public class CommentRequestValidator : AbstractValidator<CommentRequest>
{
    public CommentRequestValidator()
    {
        RuleFor(x => x.Title)
            .Length(3,150)
            .NotEmpty();

        RuleFor(x => x.CommentText)
        .Length(1,1000)
           .NotEmpty();

        RuleFor(x => x.LessonId)
           .NotEmpty();

        RuleFor(x => x.commentedByUserId)
           .NotEmpty();

    }
}
