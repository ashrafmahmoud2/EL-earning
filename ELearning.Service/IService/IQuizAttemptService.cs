using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.QuizAttempt;

namespace ELearning.Service.IService;

public interface IQuizAttemptService
{
    Task<Result<QuizAttemptResponse>> GetQuizAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuizAttemptResponse>>> GetAllQuizAttemptsAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateQuizAttemptAsync(QuizAttemptRequest request, CancellationToken cancellationToken = default);
    Task<Result<QuizAttemptResponse>> UpdateQuizAttemptAsync(Guid id, QuizAttemptRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

