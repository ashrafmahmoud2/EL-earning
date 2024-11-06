using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Roles;
public record RoleResponse(
    string Id,
    string Name,
    bool IsDeleted
);
