using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Enrollment;
using ELearning.Data.Errors;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Payment;
using ELearning.Data.Consts;
using Azure.Core;
namespace ELearning.Service.Service;

public class EnrollmentService : BaseRepository<Enrollment>, IEnrollmentService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public EnrollmentService(ApplicationDbContext context, IUnitOfWork unitOfWork, IPaymentService paymentService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Result<EnrollmentResponse>> GetEnrollmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Enrollments = await _unitOfWork.Repository<Enrollment>()
                                         .FindAsync(x => x.EnrollmentId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Enrollment = Enrollments.FirstOrDefault();

        if (Enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);

        var EnrollmentResponse = Enrollment.Adapt<EnrollmentResponse>();

        return Result.Success(EnrollmentResponse);
    }

    public async Task<Result<EnrollmentResponse>> CreateEnrollmentAsync(EnrollmentAddRequest request, string EnrollmentStatus, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);

        bool isDuplicate = await _unitOfWork.Repository<Enrollment>()
        .AnyAsync(x => x.StudentId == request.StudentId && x.CourseId == request.CourseId, cancellationToken);

        if (isDuplicate)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.DuplicatedEnrollment);



        var enrollment = request.Adapt<Enrollment>();

        enrollment.Status = EnrollmentStatus;

        await _unitOfWork.Repository<Enrollment>().AddAsync(enrollment, cancellationToken);

        // Check if changes were successfully saved
        var changes = await _unitOfWork.CompleteAsync(cancellationToken);
        if (changes <= 0)
        {
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentAddFailed);
        }

        // If enrollment added successfully, proceed with payment
        await _paymentService.CreatePaymentAsync(new PaymentRequest(enrollment.EnrollmentId, enrollment.StudentId, enrollment.CourseId), PaymentStatus.Completed, cancellationToken);

        return Result.Success(enrollment.Adapt<EnrollmentResponse>());
    }

    public async Task<IEnumerable<EnrollmentResponse>> GetAllEnrollmentsAsync(CancellationToken cancellationToken = default)
    {
        var enrollments = await _unitOfWork.Repository<Enrollment>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.student)
                               .Include(s => s.course),
                cancellationToken: cancellationToken);


        return enrollments.Select(s => new EnrollmentResponse(
                EnrollmentId: s.EnrollmentId,
                IsActive: s.IsActive,
                StudentId: s.student.StudentId,
                StudentName: s.student.User.FirstName,
                CourseId: s.course.CourseId,
                CourseName: s.course.Title,
                EnrolledAt: s.enrolledAt,
                CompletedAt: s.completedAt,
                Status: s.Status
            )).ToList();
    }

    public async Task<Result<EnrollmentResponse>> UpdateEnrollmentAsync(Guid enrollmentId, EnrollmentUpdateRequest request, CancellationToken cancellationToken = default)
    {

        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                         .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

        if (enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);


        var payment = await _unitOfWork.Repository<Payment>()
                               .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

        if (payment is null)
            return Result.Failure<EnrollmentResponse>(PaymentErrors.PaymentNotFound);


        if (enrollment.Status == EnrollmentStatus.CanceledForNewStudent || enrollment.Status == EnrollmentStatus.CanceledForNewCourse)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.CanceledEnrollment);

        if (enrollment.Status == EnrollmentStatus.Refunded)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.RefundedEnrollment);



        if (enrollment.StudentId == request.StudentId && enrollment.CourseId == request.CourseId)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.DuplicateEnrollmentData);



        if (enrollment.StudentId == request.StudentId && enrollment.CourseId != request.CourseId)
        {
            enrollment.Status = EnrollmentStatus.CanceledForNewCourse;
            await CreateEnrollmentAsync(new EnrollmentAddRequest(request.StudentId, request.CourseId),
                EnrollmentStatus.ReplacedByNewCourse,
                cancellationToken);


            await _paymentService.UpdatePaymentAsync(payment.PaymentId, PaymentStatus.CanceledForDifferentCourse,
                new PaymentRequest(enrollmentId, enrollment.StudentId, enrollment.CourseId),
                cancellationToken);


        }
        else if (enrollment.CourseId == request.CourseId && enrollment.StudentId != request.StudentId)
        {
            enrollment.StudentId = request.StudentId;
            enrollment.Status = EnrollmentStatus.CanceledForNewStudent;
            await CreateEnrollmentAsync(new EnrollmentAddRequest(request.StudentId, request.CourseId),
                EnrollmentStatus.ReplacedByNewStudent,
                cancellationToken);


            await _paymentService.UpdatePaymentAsync(payment.PaymentId, PaymentStatus.CanceledForDifferentStudent,
                           new PaymentRequest(enrollmentId, enrollment.StudentId, enrollment.CourseId),
                           cancellationToken);

        }




        await _unitOfWork.Repository<Enrollment>().UpdateAsync(enrollment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(enrollment.Adapt<EnrollmentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Enrollments = await _unitOfWork.Repository<Enrollment>()
                                           .FindAsync(x => x.EnrollmentId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Enrollment = Enrollments.FirstOrDefault();

        if (Enrollment is null)
            return Result.Failure(EnrollmentErrors.EnrollmentNotFound);

        Enrollment.IsActive = !Enrollment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RefundEnrollmentAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                         .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

        if (enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);


        var payment = await _unitOfWork.Repository<Payment>()
                               .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

        if (payment is null)
            return Result.Failure<EnrollmentResponse>(PaymentErrors.PaymentNotFound);


        if (enrollment.Status == EnrollmentStatus.CanceledForNewStudent || enrollment.Status == EnrollmentStatus.CanceledForNewCourse)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.CanceledEnrollment);

        if (enrollment.Status == EnrollmentStatus.Refunded)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.RefundedEnrollment);




        enrollment.Status = EnrollmentStatus.Refunded;
        if (await _unitOfWork.CompleteAsync(cancellationToken) >= 0)
        {
            await _paymentService.UpdatePaymentAsync(payment.PaymentId, PaymentStatus.Refunded,
                                      new PaymentRequest(enrollmentId, enrollment.StudentId, enrollment.CourseId),
                                      cancellationToken);
        }


        return Result.Success();
    }
}



