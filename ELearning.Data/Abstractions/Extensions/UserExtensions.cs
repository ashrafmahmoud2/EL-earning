using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Abstractions.Extensions;
public static class UserExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
         return user.FindFirstValue(ClaimTypes.NameIdentifier);
      //  return "01930804-c3e6-762d-af42-29b14ebf9757";
    }
}