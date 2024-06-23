using SixLabors.ImageSharp;

namespace ServiceApi.Services.HelpService;

public class HelpService:IHelpService
{
    public async Task<SendImageResponse> LoadImage(IFormFile file)
    {
        SendImageResponse result = new();
        try
        {
            var dateTimeNow = DateTime.Now.ToShortDateString();
            string fileName = Path.GetRandomFileName() + "." + file.FileName.Split(".").Last();
            string url = $"/images/{dateTimeNow.Replace(".", "")}/{fileName}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot//images//{dateTimeNow.Replace(".", "")}//");

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                await file.CopyToAsync(stream);
            }
            result.Url = url;
        }
        catch (Exception ex)
        {
            result.Ex = ex.Message;
        }
        return result;
    }
    public ImgMetaData ReadMetaData(string url)
    {
        var size = System.IO.File.ReadAllBytes(url).Length;
        ImageInfo imageInfo = Image.Identify(url);
        return new ImgMetaData()
        {
            w = imageInfo.Width,
            h = imageInfo.Height,
            size = size,
        };
    }

}
