using System.Text.Json.Serialization;

namespace ServiceApi.Models.Responses.News;
public class GetNewsResponse : BaseResponse
{
    public List<Models.News>? News { get; set; }
}
public class GetNewResponse : BaseResponse
{
    public Models.News? News { get; set; }
}
public class NewsResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
    public required DateTime Ts { get; set; }
}