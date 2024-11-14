using ELearning.Data.Contracts.Course;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService CourseService) : ControllerBase
{
    private readonly ICourseService _CourseService = CourseService;


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Course = await _CourseService.GetCourseByIdAsync(id, cancellationToken);

        return Course.IsSuccess ? Ok(Course.Value) : Course.ToProblem();
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


    [HttpGet("")]
    public async Task<IActionResult> GetAllCourses()
    {
        var Course = await _CourseService.GetAllCoursesAsync();

        return Ok(Course);
    }


    [HttpPost("")]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequest request, CancellationToken cancellationToken)
    {
        var Instructor = await _CourseService.CreateCourseAsync(request, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] CourseRequest request, CancellationToken cancellationToken)
    {
        var coures =await  _CourseService.UpdateCourseAsync(id, request, cancellationToken);

        return coures.IsSuccess ? NoContent() : coures.ToProblem();
    }


    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusCourse([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Instructor = await _CourseService.ToggleStatusAsync(id, cancellationToken);

        return Instructor.IsSuccess ? NoContent() : Instructor.ToProblem();
    }


    [HttpGet("counts")]
    public async Task<IActionResult> CountCoursesWithSectionsAndLessons( CancellationToken cancellationToken)
    {
        var result = await _CourseService.GetCourseSectionLessonCounts( cancellationToken);

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
