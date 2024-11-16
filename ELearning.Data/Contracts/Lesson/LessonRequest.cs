namespace ELearning.Data.Contracts.Lesson;

public record LessonRequest
(
    string Title,
    string Description,
    Guid SectionId
);
