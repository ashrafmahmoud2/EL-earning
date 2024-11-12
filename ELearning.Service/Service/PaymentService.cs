using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Payment;
using ELearning.Data.Errors;
using ELearning.Data.Consts;
namespace ELearning.Service.Service;

public class PaymentService : BaseRepository<Payment>, IPaymentService
{


    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaymentResponse>> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Payments = await _unitOfWork.Repository<Payment>()
                                         .FindAsync(x => x.PaymentId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Payment = Payments.FirstOrDefault();

        if (Payment is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentNotFound);

        var PaymentResponse = Payment.Adapt<PaymentResponse>();

        return Result.Success(PaymentResponse);
    }

    public async Task<Result<PaymentResponse>> CreatePaymentAsync(PaymentRequest request, string paymentStatus, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentNotFound);

        var enrollment = await _unitOfWork.Repository<Enrollment>().FirstOrDefaultAsync(x => x.EnrollmentId == request.EnrollmentId);

        if (enrollment is null)
            return Result.Failure<PaymentResponse>(EnrollmentErrors.EnrollmentNotFound);

        bool isDuplicatePayment = await _unitOfWork.Repository<Payment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId, cancellationToken);
        if (isDuplicatePayment)
            return Result.Failure<PaymentResponse>(EnrollmentErrors.DuplicatedEnrollment);


        if (await _unitOfWork.Repository<Payment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId))
            return Result.Failure<PaymentResponse>(EnrollmentErrors.DuplicatedEnrollment);

        var course = await _unitOfWork.Repository<Course>().FirstOrDefaultAsync(x => x.CourseId == request.CourseId);

        var payment = new Payment
        {
            Amount = course.Price,
            Status = paymentStatus,
            EnrollmentId = request.EnrollmentId
        };

        await _unitOfWork.Repository<Payment>().AddAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var result = new PaymentResponse(payment.PaymentId, payment.PaymentDate, payment.Amount, payment.Enrollment.StudentId, payment.Enrollment.CourseId, payment.IsActive, payment.Status);
        return Result.Success(result);
    }

    public async Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var Payments = await _unitOfWork.Repository<Payment>()
            .FindAsync(
                s => true,
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return Payments.Adapt<IEnumerable<PaymentResponse>>();
    }

    public async Task<Result<PaymentResponse>> UpdatePaymentAsync(Guid paymentId, string paymentStatus, PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Repository<Payment>().FirstOrDefaultAsync(x => x.PaymentId == paymentId);

        if (payment is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentNotFound);

        if (payment.Status == PaymentStatus.Refunded)
            return Result.Failure<PaymentResponse>(PaymentErrors.RefundedPayment);

        if (payment.Status == PaymentStatus.CanceledForDifferentCourse || payment.Status == PaymentStatus.CanceledForDifferentStudent)
            return Result.Failure<PaymentResponse>(PaymentErrors.CanceledPayment);

        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                           .FirstOrDefaultAsync(x => x.EnrollmentId == request.EnrollmentId);

        if (enrollment is null)
            return Result.Failure<PaymentResponse>(EnrollmentErrors.EnrollmentNotFound);

        // Additional logic can be added here as needed, like handling different student/course conditions.

        await _unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(payment.Adapt<PaymentResponse>());
    }


    public async Task<Result<ReBackMonyResponse>> RefundPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Repository<Payment>()
                                         .FirstOrDefaultAsync(x => x.PaymentId == paymentId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (payment is null)
            return Result.Failure<ReBackMonyResponse>(PaymentErrors.PaymentNotFound);

        if (payment.Status == PaymentStatus.Refunded)
            return Result.Failure<ReBackMonyResponse>(PaymentErrors.PaymentNotFound);


        payment.Status = PaymentStatus.Refunded;

        await _unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(payment.Adapt<ReBackMonyResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Payments = await _unitOfWork.Repository<Payment>()
                                           .FindAsync(x => x.PaymentId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Payment = Payments.FirstOrDefault();

        if (Payment is null)
            return Result.Failure(PaymentErrors.PaymentNotFound);

        Payment.IsActive = !Payment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}



