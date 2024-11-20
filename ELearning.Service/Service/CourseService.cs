
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
using ELearning.Data.Contracts.Answer;
using Hangfire;
using Stripe;
using Microsoft.Extensions.DependencyInjection;
namespace ELearning.Service.Service;

public class CourseService : BaseRepository<Course>, ICourseService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    public CourseService(ApplicationDbContext context, IUnitOfWork unitOfWork,IServiceProvider serviceProvider) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<CourseResponse>> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var course = await _unitOfWork.Repository<Course>()
                                      .FirstOrDefaultAsync(x => x.CourseId == courseId && x.IsActive,
                                      x => x.Include(c => c.CreatedBy)
                                     .Include(c => c.Instructor)
                                     .ThenInclude(i => i.User),
                                      cancellationToken);


        if (course is null)
            return Result.Failure<CourseResponse>(CoursesErrors.NotFound);

        return Result.Success(course.Adapt<CourseResponse>());
    }

    public async Task<Result<IEnumerable<CourseResponse>>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        // Fetch courses with related entities eagerly loaded
        var courses = await _unitOfWork.Repository<Course>()
            .FindAsync(
                x => x.IsActive, // Fetch all records
                include: query => query.Include(c => c.CreatedBy)
                                      .Include(c => c.Instructor)
                                        .ThenInclude(i => i.User),
                cancellationToken: cancellationToken
            );

        return Result.Success(courses.Adapt<IEnumerable<CourseResponse>>());
    }

    public async Task<Result> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default)
    {

        if (await _unitOfWork.Repository<Course>().AnyAsync(x => x.Title == request.Title))
            return Result.Failure<CourseResponse>(CoursesErrors.DuplicatedCourse);

        if (!await _unitOfWork.Repository<Instructor>().AnyAsync(x => x.InstructorId == request.InstructorId))
            return Result.Failure<CourseResponse>(InstructorsErrors.InstructorNotFound);

        if (!await _unitOfWork.Repository<Category>().AnyAsync(x => x.CategoryId == request.CategoryId))
            return Result.Failure<CourseResponse>(CategorysErrors.CategoryNotFound);




        if (request is null)
            Result.Failure(CoursesErrors.NotFound);

      



        var course = request.Adapt<Course>();

        BackgroundJob.Enqueue(() => AddCourseInBackground(course));

        return Result.Success();
    }

    public async Task<Result<CourseResponse>> UpdateCourseAsync(Guid id, CourseRequest request, CancellationToken cancellationToken = default)
    {


        if (!await _unitOfWork.Repository<Instructor>().AnyAsync(x => x.InstructorId == request.InstructorId))
            return Result.Failure<CourseResponse>(InstructorsErrors.InstructorNotFound);

        if (!await _unitOfWork.Repository<Category>().AnyAsync(x => x.CategoryId == request.CategoryId))
            return Result.Failure<CourseResponse>(CategorysErrors.CategoryNotFound);



        var course = await _unitOfWork.Repository<Course>()
                                          .FirstOrDefaultAsync(x => x.CourseId == id && x.IsActive,
                                          x => x.Include(c => c.CreatedBy)
                                         .Include(c => c.Instructor).ThenInclude(i => i.User),
                                          cancellationToken);

        if (course is null)
            return Result.Failure<CourseResponse>(CoursesErrors.NotFound);


    
        course = request.Adapt(course);

        BackgroundJob.Enqueue(() => UpdateCourseInBackground(course));

        return Result.Success(course.Adapt<CourseResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var course = await _unitOfWork.Repository<Course>()
                                           .FirstOrDefaultAsync(x => x.CourseId == id && x.IsActive);

        if (course is null)
            return Result.Failure(CoursesErrors.NotFound);

        course.IsActive = !course.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<CourseResponse>> GetCourseBycategoryId(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var course = await _unitOfWork.Repository<Course>()
                                  .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.IsActive,
                                  x => x.Include(c => c.CreatedBy)
                                 .Include(c => c.Instructor)
                                 .ThenInclude(i => i.User),
                                  cancellationToken);

        if (course is null)
            return Result.Failure<CourseResponse>(CoursesErrors.NotFound);

        var courseResponse = course.Adapt<CourseResponse>();

        return Result.Success(courseResponse);
    }
  
    public async Task<Result<CourseResponse>> GetCourseByinstructorId(Guid instructorId, CancellationToken cancellationToken = default)
    {

        var course = await _unitOfWork.Repository<Course>()
                                   .FirstOrDefaultAsync(x => x.InstructorId == instructorId && x.IsActive,
                                   x => x.Include(c => c.CreatedBy)
                                  .Include(c => c.Instructor)
                                  .ThenInclude(i => i.User),
                                   cancellationToken);

        if (course is null)
            return Result.Failure<CourseResponse>(CoursesErrors.NotFound);

        var courseResponse = course.Adapt<CourseResponse>();

        return Result.Success(courseResponse);
    }

    public async Task<Result<List<CourseSectionLessonCountResponse>>> GetCoursesStructure(CancellationToken cancellationToken = default)
    {
        var courseSectionLessonCounts = await _context.Courses
            .Where(x =>x.IsActive)
            .Select(course => new CourseSectionLessonCountResponse(
                course.CourseId,
                course.Title, // Ensure that the CourseName property exists
                course.sections.Count,
                course.sections.Select(section => new SectionLessonCountResponse(
                    section.SectionId,
                    section.Title, // Ensure that the SectionName property exists
                    section.Lessons.Count
                )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(courseSectionLessonCounts);
    }

    public async Task<Result<CourseSectionLessonCountResponse>> GetCourseStructureById(Guid id, CancellationToken cancellationToken = default)
    {
        var courseSectionLessonCount = await _context.Courses
            .Where(course => course.CourseId == id &&  course.IsActive)
            .Select(course => new CourseSectionLessonCountResponse(
                course.CourseId,
                course.Title, // Ensure that the CourseName property exists
                course.sections.Count,
                course.sections.Select(section => new SectionLessonCountResponse(
                    section.SectionId,
                    section.Title, // Ensure that the SectionName property exists
                    section.Lessons.Count
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (courseSectionLessonCount == null)
        {
            return Result.Failure<CourseSectionLessonCountResponse>(CoursesErrors.NotFound);
        }

        return Result.Success(courseSectionLessonCount);
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

    private async Task UpdateCourseInBackground(Course course)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // Update the course in the repository
        await unitOfWork.Repository<Course>().UpdateAsync(course);

        // Commit the changes asynchronously
        await unitOfWork.CompleteAsync();
    }

    private async Task AddCourseInBackground(Course course)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await unitOfWork.Repository<Course>().AddAsync(course);
        await unitOfWork.CompleteAsync();
    }


}
