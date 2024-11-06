namespace ELearning.Data.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Permissions
);
