using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Instrctors;
using ELearning.Data.Contracts.Students;
using ELearning.Data.Entities;

namespace ELearning.Service.IService;

public interface IInstructorService
{
    Task<Result<InstructorResponse>> GetInstructorByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<InstructorResponse>>> GetAllInstructorsAsync(CancellationToken cancellationToken = default); 
    Task<Result<InstructorResponse>> CreateInstructorAsync(ApplicationUser user,InstructorRequest request, CancellationToken cancellationToken = default); 
    Task<Result> UpdateInstructorAsync(Guid id, InstructorRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default); 
}

