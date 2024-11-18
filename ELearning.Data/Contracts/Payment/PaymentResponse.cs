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
string StudentName,
Guid CourseId,
string CourseTitle,
bool IsActive,
string Status,
Guid EnrollmentId,
string CreatedBy
);
