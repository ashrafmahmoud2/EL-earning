using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Section;
using ELearning.Data.Errors;
using static System.Collections.Specialized.BitVector32;
using Section = ELearning.Data.Entities.Section;
using ELearning.Data.Contracts.QuizAttempt;
using ELearning.Data.Entities;
namespace ELearning.Service.Service;

public class SectionService : BaseRepository<Section>, ISectionService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public SectionService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SectionResponse>> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var section = await _unitOfWork.Repository<Section>()
                                         .FirstOrDefaultAsync(x => x.SectionId == id && x.IsActive, 
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Course)
                                         , cancellationToken);

        if (section is null)
            return Result.Failure<SectionResponse>(SectionsErrors.NotFound);

        var sectionResponse = section.Adapt<SectionResponse>();

        return Result.Success(sectionResponse);
    }

    public async Task<IEnumerable<SectionResponse>> GetAllSectionsAsync(CancellationToken cancellationToken = default)
    {
        var sections = await _unitOfWork.Repository<Section>()
            .FindAsync(
                s => s.IsActive,
                q => q.Include(x => x.CreatedBy)
                      .Include(x => x.Course)
            , cancellationToken);




        return sections.Adapt<IEnumerable<SectionResponse>>();
    }
    
    public async Task<IEnumerable<SectionWithLessonsResponse>> GetAllSectionsWithLessonsAsync(CancellationToken cancellationToken = default)
    {
        var sections = await _unitOfWork.Repository<Section>()
            .FindAsync(
               x => x.IsActive,
                query => query.Include(section => section.CreatedBy)
                              .Include(section => section.Course)
                              .Include(section => section.Lessons),
                cancellationToken);

        return sections.Adapt<IEnumerable<SectionWithLessonsResponse>>();
    }


    public async Task<Result> CreateSectionAsync(SectionRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Course>().AnyAsync(x => x.CourseId == request.CourseId))
            return Result.Failure<SectionResponse>(CoursesErrors.NotFound);

         if (await _unitOfWork.Repository<Section>().AnyAsync(x => x.CourseId == request.CourseId && x.Title == request.Title))
            return Result.Failure<SectionResponse>(SectionsErrors.DuplicatedSection);

        

        if (request is null)
            Result.Failure(SectionsErrors.NotFound);

        
        var section = request.Adapt<Section>();

        section.OrderIndex = await GetNextOrderIndexForCourseAsync(section.CourseId, cancellationToken);

        await _unitOfWork.Repository<Section>().AddAsync(section, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<SectionResponse>> UpdateSectionAsync(Guid id, SectionRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Course>().AnyAsync(x => x.CourseId == request.CourseId && x.IsActive))
            return Result.Failure<SectionResponse>(CoursesErrors.NotFound);

        var section = await _unitOfWork.Repository<Section>()
                                      .FirstOrDefaultAsync(x => x.SectionId == id,
                                      q => q.Include(x => x.CreatedBy)
                                            .Include(x => x.Course)
                                      , cancellationToken);

        if (section is null)
            return Result.Failure<SectionResponse>(SectionsErrors.NotFound);

        section.CourseId = request.CourseId;
        section.Title = request.Title;
        section.Description = request.Description;


        await _unitOfWork.Repository<Section>().UpdateAsync(section, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(section.Adapt<SectionResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var section = await _unitOfWork.Repository<Section>()
                                           .FirstOrDefaultAsync(x => x.SectionId == id && x.IsActive);

        if (section is null)
            return Result.Failure(SectionsErrors.NotFound);

        section.IsActive = !section.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<int> GetNextOrderIndexForCourseAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var lastOrderIndex = await _unitOfWork.Repository<Section>()
            .Find(s => s.CourseId == courseId)
            .MaxAsync(s => (int?)s.OrderIndex, cancellationToken);

        return (lastOrderIndex ?? 0) + 1;
    }

}
