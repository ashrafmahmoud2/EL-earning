namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.UserLimiter)]
public class LessonsController(ILessonService LessonService,ICommentService commentService) : ControllerBase
{
    private readonly ILessonService _LessonService = LessonService;
    private readonly ICommentService _commentService = commentService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLessonById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var lesson = await _LessonService.GetLessonByIdAsync(id, cancellationToken);

        return lesson.IsSuccess ? Ok(lesson.Value) : lesson.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllLessons()
    {
        var lessons = await _LessonService.GetAllLessonsAsync();

        return lessons.IsSuccess ? Ok(lessons.Value) : lessons.ToProblem();
    }

    [HttpPost("")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> CreateLesson([FromBody] LessonRequest request, CancellationToken cancellationToken)
    {
        var Lesson = await _LessonService.CreateLessonAsync(request, cancellationToken);

        return Lesson.IsSuccess ? Created() : Lesson.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> UpdateLesson([FromRoute]Guid id, [FromBody] LessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await _LessonService.UpdateLessonAsync(id, request, cancellationToken);

        return lesson.IsSuccess ? Ok(lesson.Value) : lesson.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    [Authorize(Roles = $"{UserRole.Admin},{UserRole.Instructor}")]
    public async Task<IActionResult> ToggleStatusLesson([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var lesson = await _LessonService.ToggleStatusAsync(id, cancellationToken);

        return lesson.IsSuccess ? NoContent() : lesson.ToProblem();
    }

    [HttpPut("{lessonId}/comments/count")]
    public async Task<IActionResult> CountCommentsForLesson([FromRoute] Guid lessonId, CancellationToken cancellationToken)
    {
        var lesson = await _commentService.CountCommentsForLesson(lessonId, cancellationToken);

        return lesson.IsSuccess ? Ok(lesson.Value) : lesson.ToProblem();
    }
}
