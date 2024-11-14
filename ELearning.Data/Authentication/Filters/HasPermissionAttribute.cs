using Microsoft.AspNetCore.Authorization;

namespace ELearning.Data.Authentication.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{

}