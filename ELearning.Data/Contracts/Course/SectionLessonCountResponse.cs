namespace ELearning.Data.Contracts.Course
{
    public record SectionLessonCountResponse
    (
        Guid SectionId,
        string SectionName,
        int TotalLessons
    );
}
