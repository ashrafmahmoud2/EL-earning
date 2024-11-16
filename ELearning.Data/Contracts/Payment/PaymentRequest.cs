namespace ELearning.Data.Contracts.Payment;

public record PaymentRequest
(

Guid EnrollmentId,
Guid StudentId,
Guid CourseId
);
