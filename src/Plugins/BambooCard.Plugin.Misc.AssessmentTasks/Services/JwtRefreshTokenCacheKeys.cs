using Nop.Core.Caching;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;

public static class JwtRefreshTokenCacheKeys
{
    public const string PREFIX = "Crowley.Features.JwtRefreshToken.";

    public static CacheKey GetJwtRefreshTokenByTokenCacheKey => new("Crowley.Features.JwtRefreshToken.GetJwtRefreshTokenByToken.{0}", PREFIX);

    public static CacheKey GetJwtRefreshTokensByCustomerIdCacheKey => new("Crowley.Features.JwtRefreshToken.GetJwtRefreshTokensByCustomerId.{0}.{1}", PREFIX);
}