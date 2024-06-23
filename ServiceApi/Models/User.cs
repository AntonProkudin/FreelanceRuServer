namespace ServiceApi.Models;
public partial class User
{
    public User()
    {
        UserSkills = new HashSet<UserSkill>();
        RefreshTokens = new HashSet<RefreshToken>();
        Tasks = new HashSet<Task>();
    }

    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PasswordSalt { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Ts { get; set; }
    public required bool Active { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Task> Tasks { get; set; }
    public virtual ICollection<UserSkill> UserSkills { get; set; }
}