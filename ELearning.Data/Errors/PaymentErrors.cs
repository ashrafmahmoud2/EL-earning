using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record PaymentErrors
{
    public static readonly Error  PaymentNotFound =
        new(" Payment. PaymentNotFound", " Payment is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedPayment =
        new(" Payment.Duplicated Payment", "Another  Payment with the same name is already exists", StatusCodes.Status409Conflict);

    
    public static readonly Error RefundedPayment =
        new(" Payment.Refunded Payment", "Cannot perform any action on a refunded payment.", StatusCodes.Status409Conflict);

    public static readonly Error CanceledPayment =
       new("Payment.CanceledPayment", "Cannot perform actions on a canceled Payment.", StatusCodes.Status409Conflict);




}
