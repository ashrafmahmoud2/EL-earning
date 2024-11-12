namespace ELearning.Data.Contracts.Enrollment;

public record EnrollmentAddRequest(
       Guid StudentId,
       Guid CourseId
   );
