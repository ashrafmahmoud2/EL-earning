using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Lesson;
public record LessonResponse
(
    Guid LessonId,
    string Title ,
    string Description,
    int OrderIndex,
    bool IsActive,
    Guid SectionId
);

public record LessonRequest
(
    string Title,
    string Description,
    Guid SectionId
);
