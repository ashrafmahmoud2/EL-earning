using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Categorys;
using ELearning.Data.Errors;
namespace ELearning.Service.Service;

public class CategoryService : BaseRepository<Category>, ICategoryService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Categorys = await _unitOfWork.Repository<Category>()
                                         .FindAsync(x => x.CategoryId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Category = Categorys.FirstOrDefault();

        if (Category is null)
            return Result.Failure<CategoryResponse>(CategorysErrors.CategoryNotFound);

        var CategoryResponse = Category.Adapt<CategoryResponse>();

        return Result.Success(CategoryResponse);
    }

    public async Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            Result.Failure(CategorysErrors.CategoryNotFound);


        var category = request.Adapt<Category>();
        await _unitOfWork.Repository<Category>().AddAsync(category, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        var result = category.Adapt<CategoryResponse>();
        return Result.Success(result);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategorysAsync(CancellationToken cancellationToken = default)
    {


        var Categorys = await _unitOfWork.Repository<Category>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.CreatedBy),
                cancellationToken: cancellationToken);




        return Categorys.Select(s => new CategoryResponse(
              s.CategoryId,
             Name: s.Name,
             NumberOfCourses: s.NumberOfCourses,
             IsActive: s.IsActive,
             CreatedBy: s.CreatedBy.FirstName + " " + s.CreatedBy.LastName,
             CreatedOn: s.CreatedOn
         )).ToList();

    }

    public async Task<Result<CategoryResponse>> UpdateCategoryAsync(Guid id, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var Categorys = await _unitOfWork.Repository<Category>()
                                         .FindAsync(x => x.CategoryId == id,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);
        var category = Categorys.FirstOrDefault();

        if (category is null)
            return Result.Failure<CategoryResponse>(CategorysErrors.CategoryNotFound);


        category.Name = request.Name;

         await _unitOfWork.Repository<Category>().UpdateAsync(category, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(category.Adapt<CategoryResponse>());
    }


    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Categorys = await _unitOfWork.Repository<Category>()
                                           .FindAsync(x => x.CategoryId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Category = Categorys.FirstOrDefault();

        if (Category is null)
            return Result.Failure(CategorysErrors.CategoryNotFound);

        Category.IsActive= !Category.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

}
