namespace ELearning.Data.Contracts.Auth;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);
