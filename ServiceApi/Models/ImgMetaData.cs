namespace ServiceApi.Models;

public class ImgMetaData
{
    public string type { get; set; } = "IMG";
    public int w { get; set; }
    public int h { get; set; }
    public int size { get; set; }
    public string? name { get; set; }
    public string? src { get; set; }
}
