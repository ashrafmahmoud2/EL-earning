
using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Entities;
using ELearning.Data.Contracts.Course;
using ELearning.Data.Errors;
using ELearning.Data.Consts;
namespace ELearning.Service.Service;

public class CourseService : BaseRepository<Course>, ICourseService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;


    public CourseService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;

    }
    public async Task<Result<CourseResponse>> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {


        // Ensure the correct method signature for including related data and passing cancellation token
        //var course = await _unitOfWork.Repository<Course>()
        //                              .FindAsync(x => x.CourseId == courseId, include: x => x.Include(c => c.CreatedBy),  cancellationToken);
        var course = await _context.Courses
                                    .Where(x => x.CourseId == courseId)
                                    .FirstOrDefaultAsync();

        if (course is null)
            return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

        var courseResponse = course.Adapt<CourseResponse>();

        return Result.Success(courseResponse);
    }
    public async Task<Result<CourseResponse>> GetCourseBycategoryId(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var coures = await _unitOfWork.Repository<Course>().FindAsync(x => x.CategoryId == categoryId);


        if (coures is null)
            return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

        var courseResponse = coures.Adapt<CourseResponse>();

        return Result.Success(courseResponse);
    }
    public async Task<Result<CourseResponse>> GetCourseByinstructorId(Guid instructorId, CancellationToken cancellationToken = default)
    {
        var coures = await _unitOfWork.Repository<Course>().FindAsync(x => x.InstructorId == instructorId);


        if (coures is null)
            return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);

        var courseResponse = coures.Adapt<CourseResponse>();

        return Result.Success(courseResponse);
    }

    public async Task<Result<CourseResponse>> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            Result.Failure(CourseErrors.CourseNotFound);

        //if (await _categoryService.GetCategoryByIdAsync(request.CategoryId) == null ||
        //  await _instructorService.GetInstructorByIdAsync(request.InstructorId) == null)
        //{
        //    return Result.Failure(CourseErrors.CourseNotFound);
        //}



        var Course = request.Adapt<Course>();
        await _unitOfWork.Repository<Course>().AddAsync(Course, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(Course.Adapt<CourseResponse>());
    }

    public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _unitOfWork.Repository<Course>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.CreatedBy),
                cancellationToken: cancellationToken
            );

        var courseResponses = courses.Adapt<IEnumerable<CourseResponse>>();

        return courseResponses;
    }

    public async Task<Result> UpdateCourseAsync(Guid id, CourseRequest request, CancellationToken cancellationToken = default)
    {
    //need to fix Handling Concurrency Conflicts
    https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations
        var course = await _unitOfWork.Repository<Course>()
                                         .FirstOrDefaultAsync(x => x.CourseId == id,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (course is null)
            return Result.Failure<CourseResponse>(CourseErrors.CourseNotFound);


        course = request.Adapt<Course>();


        _unitOfWork.Repository<Course>().Attach(course);
        await _unitOfWork.Repository<Course>().UpdateAsync(course, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(course.Adapt<CourseResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Courses = await _unitOfWork.Repository<Course>()
                                           .FindAsync(x => x.CourseId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Course = Courses.FirstOrDefault();

        if (Course is null)
            return Result.Failure(CourseErrors.CourseNotFound);

        Course.IsActive = !Course.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<List<CourseSectionLessonCountResponse>>> GetCourseSectionLessonCounts(CancellationToken cancellationToken = default)
    {
        var courseSectionLessonCounts = await _context.Courses
            .Select(course => new CourseSectionLessonCountResponse(
                course.CourseId,
                course.sections.Count,
                course.sections.Select(section => new SectionLessonCountResponse(
                    section.SectionId,
                    section.Lessons.Count
                )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(courseSectionLessonCounts);
    }
   
    public async Task<Result<List<CourseEnrollmentCountResponse>>> CountEnrollmentsForCourses(CancellationToken cancellationToken = default)
    {
        var enrollments = await _unitOfWork.Repository<Enrollment>().GetAllAsync(cancellationToken);

        var courseEnrollmentCounts = enrollments
            .GroupBy(enrollment => enrollment.CourseId)
            .Select(group => new CourseEnrollmentCountResponse(
                group.Key,          // CourseId
                group.Count()       // Enrollment count
            ))
            .ToList();

        return Result.Success(courseEnrollmentCounts);
    }

    public async Task<Result<List<CourseRefundedCountResponse>>> GetCourseRefundedCountsAsync(CancellationToken cancellationToken = default)
    {
        // Fetch all refunded enrollments and include the associated Course
        var refundedEnrollmentsQuery = _unitOfWork.Repository<Enrollment>()
            .Find(x => x.Status == EnrollmentStatus.Refunded, include: q => q.Include(e => e.course));

        // Group by CourseId and project the results into CourseRefundedCountResponse
        var refundedEnrollmentCounts = await refundedEnrollmentsQuery
            .GroupBy(e => e.CourseId)
            .Select(g => new CourseRefundedCountResponse(
                g.Key,  // Pass CourseId as the first positional argument
                g.First().course.Title,  // Pass CourseTitle from the first course in the group
                g.Count()  // Pass the count of enrollments for this course
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(refundedEnrollmentCounts);
    }





}
