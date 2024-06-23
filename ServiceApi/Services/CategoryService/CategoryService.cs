using ServiceApi.Models.Responses.Task;
using ServiceApi.Models;
using ServiceApi.Wrappers;
using static ServiceApi.Database.EF.ApiDbContext;
using Microsoft.EntityFrameworkCore;

namespace ServiceApi.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ThisDbContext thisDbContext;
    public CategoryService(ThisDbContext thisDbContext)
    {
        this.thisDbContext = thisDbContext;
    }
    public async Task<DeleteCategoryResponse> DeleteCategory(int Id)
    {
        var category = await thisDbContext.Categories.FindAsync(Id);

        if (category == null)
        {
            return new DeleteCategoryResponse
            {
                Success = false,
                Error = "Category not found",
                ErrorCode = "T01"
            };
        }

        thisDbContext.Categories.Remove(category);
        ServiceWrapper.CategoryList.Remove(category.Id, out _);

        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            return new DeleteCategoryResponse
            {
                Success = true,
                CategoryId = category.Id
            };
        }

        return new DeleteCategoryResponse
        {
            Success = false,
            Error = "Unable to delete category",
            ErrorCode = "T03"
        };
    }
    public async Task<GetCategoriesResponse> GetCategories()
    {
        var categories = await thisDbContext.Categories.ToListAsync();

        if (categories.Count == 0)
        {
            return new GetCategoriesResponse
            {
                Success = false,
                Error = "No categories found",
                ErrorCode = "T04"
            };
        }

        return new GetCategoriesResponse { Success = true, Categories = categories };
    }
    public async Task<SaveCategoryResponse> SaveCategory(Category category)
    {
        await thisDbContext.Categories.AddAsync(category);

        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            ServiceWrapper.CategoryList.TryAdd(category.Id, category.Title);
            return new SaveCategoryResponse
            {
                Success = true,
                Category = category
            };
        }
        return new SaveCategoryResponse
        {
            Success = false,
            Error = "Unable to save task",
            ErrorCode = "T05"
        };
    }
}