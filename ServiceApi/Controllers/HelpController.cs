using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HelpController:BaseApiController
{
    private readonly IHelpService helper;
    public HelpController(IServiceProvider provider)
    {
        helper = provider.GetService<IHelpService>();
    }

    [Authorize]
    [HttpPost("LoadImage")]
    public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
    {
        try
        {
            if (!file.ContentType.StartsWith("image") || file == null)
                return BadRequest("Image not found");

            SendImageResponse result = await helper.LoadImage(file);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest("Image not found");
        }

    }
}
