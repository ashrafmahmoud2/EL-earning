using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Course;

public record CourseResponse(
   Guid CourseId,
    string Title,
    string Description,
    bool IsActive,
    string ShortDescription,
    decimal Price,
    Guid InstructorId,
    string InstructorName,
    Guid CategoryId,
    string ThumbnailUrl,
    string TrailerVideoUrl,
    string Level,
    List<string> Prerequisites,
    List<string> LearningObjectives,
    TimeSpan? TotalTime  
);

