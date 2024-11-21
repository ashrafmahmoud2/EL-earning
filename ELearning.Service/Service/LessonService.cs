using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Lesson;
using ELearning.Data.Errors;
using ELearning.Data.Contracts.Instrctors;
using Mailjet.Client.Resources;
namespace ELearning.Service.Service;

public class LessonService : BaseRepository<Lesson>, ILessonService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public LessonService(ApplicationDbContext context, IUnitOfWork unitOfWork, ICacheService cacheService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<LessonResponse>> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lesson = await _unitOfWork.Repository<Lesson>()
                                         .FirstOrDefaultAsync(x => x.LessonId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                         .Include(x => x.Section),
                                cancellationToken);
       

        if (lesson is null)
            return Result.Failure<LessonResponse>(LessonsErrors.NotFound);

        var lessonResponse = lesson.Adapt<LessonResponse>();

        return Result.Success(lessonResponse);
    }

    public async Task<IEnumerable<LessonResponse>> GetAllLessonsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Lessons:GetAll";

       

        // Check if data is in the cache
        var cachedLessons = await _cacheService.GetCacheAsync<IEnumerable<LessonResponse>>(cacheKey);

        if (cachedLessons != null)
        {
            return cachedLessons;
        }


        var lessons = await _unitOfWork.Repository<Lesson>()
                                        .FindAsync(x =>  x.IsActive,
                                        q => q.Include(x => x.CreatedBy)
                                        .Include(x => x.Section),
                               cancellationToken);




        var lessonsResponses = lessons.Adapt<IEnumerable<LessonResponse>>();

        // Cache the adapted response
        await _cacheService.SetCacheAsync(cacheKey, lessonsResponses, _cacheDuration);

        return lessonsResponses;
    }

    public async Task<Result> CreateLessonAsync(LessonRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Section>().AnyAsync(x => x.SectionId ==request.SectionId))
            return Result.Failure<LessonResponse>(SectionsErrors.NotFound);

        if (await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.Title == request.Title && x.SectionId == request.SectionId))
            return Result.Failure<LessonResponse>(LessonsErrors.DuplicatedLesson);

        if (request is null)
            Result.Failure(LessonsErrors.NotFound);

        
        var Lesson = request.Adapt<Lesson>();

        Lesson.OrderIndex = await GetNextOrderIndexForCourseAsync(Lesson.SectionId, cancellationToken);

        await _unitOfWork.Repository<Lesson>().AddAsync(Lesson, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Lessons:GetAll");

        return Result.Success();
    }

    public async Task<Result<LessonResponse>> UpdateLessonAsync(Guid id, LessonRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Section>().AnyAsync(x => x.SectionId == request.SectionId))
            return Result.Failure<LessonResponse>(SectionsErrors.NotFound);


        var lesson = await _unitOfWork.Repository<Lesson>()
                                         .FirstOrDefaultAsync(x => x.LessonId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                         .Include(x => x.Section),
                                cancellationToken);

        if (lesson is null)
            return Result.Failure<LessonResponse>(LessonsErrors.NotFound);

       lesson.SectionId = request.SectionId;
       lesson.Title = request.Title;
       lesson.Description = request.Description;

        await _unitOfWork.Repository<Lesson>().UpdateAsync(lesson, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Lessons:GetAll");

        return Result.Success(lesson.Adapt<LessonResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lesson = await _unitOfWork.Repository<Lesson>()
                                         .FirstOrDefaultAsync(x => x.LessonId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                         .Include(x => x.Section),
                                cancellationToken);

        if (lesson is null)
            return Result.Failure(LessonsErrors.NotFound);

        lesson.IsActive = !lesson.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Lessons:GetAll");

        return Result.Success();
    }

    private async Task<int> GetNextOrderIndexForCourseAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var lastOrderIndex = await _unitOfWork.Repository<Lesson>()
            .Find(s => s.SectionId == courseId)
            .MaxAsync(s => (int?)s.OrderIndex, cancellationToken);

        return (lastOrderIndex ?? 0) + 1;
    }

}
