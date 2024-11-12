using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Quiz;

namespace ELearning.Service.IService;

public interface IQuizService
{
    Task<Result<QuizResponse>> GetQuizByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<QuizResponse>> GetAllQuizsAsync(CancellationToken cancellationToken = default);
    Task<Result<QuizResponse>> CreateQuizAsync(QuizRequest request, CancellationToken cancellationToken = default);
    Task<Result<QuizResponse>> UpdateQuizAsync(Guid id, QuizRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

