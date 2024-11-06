namespace ELearning.Data.Contracts.Auth;

public record ConfirmEmailRequest
(
   string UserId,
   string Code
 );
