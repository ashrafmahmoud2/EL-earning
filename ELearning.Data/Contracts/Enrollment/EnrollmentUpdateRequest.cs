using ELearning.Data.Enums;

namespace ELearning.Data.Contracts.Enrollment;

public record EnrollmentUpdateRequest(
     
       //EnrollmentStatus NewStatus
       Guid StudentId,
Guid CourseId
   );
