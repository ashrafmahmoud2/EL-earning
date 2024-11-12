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
        var Sections = await _unitOfWork.Repository<Section>()
                                         .FindAsync(x => x.SectionId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Section = Sections.FirstOrDefault();

        if (Section is null)
            return Result.Failure<SectionResponse>(SectionErrors.SectionNotFound);

        var SectionResponse = Section.Adapt<SectionResponse>();

        return Result.Success(SectionResponse);
    }

    public async Task<Result<SectionResponse>> CreateSectionAsync(SectionRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            Result.Failure(SectionErrors.SectionNotFound);

        
        var section = request.Adapt<Section>();

        section.OrderIndex = await GetNextOrderIndexForCourseAsync(section.CourseId, cancellationToken);

        await _unitOfWork.Repository<Section>().AddAsync(section, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(section.Adapt<SectionResponse>());
    }


    public async Task<IEnumerable<SectionResponse>> GetAllSectionsAsync(CancellationToken cancellationToken = default)
    {
        var Sections = await _unitOfWork.Repository<Section>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.CreatedBy),
                cancellationToken: cancellationToken);




        return Sections.Adapt<IEnumerable<SectionResponse>>();
    }

    public async Task<Result<SectionResponse>> UpdateSectionAsync(Guid id, SectionRequest request, CancellationToken cancellationToken = default)
    {
        
        var Sections = await _unitOfWork.Repository<Section>()
                                         .FindAsync(x => x.SectionId == id,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);
        var section = Sections.FirstOrDefault();

        if (section is null)
            return Result.Failure<SectionResponse>(SectionErrors.SectionNotFound);

        section.CourseId = request.CourseId;
        section.Title = request.Title;
        section.Description = request.Description;


        await _unitOfWork.Repository<Section>().UpdateAsync(section, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(section.Adapt<SectionResponse>());
    }


    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Sections = await _unitOfWork.Repository<Section>()
                                           .FindAsync(x => x.SectionId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Section = Sections.FirstOrDefault();

        if (Section is null)
            return Result.Failure(SectionErrors.SectionNotFound);

        Section.IsActive = !Section.IsActive;

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
