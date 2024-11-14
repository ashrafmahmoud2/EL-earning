using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Enrollment;

namespace ELearning.Service.IService;

public interface IEnrollmentService
{
    Task<Result<EnrollmentResponse>> GetEnrollmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<EnrollmentResponse>> GetEnrollmentByCourseId(Guid id, CancellationToken cancellationToken = default);
    Task<Result<EnrollmentResponse>> GetEnrollmentByStudentId(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EnrollmentResponse>> GetEnrollmentCoursesForStudentAsync(string userId);
    Task<IEnumerable<EnrollmentResponse>> GetAllEnrollmentsAsync(CancellationToken cancellationToken = default);
    Task<Result< EnrollmentResponse>> CreateEnrollmentAsync(EnrollmentAddRequest request, string EnrollmentStatus, CancellationToken cancellationToken = default);
    Task<Result< EnrollmentResponse>> UpdateEnrollmentAsync(Guid id, EnrollmentUpdateRequest  request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> RefundEnrollmentAsync(Guid id, CancellationToken cancellationToken = default);
}
