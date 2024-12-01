using ELearning.Data.Consts;
using ELearning.Data.Contracts.Payment;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = UserRole.Admin)]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    //[HttpPost("create-payment")]
    //public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
    //{
    //    var result = await _paymentService.CreatePaymentAsync(paymentRequest, PaymentStatus.Completed);

    //    return result.IsSuccess ? Ok(result) : result.ToProblem();    
   
    //}
}
