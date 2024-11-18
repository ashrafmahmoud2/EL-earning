using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Payment;

namespace ELearning.Service.IService;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PaymentResponse>>> GetAllPaymentsForStudentAsync(string userId);
    Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync(CancellationToken cancellationToken = default);
    Task<Result> CreatePaymentAsync(PaymentRequest request,string PaymentStatus, CancellationToken cancellationToken = default);
    Task<Result<PaymentResponse>> UpdatePaymentAsync(Guid id, string PaymentStatus,PaymentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ReBackMonyResponse>> RefundPaymentAsync(Guid paymentId,  CancellationToken cancellationToken = default);
}

