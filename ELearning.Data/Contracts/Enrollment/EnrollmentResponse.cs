using ELearning.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Enrollment;
public record EnrollmentResponse(
    Guid EnrollmentId,
    bool IsActive,
    Guid StudentId,
   string StudentName,
    Guid CourseId,
    string CourseName,
    DateTime EnrolledAt,
    DateTime? CompletedAt,
    string Status
);

