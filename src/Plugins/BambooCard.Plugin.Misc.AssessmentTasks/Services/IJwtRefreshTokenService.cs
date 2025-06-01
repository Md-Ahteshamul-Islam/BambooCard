using BambooCard.Plugin.Misc.AssessmentTasks.Domains;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;
public interface IJwtRefreshTokenService
{
    Task DeleteJwtRefreshTokenAsync(JwtRefreshToken entity);
    Task<JwtRefreshToken> GetJwtRefreshTokenByTokenAsync(string token);
    Task<IList<JwtRefreshToken>> GetJwtRefreshTokensByCustomer(int customerId, bool excludeInactive = true);
    Task InsertJwtRefreshTokenAsync(JwtRefreshToken entity);
    Task UpdateJwtRefreshTokenAsync(JwtRefreshToken entity);
}