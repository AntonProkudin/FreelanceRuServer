using System.Text.Json.Serialization;

namespace ServiceApi.Models.Responses.Task;

public class SaveCategoryResponse : BaseResponse
{
    public Category? Category { get; set; }
}
public class GetCategoriesResponse : BaseResponse
{
    public List<Category>? Categories { get; set; }
}
public class DeleteCategoryResponse : BaseResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int CategoryId { get; set; }
}
public class CategoryResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
}