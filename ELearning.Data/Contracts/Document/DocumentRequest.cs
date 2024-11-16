using ELearning.Data.Contracts.Enrollment;

namespace ELearning.Data.Contracts.Document;

public record DocumentRequest
(
    string Title,
    string? Description,
    string DocumentPath,
    Guid LessonId
 );
