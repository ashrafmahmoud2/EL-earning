using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.QuizAttempt;
public record QuizAttemptResponse
(
    Guid QuizAttemptId,
    DateTime AttemptDate,
    Guid QuizId,
    //string QuizTitle
    Guid StudentId,
    //strin StudentName
    bool IsPassed,
    int Score,
    bool IsActive
);
