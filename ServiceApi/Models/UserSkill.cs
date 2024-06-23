namespace ServiceApi.Models;

public partial class UserSkill
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public virtual User? User { get; set; }
}
