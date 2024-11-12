using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Lesson;

namespace ELearning.Service.IService;

public interface ILessonService
{
    Task<Result<LessonResponse>> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonResponse>> GetAllLessonsAsync(CancellationToken cancellationToken = default);
    Task<Result<LessonResponse>> CreateLessonAsync(LessonRequest request, CancellationToken cancellationToken = default);
    Task<Result<LessonResponse>> UpdateLessonAsync(Guid id, LessonRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

