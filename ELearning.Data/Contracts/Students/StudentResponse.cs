using ELearning.Data.Contracts.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Students;
public record StudentResponse
(
    Guid StudentId,
    string StudentName,  
    string CreatedBy,
    DateTime CreatedOn, 
    string Email,
bool IsActive
);


