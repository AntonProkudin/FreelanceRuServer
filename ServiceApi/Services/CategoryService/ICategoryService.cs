using ServiceApi.Models.Responses.Task;
using ServiceApi.Models;

namespace ServiceApi.Services.CategoryService;

public interface ICategoryService
{
    Task<GetCategoriesResponse> GetCategories();
    Task<SaveCategoryResponse> SaveCategory(Category category);
    Task<DeleteCategoryResponse> DeleteCategory(int Id);
}
