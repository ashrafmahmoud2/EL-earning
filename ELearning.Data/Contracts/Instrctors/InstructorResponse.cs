using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Instrctors;
public record InstructorResponse
(
    Guid InstructorId,
    string InstructorName,
    string CreatedBy,
    DateTime CreatedOn,
    string Email,
    bool IsActive
);


