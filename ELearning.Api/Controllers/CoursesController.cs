﻿using ELearning.Data.Consts;
using ELearning.Data.Contracts.Course;
using ELearning.Data.Contracts.Filters;
using ELearning.Data.Enums;
using Microsoft.AspNetCore.RateLimiting;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.Concurrency)]
[Authorize]
public class CoursesController(ICourseService CourseService) : ControllerBase
{
    private readonly ICourseService _CourseService = CourseService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var course = await _CourseService.GetCourseByIdAsync(id, cancellationToken);

        return course.IsSuccess ? Ok(course.Value) : course.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllCourses([FromQuery] RequestFilters filters , CancellationToken cancellationToken)
    {
        var course = await _CourseService.GetAllCoursesAsync(filters , cancellationToken);

        return course.IsSuccess ? Ok(course.Value) : course.ToProblem();
    }

    [HttpPost("")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequest request, CancellationToken cancellationToken)
    {
        var course = await _CourseService.CreateCourseAsync(request, cancellationToken);

        return course.IsSuccess ? Created() : course.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] CourseRequest request, CancellationToken cancellationToken)
    {
        var coures = await _CourseService.UpdateCourseAsync(id, request, cancellationToken);

        return coures.IsSuccess ? Ok(coures.Value) : coures.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> ToggleStatusCourse([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var course = await _CourseService.ToggleStatusAsync(id, cancellationToken);

        return course.IsSuccess ? NoContent() : course.ToProblem();
    }

    [HttpGet("get_by_instructor/{id}")]
    public async Task<IActionResult> GetCourseByinstructorId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Course = await _CourseService.GetCourseByinstructorId(id, cancellationToken);

        return Course.IsSuccess ? Ok(Course.Value) : Course.ToProblem();
    }
    
    [HttpGet("get_by_categoryId/{id}")]
    public async Task<IActionResult> GetCourseBycategoryId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Course = await _CourseService.GetCourseBycategoryId(id, cancellationToken);

        return Course.IsSuccess ? Ok(Course.Value) : Course.ToProblem();
    }

    [HttpGet("courses_structure")]
    public async Task<IActionResult> CountCoursesWithSectionsAndLessons( CancellationToken cancellationToken)
    {
        var result = await _CourseService.GetCoursesStructure( cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("course_structure/{id}")]
    public async Task<IActionResult> CountCoursesWithSectionsAndLessons([FromRoute] Guid id,CancellationToken cancellationToken)
    {
        var result = await _CourseService.GetCourseStructureById(id,cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpGet("enrollment-counts")]
    public async Task<IActionResult> GetCourseEnrollmentCounts(CancellationToken cancellationToken)
    {
        var result = await _CourseService.CountEnrollmentsForCourses(cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("refude-counts")]
    public async Task<IActionResult> GetCourserefudeCounts(CancellationToken cancellationToken)
    {
        var result = await _CourseService.GetCourseRefundedCountsAsync(cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


}
