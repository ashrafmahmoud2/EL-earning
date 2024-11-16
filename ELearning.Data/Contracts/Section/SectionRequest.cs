using ELearning.Data.Entities;


namespace ELearning.Data.Contracts.Section;

public record SectionRequest
(
     string Title,
 string Description,
 Guid CourseId

// ICollection<Lesson> Lessons
);

