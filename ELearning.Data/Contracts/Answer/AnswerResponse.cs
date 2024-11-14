using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Answer;
public record AnswerResponse
(
    Guid AnswerId ,
    string Text,
    bool IsCorrect,
    bool IsActive,
    Guid QuestionId
);
