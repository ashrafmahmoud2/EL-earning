﻿using ELearning.Infrastructure.Base;
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
using Stripe;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ELearning.Service.Service;

public class PaymentService : BaseRepository<Payment>, IPaymentService
{


    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly StripeClient _stripeClient;

    public PaymentService(ApplicationDbContext context, IUnitOfWork unitOfWork, StripeClient stripeClient) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _stripeClient = stripeClient;
    }

    public async Task<Result<PaymentResponse>> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Repository<Payment>()
             .FirstOrDefaultAsync(
                 x => x.IsActive,
                 query => query.Include(p => p.CreatedBy)
                               .Include(p => p.Enrollment)
                                 .ThenInclude(e => e.course)
                               .Include(p => p.Enrollment)
                                 .ThenInclude(e => e.student)
                                   .ThenInclude(s => s.User),
                 cancellationToken);

        if (payment is null)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentNotFound);

        return Result.Success(payment.Adapt<PaymentResponse>());
    }

    public async Task<IEnumerable<PaymentResponse>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _unitOfWork.Repository<Payment>()
            .FindAsync(
                x => x.IsActive,
                query => query.Include(p => p.CreatedBy)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.course)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.student)
                                  .ThenInclude(s => s.User),
                cancellationToken);

        return payments.Adapt<IEnumerable<PaymentResponse>>();
    }

    public async Task<Result<IEnumerable<PaymentResponse>>> GetAllPaymentsForStudentAsync(string userId, CancellationToken cancellationToken)
    {
        var payments = await _unitOfWork.Repository<Payment>().FindAsync(x => x.Enrollment.student.UserId == userId &&
        x.IsActive && x.Enrollment.student.IsActive,
        query => query.Include(p => p.CreatedBy)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.course)
                              .Include(p => p.Enrollment)
                                .ThenInclude(e => e.student)
                                  .ThenInclude(s => s.User),
                cancellationToken);


        var paymentResponses = payments.Adapt<IEnumerable<PaymentResponse>>();
        return Result.Success(paymentResponses);

    }

    public async Task<Result> CreatePaymentAsync(PaymentRequest request, string paymentStatus, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Enrollment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId))
            return Result.Failure<PaymentResponse>(EnrollmentErrors.EnrollmentNotFound);

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

        var course = await _unitOfWork.Repository<Course>().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.IsActive);

        var paymentIntentService = new PaymentIntentService(_stripeClient);
        var paymentIntentOptions = new PaymentIntentCreateOptions
        {
            Amount = (long)(course.Price * 100),  // Amount is in cents
            Currency = "usd",  // Or the currency you're using
            Metadata = new Dictionary<string, string>
            {
                { "EnrollmentId", request.EnrollmentId.ToString() },
                { "CourseId", request.CourseId.ToString() }
            }
        };

        var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

        var payment = new Payment
        {
            Amount = course.Price,
            Status = paymentStatus,
            EnrollmentId = request.EnrollmentId,
            StripePaymentIntentId = paymentIntent.Id,
        };

        await _unitOfWork.Repository<Payment>().AddAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(paymentIntent.ClientSecret);
    }

    public async Task<Result<PaymentResponse>> UpdatePaymentAsync(Guid paymentId, string paymentStatus, PaymentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Enrollment>().AnyAsync(x => x.EnrollmentId == request.EnrollmentId))
            return Result.Failure<PaymentResponse>(EnrollmentErrors.EnrollmentNotFound);

        var payment = await _unitOfWork.Repository<Payment>()
                   .FirstOrDefaultAsync(
                       x => x.PaymentId == paymentId && x.IsActive,
                       query => query.Include(p => p.CreatedBy)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.course)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.student)
                                         .ThenInclude(s => s.User),
                       cancellationToken);
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

        payment.Status = paymentStatus;

        await _unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(payment.Adapt<PaymentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _unitOfWork.Repository<Payment>()
                   .FirstOrDefaultAsync(
                       x => x.PaymentId == id && x.IsActive,
                       query => query.Include(p => p.CreatedBy)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.course)
                                     .Include(p => p.Enrollment)
                                       .ThenInclude(e => e.student)
                                         .ThenInclude(s => s.User),
                       cancellationToken);

        if (payment is null)
            return Result.Failure(PaymentErrors.PaymentNotFound);

        payment.IsActive = !payment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<ReBackMonyResponse>> RefundPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {

        var payment = await _unitOfWork.Repository<Payment>()
                                         .FirstOrDefaultAsync(x => x.PaymentId == paymentId && x.IsActive,
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



}



