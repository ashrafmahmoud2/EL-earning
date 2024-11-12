using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Section;

namespace ELearning.Service.IService;

public interface ISectionService
{
    Task<Result<SectionResponse>> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SectionResponse>> GetAllSectionsAsync(CancellationToken cancellationToken = default);
    Task<Result<SectionResponse>> CreateSectionAsync(SectionRequest request, CancellationToken cancellationToken = default);
    Task<Result<SectionResponse>> UpdateSectionAsync(Guid id, SectionRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

