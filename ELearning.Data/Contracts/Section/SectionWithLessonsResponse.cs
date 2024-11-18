using ELearning.Data.Contracts.Lesson;

namespace ELearning.Data.Contracts.Section;

public record SectionWithLessonsResponse
(
 Guid SectionId,
 string Title,
 string Description,
 int OrderIndex,
 bool IsActive,
 Guid CourseId,
 string CourseTitle,
IEnumerable<LessonResponse> Lessons ,
string CreatedBy
 );



