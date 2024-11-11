using ELearning.Data.Contracts.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Course;
public record CourseRequest(
    string Title,
    string Description,
    bool IsActive,
    string ShortDescription,
    decimal Price,
    Guid InstructorId,
    Guid CategoryId,
    string ThumbnailUrl,
    string TrailerVideoUrl,
    string Level,
    List<string> Prerequisites,
    List<string> LearningObjectives,
    TimeSpan TotalTime
);






