namespace ELearning.Data.Contracts.Course;

public record CourseSectionLessonCountResponse
(
   Guid CourseId,
    int TotalSections,
    List<SectionLessonCountResponse> Sections
);
