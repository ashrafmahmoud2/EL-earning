using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Question;
public record QuestionResponse
(
    Guid QuestionId,
    string Text,
    int OrderIndex,
    bool IsActive,
    Guid QuizId
);
