using ELearning.Data.Consts;
using ELearning.Data.Contracts.Enrollment;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEnrollmentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id, cancellationToken);
        return enrollment.IsSuccess ? Ok(enrollment.Value) : enrollment.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var enrollment = await _enrollmentService.GetAllEnrollmentsAsync();
        return Ok(enrollment);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentAddRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.CreateEnrollmentAsync(request, EnrollmentStatus.Completed, cancellationToken);
        return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnrollment([FromRoute] Guid id, [FromBody] EnrollmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.UpdateEnrollmentAsync(id, request, cancellationToken);
        return enrollment.IsSuccess ? NoContent() : enrollment.ToProblem();
    }

    [HttpPut("refund_payment/{id}")]
    public async Task<IActionResult> RefundPayment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var coures = await _enrollmentService.RefundEnrollmentAsync(id, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
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
