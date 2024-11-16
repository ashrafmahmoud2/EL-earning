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

    public LessonService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LessonResponse>> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Lessons = await _unitOfWork.Repository<Lesson>()
                                         .FindAsync(x => x.LessonId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Lesson = Lessons.FirstOrDefault();

        if (Lesson is null)
            return Result.Failure<LessonResponse>(LessonErrors.LessonNotFound);

        var LessonResponse = Lesson.Adapt<LessonResponse>();

        return Result.Success(LessonResponse);
    }

    public async Task<Result<LessonResponse>> CreateLessonAsync(LessonRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Section>().AnyAsync(x => x.SectionId ==request.SectionId))
            return Result.Failure<LessonResponse>(SectionErrors.SectionNotFound);


        if (request is null)
            Result.Failure(LessonErrors.LessonNotFound);

        
        var Lesson = request.Adapt<Lesson>();

        Lesson.OrderIndex = await GetNextOrderIndexForCourseAsync(Lesson.SectionId, cancellationToken);

        await _unitOfWork.Repository<Lesson>().AddAsync(Lesson, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(Lesson.Adapt<LessonResponse>());
    }


    public async Task<IEnumerable<LessonResponse>> GetAllLessonsAsync(CancellationToken cancellationToken = default)
    {
        var Lessons = await _unitOfWork.Repository<Lesson>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.CreatedBy),
                cancellationToken: cancellationToken);




        return Lessons.Adapt<IEnumerable<LessonResponse>>();
    }

    public async Task<Result<LessonResponse>> UpdateLessonAsync(Guid id, LessonRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Section>().AnyAsync(x => x.SectionId == request.SectionId))
            return Result.Failure<LessonResponse>(SectionErrors.SectionNotFound);

        var Lessons = await _unitOfWork.Repository<Lesson>()
                                         .FindAsync(x => x.LessonId == id,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);
        var Lesson = Lessons.FirstOrDefault();

        if (Lesson is null)
            return Result.Failure<LessonResponse>(LessonErrors.LessonNotFound);

        Lesson.SectionId = request.SectionId;
        Lesson.Title = request.Title;
        Lesson.Description = request.Description;


        await _unitOfWork.Repository<Lesson>().UpdateAsync(Lesson, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(Lesson.Adapt<LessonResponse>());
    }


    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Lessons = await _unitOfWork.Repository<Lesson>()
                                           .FindAsync(x => x.LessonId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Lesson = Lessons.FirstOrDefault();

        if (Lesson is null)
            return Result.Failure(LessonErrors.LessonNotFound);

        Lesson.IsActive = !Lesson.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

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
