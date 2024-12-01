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
using ELearning.Data.Contracts.Answer;
using System.ComponentModel;
using ELearning.Data.Contracts.Comment;
using Stripe.Forwarding;
namespace ELearning.Service.Service;

public class EnrollmentService : BaseRepository<Enrollment>, IEnrollmentService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);


    public EnrollmentService(ApplicationDbContext context, IUnitOfWork unitOfWork, IPaymentService paymentService, ICacheService cacheService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
        _cacheService = cacheService;
    }

    public async Task<Result<EnrollmentResponse>> GetEnrollmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
      .FirstOrDefaultAsync(x => x.EnrollmentId == id && x.IsActive,
          include: q => q.Include(e => e.student)
                          .ThenInclude(s => s.User)
                          .Include(e => e.course)
                         .Include(e => e.CreatedBy),
          cancellationToken);


        if (enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);

        var enrollmentResponse = enrollment.Adapt<EnrollmentResponse>();

        return Result.Success(enrollmentResponse);
    }

    public async Task<Result<EnrollmentResponse>> GetEnrollmentByCourseId(Guid id, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                     .FirstOrDefaultAsync(x => x.CourseId == id && x.IsActive,
                                        include: q => q.Include(s => s.student)
                                                  .ThenInclude(x => x.User)
                                                  .Include(s => s.course)
                                                  .Include(s => s.CreatedBy),
                                                  cancellationToken);

        if (enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);

        var EnrollmentResponse = enrollment.Adapt<EnrollmentResponse>();

        return Result.Success(EnrollmentResponse);
    }

    public async Task<Result<EnrollmentResponse>> GetEnrollmentByStudentId(Guid id, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                     .FirstOrDefaultAsync(x => x.StudentId == id && x.IsActive,
                                        include: q => q.Include(s => s.student)
                                                  .ThenInclude(x => x.User)
                                                  .Include(s => s.course)
                                                  .Include(s => s.CreatedBy),
                                                  cancellationToken);

        if (enrollment is null)
            return Result.Failure<EnrollmentResponse>(EnrollmentErrors.EnrollmentNotFound);

        return Result.Success(enrollment.Adapt<EnrollmentResponse>());
    }

    public async Task<Result<IEnumerable<EnrollmentResponse>>> GetAllEnrollmentsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Enrollments:GetAll";


        // Check if data is in the cache
        var cachedResult = await _cacheService.GetCacheAsync<IEnumerable<EnrollmentResponse>>(cacheKey);

        if (cachedResult.IsSuccess && cachedResult.Value != null)      
            return Result.Success(cachedResult.Value);
        

        if (cachedResult.IsFailure && cachedResult.Error != CashErrors.NotFound)
            return Result.Failure<IEnumerable<EnrollmentResponse>>(cachedResult.Error);





        // Retrieve enrollments from the database
        var enrollments = await _unitOfWork.Repository<Enrollment>()
                .FindAsync(
                    s => s.IsActive,
                    include: query => query.Include(e => e.student)
                                           .ThenInclude(s => s.User)
                                           .Include(e => e.course)
                                           .Include(e => e.CreatedBy),
                    cancellationToken: cancellationToken
                );

        // Adapt enrollments to EnrollmentResponse
        var enrollmentResponses = enrollments.Adapt<IEnumerable<EnrollmentResponse>>();

        // Cache the adapted response
        await _cacheService.SetCacheAsync(cacheKey, enrollmentResponses, _cacheDuration);

        return Result.Success(enrollmentResponses);
    }

    public async Task<Result<IEnumerable<EnrollmentResponse>>> GetEnrollmentCoursesForStudentAsync(string userId, CancellationToken cancellationToken)
    {
        // Check if the user exists
        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == userId))
        {
            return Result.Failure<IEnumerable<EnrollmentResponse>>(UserErrors.UserNotFound);
        }

        var enrollments = await _unitOfWork.Repository<Enrollment>()
            .FindAsync(
                x => x.student.UserId == userId
                     && x.IsActive
                     && x.student.IsActive,
                q => q.Include(e => e.student)
                      .ThenInclude(s => s.User)
                      .Include(e => e.course)
                      .Include(e => e.CreatedBy),
                cancellationToken // pass the CancellationToken here
            );






        if (enrollments == null || !enrollments.Any())
        {
            return Result.Failure<IEnumerable<EnrollmentResponse>>(EnrollmentErrors.EnrollmentNotFound);
        }

        var mappedEnrollments = enrollments.Adapt<IEnumerable<EnrollmentResponse>>();

        return Result.Success(mappedEnrollments);
    }


    public async Task<Result<EnrollmentResponse>> CreateEnrollmentAsync(EnrollmentAddRequest request, string EnrollmentStatus, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Student>().AnyAsync(x => x.StudentId == request.StudentId))
            return Result.Failure<EnrollmentResponse>(StudentsErrors.NotFound);

        if (!await _unitOfWork.Repository<Course>().AnyAsync(x => x.CourseId == request.CourseId))
            return Result.Failure<EnrollmentResponse>(CoursesErrors.NotFound);


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


        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Enrollments:GetAll");

        return Result.Success(enrollment.Adapt<EnrollmentResponse>());
    }



    public async Task<Result<EnrollmentResponse>> UpdateEnrollmentAsync(Guid enrollmentId, EnrollmentUpdateRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Student>().AnyAsync(x => x.StudentId == request.StudentId))
            return Result.Failure<EnrollmentResponse>(StudentsErrors.NotFound);

        if (!await _unitOfWork.Repository<Course>().AnyAsync(x => x.CourseId == request.CourseId))
            return Result.Failure<EnrollmentResponse>(CoursesErrors.NotFound);

        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                          .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.IsActive,
                                             include: q => q.Include(s => s.student)
                                                       .ThenInclude(x => x.User)
                                                       .Include(s => s.course)
                                                       .Include(s => s.CreatedBy),
                                                       cancellationToken);

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


        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Enrollments:GetAll");

        return Result.Success(enrollment.Adapt<EnrollmentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                           .FirstOrDefaultAsync(x => x.EnrollmentId == id && x.IsActive);

        if (enrollment is null)
            return Result.Failure(EnrollmentErrors.EnrollmentNotFound);

        enrollment.IsActive = !enrollment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);


        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Enrollments:GetAll");

        return Result.Success();
    }

    public async Task<Result> RefundEnrollmentAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.Repository<Enrollment>()
                                           .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.IsActive,
                                              include: q => q.Include(s => s.student)
                                                        .ThenInclude(x => x.User)
                                                        .Include(s => s.course)
                                                        .Include(s => s.CreatedBy),
                                                        cancellationToken);

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


        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Enrollments:GetAll");


        return Result.Success();
    }




}



