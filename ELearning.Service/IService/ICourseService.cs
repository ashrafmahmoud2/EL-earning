﻿using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Course;

namespace ELearning.Service.IService;

public interface ICourseService
{
    Task<Result<CourseResponse>> GetCourseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default);
    Task<Result>  UpdateCourseAsync(Guid id, CourseRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}