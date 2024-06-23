using System.Security.Claims;

namespace ServiceApi.Controllers.Hubs;

public partial class MessageHub
{
    private string FindClaim(string claimName)
    {
        var claimsIdentity = Context.User.Identity as ClaimsIdentity;
        var claim = claimsIdentity?.FindFirst(claimName);

        if (claim == null)
        {
            return String.Empty;
        }
        return claim.Value;
    }
    protected int UserID => int.Parse(FindClaim(ClaimTypes.NameIdentifier));
}
