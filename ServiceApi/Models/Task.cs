namespace ServiceApi.Models;
public partial class Task
{
    public  int Id { get; set; }
    public required int UserId { get; set; }
    public required int CategoryId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required bool IsCompleted { get; set; }
    public required DateTime Ts { get; set; }
    public virtual  User? User { get; set; }
    public virtual Category? Category { get; set; }
}