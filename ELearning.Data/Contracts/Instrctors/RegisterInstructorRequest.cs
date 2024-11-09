using ELearning.Data.Contracts.Auth;

namespace ELearning.Data.Contracts.Instrctors;

public record RegisterInstructorRequest
(
  RegisterRequest RegisterRequest,
  InstructorRequest InstructorRequest
);





