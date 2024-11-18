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
using ELearning.Data.Contracts.Answer;
using Azure.Core;
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
        var payment = await _unitOfWork.Repository<Payment>()
             .FirstOrDefaultAsync(
                 _ => true,
                 query => query.Include(p => p.CreatedBy)
                               .Include(p => p.Enrollment)
                                 .ThenInclude(e => e.course)
                               .Include(p => p.Enrollment)
                                 .ThenInclude(e => e.student)
                                   .ThenInclude(s => s.User),
                 cancellationToken);

        if (payment is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.NotFound);

        return Result.Success(payment.Adapt<PaymentResponse>());
    }

    public async Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _unitOfWork.Repository<Payment>()
            .FindAsync(
                _ => true,
                query => query.Include(p => p.CreatedBy)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.course)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.student)
                                  .ThenInclude(s => s.User),
                cancellationToken);

        return payments.Adapt<IEnumerable<PaymentResponse>>();
    }

    public async Task<Result<IEnumerable<PaymentResponse>>> GetAllPaymentsForStudentAsync(string userId)
    {
        var payments = await _context.Students
            .Where(s => s.User.Id == userId && s.IsActive) // Filter by userId and active student
            .SelectMany(s => s.Enrollments) // Flatten the enrollments
            .Where(e => e.IsActive) // Ensure active enrollments
            .SelectMany(e => e.Payments) // Flatten the payments for each enrollment
            .Where(p => p.IsActive) // Ensure active payments
            .ToListAsync();


        var paymentResponses = payments.Adapt<IEnumerable<PaymentResponse>>();
        return Result.Success(paymentResponses);

    }

    public async Task<Result> CreatePaymentAsync(PaymentRequest request, string paymentStatus, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Enrollment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId))
            return Result.Failure<PaymentResponse>(EnrollmentErrors.NotFound);

        if (request is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.NotFound);

        var enrollment = await _unitOfWork.Repository<Enrollment>().FirstOrDefaultAsync(x => x.EnrollmentId == request.EnrollmentId);

        if (enrollment is null)
            return Result.Failure<PaymentResponse>(EnrollmentErrors.NotFound);

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

        // var result = new PaymentResponse(payment.PaymentId, payment.PaymentDate, payment.Amount, payment.Enrollment.StudentId, payment.Enrollment.CourseId, payment.IsActive, payment.Status);
        
        return Result.Success();
    }

    public async Task<Result<PaymentResponse>> UpdatePaymentAsync(Guid paymentId, string paymentStatus, PaymentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Enrollment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId))
            return Result.Failure<PaymentResponse>(EnrollmentErrors.NotFound);

        var payment = await _unitOfWork.Repository<Payment>()
                   .FirstOrDefaultAsync(
                       x => x.PaymentId == paymentId,
                       query => query.Include(p => p.CreatedBy)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.course)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.student)
                                         .ThenInclude(s => s.User),
                       cancellationToken);
        if (payment is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.NotFound);

        if (payment.Status == PaymentStatus.Refunded)
            return Result.Failure<PaymentResponse>(PaymentErrors.RefundedPayment);

        if (payment.Status == PaymentStatus.CanceledForDifferentCourse || payment.Status == PaymentStatus.CanceledForDifferentStudent)
            return Result.Failure<PaymentResponse>(PaymentErrors.CanceledPayment);

        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                           .FirstOrDefaultAsync(x => x.EnrollmentId == request.EnrollmentId);

        if (enrollment is null)
            return Result.Failure<PaymentResponse>(EnrollmentErrors.NotFound);

        payment.Status = paymentStatus;

        await _unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(payment.Adapt<PaymentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Repository<Payment>()
                   .FirstOrDefaultAsync(
                       x => x.PaymentId == id,
                       query => query.Include(p => p.CreatedBy)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.course)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.student)
                                         .ThenInclude(s => s.User),
                       cancellationToken);

        if (payment is null)
            return Result.Failure(PaymentErrors.NotFound);

        payment.IsActive = !payment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<ReBackMonyResponse>> RefundPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
       
        var payment = await _unitOfWork.Repository<Payment>()
                                         .FirstOrDefaultAsync(x => x.PaymentId == paymentId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (payment is null)
            return Result.Failure<ReBackMonyResponse>(PaymentErrors.NotFound);

        if (payment.Status == PaymentStatus.Refunded)
            return Result.Failure<ReBackMonyResponse>(PaymentErrors.NotFound);


        payment.Status = PaymentStatus.Refunded;

        await _unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(payment.Adapt<ReBackMonyResponse>());
    }



}



