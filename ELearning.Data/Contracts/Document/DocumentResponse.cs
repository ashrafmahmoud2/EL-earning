using ELearning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Document;
public record DocumentResponse
(
    Guid DocumentId,
    string Title,
    string? Description,
    string DocumentPath,
    bool IsActive,
    Guid LessonId,
    string LessonTitle,
    string CreatedBy
 );


