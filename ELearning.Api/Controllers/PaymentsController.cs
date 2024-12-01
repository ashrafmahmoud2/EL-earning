namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = UserRole.Admin)]
public class PaymentsController(IPaymentService PaymentService) : ControllerBase
{
      private readonly IPaymentService _PaymentService = PaymentService;

    [HttpGet("{id}")]

    public async Task<IActionResult> GetPaymentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Payment = await _PaymentService.GetPaymentByIdAsync(id, cancellationToken);

        return Payment.IsSuccess ? Ok(Payment.Value) : Payment.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllPayments()
    {
        var Payment = await _PaymentService.GetAllPaymentsAsync();

        return Ok(Payment);
    }

    //[HttpPost("")]
    //public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    //{
    //    var payment = await _PaymentService.CreatePaymentAsync(request,PaymentStatus.Completed, cancellationToken);

    //    return payment.IsSuccess ?Ok(payment.Value) : payment.ToProblem();
    //}

 
    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdatePayment([FromRoute] Guid id, [FromBody] PaymentRequest request, CancellationToken cancellationToken)
    //{
    //    var coures =await  _PaymentService.UpdatePaymentAsync(id,PaymentStatus. request, cancellationToken);

    //    return coures.IsSuccess ? NoContent() : coures.ToProblem();
    //}

    //[HttpPut("refund_payment/{id}")]
    //public async Task<IActionResult> RefundPayment([FromRoute] Guid id, CancellationToken cancellationToken)
    //{
    //    var coures = await _PaymentService.RefundPaymentAsync(id,  cancellationToken);

    //    return coures.IsSuccess ? NoContent() : coures.ToProblem();
    //}

    //[HttpPut("Toggle_status{id}")]
    //public async Task<IActionResult> ToggleStatusPayment([FromRoute] Guid id, CancellationToken cancellationToken)
    //{
    //    var Instructor = await _PaymentService.ToggleStatusAsync(id, cancellationToken);

    //    return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    //}



}
