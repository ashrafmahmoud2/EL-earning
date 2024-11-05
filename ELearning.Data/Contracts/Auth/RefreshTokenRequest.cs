namespace ELearning.Data.Contracts.Auth;

public record RefreshTokenRequest
(
    string Token,
    string RefreshToken
);