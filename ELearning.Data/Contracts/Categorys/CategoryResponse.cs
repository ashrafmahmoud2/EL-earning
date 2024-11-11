namespace ELearning.Data.Contracts.Categorys;

public record CategoryResponse
(
    Guid CategoryId,
    string Name,
   int NumberOfCourses,
   bool IsActive,
   string CreatedBy,
   DateTime CreatedOn
);


