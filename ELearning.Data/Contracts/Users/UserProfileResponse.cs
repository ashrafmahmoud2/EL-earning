namespace ELearning.Data.Contracts.Users;

public record UserProfileResponse
(string Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled
);