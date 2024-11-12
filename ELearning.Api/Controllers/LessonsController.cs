using ELearning.Data.Contracts.Lesson;

namespace ELearning.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController(ILessonService LessonService) : ControllerBase
{
    private readonly ILessonService _LessonService = LessonService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLessonById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Lesson = await _LessonService.GetLessonByIdAsync(id, cancellationToken);

        return Lesson.IsSuccess ? Ok(Lesson.Value) : Lesson.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllLessons()
    {
        var Lesson = await _LessonService.GetAllLessonsAsync();

        return Ok(Lesson);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateLesson([FromBody] LessonRequest request, CancellationToken cancellationToken)
    {
        var Lesson = await _LessonService.CreateLessonAsync(request, cancellationToken);

        return Lesson.IsSuccess ? NoContent() : Lesson.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson([FromRoute]Guid id, [FromBody] LessonRequest request, CancellationToken cancellationToken)
    {
        var Lesson = await _LessonService.UpdateLessonAsync(id, request, cancellationToken);

        return Lesson.IsSuccess ? NoContent() : Lesson.ToProblem();
    }

    [HttpPut("Toggle_status{id}")]
    public async Task<IActionResult> ToggleStatusLesson([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var Lesson = await _LessonService.ToggleStatusAsync(id, cancellationToken);

        return Lesson.IsSuccess ? NoContent() : Lesson.ToProblem();
    }
}
