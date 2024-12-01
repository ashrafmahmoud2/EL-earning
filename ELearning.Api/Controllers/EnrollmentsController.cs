using ELearning.Data.Consts;
using ELearning.Data.Contracts.Enrollment;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.RateLimiting;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.UserLimiter)]
[Authorize(Roles = UserRole.Admin)]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<IActionResult> GetEnrollmentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id, cancellationToken);
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem();
    }

    [HttpGet("course/{id}")]
    public async Task<IActionResult> GetEnrollmentByCourseId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByCourseId(id, cancellationToken);
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem();
    }

    [HttpGet("student/{id}")]
    public async Task<IActionResult> GetEnrollmentByStudentId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByStudentId(id, cancellationToken);
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var enrollment = await _enrollmentService.GetAllEnrollmentsAsync();
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem() ;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentAddRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.CreateEnrollmentAsync(request, EnrollmentStatus.Completed, cancellationToken);
        return enrollment.IsSuccess ? Created() : enrollment.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnrollment([FromRoute] Guid id, [FromBody] EnrollmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.UpdateEnrollmentAsync(id, request, cancellationToken);
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem();
    }

    [HttpPut("refund_payment/{id}")]
    public async Task<IActionResult> RefundPayment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.RefundEnrollmentAsync(id, cancellationToken);

        return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    }


    [HttpPut("change-owner/{id}")]
    public async Task<IActionResult> ChangeEnrollmentOwner([FromRoute] Guid id, [FromBody] EnrollmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.UpdateEnrollmentAsync(id, request, cancellationToken);
        return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    }

    [HttpPut("replace-with-new-course/{id}")]
    public async Task<IActionResult> ReplaceEnrollmentWithNewCourse([FromRoute] Guid id, [FromBody] EnrollmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.UpdateEnrollmentAsync(id, request, cancellationToken);
        return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    }

    //[HttpPut("toggle-status/{id}")]
    //public async Task<IActionResult> ToggleStatusEnrollment([FromRoute] Guid id, CancellationToken cancellationToken)
    //{
    //    var enrollment = await _enrollmentService.ToggleStatusAsync(id, cancellationToken);
    //    return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    //}
}
