namespace ELearning.Data.Contracts.Course;

public record SectionLessonCountResponse
(
    Guid SectionId,
    int TotalLessons
);
