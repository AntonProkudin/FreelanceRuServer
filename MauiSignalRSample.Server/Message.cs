namespace FreelanceRuHub;

public class Message
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public string Text { get; set; }
    public DateTime SendTime { get; set; }
    public bool IsRead { get; set; } = false;
    public string Type { get; set; } = "text";
}
