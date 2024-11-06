using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Users;
public record UpdateProfileRequest
(

    string FirstName,
    string LastName,
    string Email


);
