﻿using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Course;

namespace ELearning.Service.IService;

public interface ICourseService
{
    Task<Result<CourseResponse>> GetCourseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> GetCourseByinstructorId(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> GetCourseBycategoryId(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CourseResponse>>> GetAllCoursesAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> UpdateCourseAsync(Guid id, CourseRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<CourseSectionLessonCountResponse>>> GetCoursesStructure(CancellationToken cancellationToken = default);
    Task<Result<CourseSectionLessonCountResponse>> GetCourseStructureById(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<CourseEnrollmentCountResponse>>> CountEnrollmentsForCourses(CancellationToken cancellationToken = default);
    Task<Result<List<CourseRefundedCountResponse>>> GetCourseRefundedCountsAsync(CancellationToken cancellationToken = default);

}
