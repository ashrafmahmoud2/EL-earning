using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Payment;
public record PaymentResponse
(
Guid PaymentId,
DateTime PaymentDate,
decimal Amount,
Guid StudentId,
Guid CourseId,
bool IsActive,
string Status
);

public record PaymentRequest
(

Guid EnrollmentId,
Guid StudentId,
Guid CourseId
);

public record ReBackMonyResponse
(
Guid PaymentId,
decimal Amount
);