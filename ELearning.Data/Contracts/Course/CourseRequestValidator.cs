namespace ELearning.Data.Contracts.Course;

public class CourseRequestValidator : AbstractValidator<CourseRequest>
{
    public CourseRequestValidator()
    {
        // Title validation: required and max length of 100 characters
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(5, 100);

        // Description validation: required and max length of 500 characters
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(5, 1000);

        // ShortDescription validation: required and max length of 200 characters
        RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Short Description is required.")
            .Length(5, 100);

       
        RuleFor(x => x.InstructorId)
            .NotEqual(Guid.Empty).WithMessage("InstructorId must be a valid GUID.");

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).WithMessage("CategoryId must be a valid GUID.");

        RuleFor(x => x.ThumbnailUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Thumbnail URL must be a valid URL.");

        RuleFor(x => x.TrailerVideoUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Trailer Video URL must be a valid URL.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(level => new[] { "Beginner", "Intermediate", "Advanced" }.Contains(level))
            .WithMessage("Level must be one of the following: Beginner, Intermediate, Advanced.");

        RuleFor(x => x.Prerequisites)
            .NotEmpty().WithMessage("Prerequisites should not be empty.")
            .Must(prerequisites => prerequisites.Count > 0).WithMessage("Prerequisites must contain at least one item.");

        RuleFor(x => x.LearningObjectives)
            .NotEmpty().WithMessage("Learning objectives should not be empty.")
            .Must(objectives => objectives.Count > 0).WithMessage("Learning objectives must contain at least one item.");

       
        RuleFor(x => x.TotalTime)
            .GreaterThan(TimeSpan.Zero).WithMessage("Total time must be greater than zero.");
    }
}



