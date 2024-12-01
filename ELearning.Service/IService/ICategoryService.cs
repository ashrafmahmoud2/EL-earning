using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Categorys;
using ELearning.Data.Entities;

namespace ELearning.Service.IService;

public interface ICategoryService
{
    Task<Result<CategoryResponse>> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryResponse>> GetAllCategorysAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> UpdateCategoryAsync(Guid id, CategoryRequest  request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

