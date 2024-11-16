namespace ELearning.Data.Contracts.Course
{
    public record CourseSectionLessonCountResponse
    (
        Guid CourseId,
        string CourseName,
        int TotalSections,
        List<SectionLessonCountResponse> Sections
    );
}
