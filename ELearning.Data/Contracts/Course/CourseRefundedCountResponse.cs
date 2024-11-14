namespace ELearning.Data.Contracts.Course;

public record CourseRefundedCountResponse
(Guid CourseId,
    string CourseTitle,
    int RefundedCount
);



