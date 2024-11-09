using ELearning.Data.Entities;

namespace ELearning.Data.Contracts.Students;

public record StudentRequest
(
   string FirstName,
   string LastName,
   string Email
);
