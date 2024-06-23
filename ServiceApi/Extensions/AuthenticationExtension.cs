using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServiceApi.Helpers;

namespace ServiceApi.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJWTAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = TokenHelper.Issuer,
                    ValidAudience = TokenHelper.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey
                                       (Convert.FromBase64String(TokenHelper.Secret))
                };
            });
        return services;
    }
}
