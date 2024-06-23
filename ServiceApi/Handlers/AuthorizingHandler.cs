using Microsoft.AspNetCore.Authentication.OAuth;

namespace ServiceApi.Handlers;

public class AuthorizingHandler : DelegatingHandler
{
    private readonly OAuthOptions _options;
    public AuthorizingHandler(HttpMessageHandler inner, OAuthOptions options)
        : base(inner)
    {
        _options = options;
    }
}
