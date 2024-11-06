namespace ELearning.Data.Contracts.Users;

public record ChangePasswordRequest
(
    string CurrentPassword,
    string NewPassword
);