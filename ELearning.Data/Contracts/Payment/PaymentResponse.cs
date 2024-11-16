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
//guid EnrollmentId
//strin StudentName
//strin QouresName
);
