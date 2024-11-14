using Microsoft.AspNetCore.Authorization;

namespace ELearning.Data.Authentication.Filters;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
