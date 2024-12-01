using System.Threading;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService,IEnrollmentService enrollmentService,IPaymentService paymentService) : ControllerBase
{
    // await _userManager.Users it like await _context.Users
    //it's better to use _context then _userManger

    private readonly IUserService _userService = userService;
    private readonly IEnrollmentService _enrollmentService = enrollmentService;
    private readonly IPaymentService _paymentService = paymentService;

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("info")]
    public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpGet("enrollment-courses")]
    public async Task<IActionResult> GetEnrollmentCoursesForStudent(CancellationToken cancellationToken)
    {
        var result = await _enrollmentService.GetEnrollmentCoursesForStudentAsync(User.GetUserId()!, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("payments")]
    public async Task<IActionResult> GetAllPaymentsHeHasMade(CancellationToken cancellationToken)
    {
        var result = await _paymentService.GetAllPaymentsForStudentAsync(User.GetUserId()!,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


}
