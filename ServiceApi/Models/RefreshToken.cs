namespace ServiceApi.Models;
public partial class RefreshToken
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required string TokenHash { get; set; }
    public required string TokenSalt { get; set; }
    public required DateTime Ts { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public virtual User? User { get; set; }
}

