using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Answer;

namespace ELearning.Service.IService;

public interface IAnswerService
{
    Task<Result<AnswerResponse>> GetAnswerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AnswerResponse>> GetAllAnswersAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateAnswerAsync(AnswerRequest request, CancellationToken cancellationToken = default);
    Task<Result<AnswerResponse>> UpdateAnswerAsync(Guid id, AnswerRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default);
}

