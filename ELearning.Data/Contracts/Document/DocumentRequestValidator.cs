namespace ELearning.Data.Contracts.Document;

public class DocumentRequestValidator : AbstractValidator<DocumentRequest>
{
    public DocumentRequestValidator()
    {
        RuleFor(x => x.Title)
            .Length(3, 100).NotEmpty();


        RuleFor(x => x.Description)
            .Length(3, 100).NotEmpty();

        RuleFor(x => x.DocumentPath)
            .Length(3, 100).NotEmpty().NotEmpty();

        RuleFor(x => x.LessonId)
            .NotEmpty();


    }
}
