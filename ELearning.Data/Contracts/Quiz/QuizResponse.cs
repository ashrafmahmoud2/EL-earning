using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Quiz;
public record QuizResponse
(
    Guid QuizId ,
    string Title,
    string? Description,
    Guid LessonId,
    string LessoneName,
    string CreatedBy,
    bool IsActive
);
