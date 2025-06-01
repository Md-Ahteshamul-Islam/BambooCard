using BambooCard.Plugin.Misc.AssessmentTasks.Domains;
using Nop.Core.Caching;
using Nop.Data;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;

public class JwtRefreshTokenService : IJwtRefreshTokenService
{
    private readonly IRepository<JwtRefreshToken> _jwtRefreshTokenRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    public JwtRefreshTokenService(IRepository<JwtRefreshToken> jwtRefreshTokenRepository,
        IStaticCacheManager staticCacheManager)
    {
        _jwtRefreshTokenRepository = jwtRefreshTokenRepository;
        _staticCacheManager = staticCacheManager;
    }

    public async Task<JwtRefreshToken> GetJwtRefreshTokenByTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token) || !Guid.TryParse(token, out var tokenGuid))
        {
            return null;
        }

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(JwtRefreshTokenCacheKeys.GetJwtRefreshTokenByTokenCacheKey,
            tokenGuid);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _jwtRefreshTokenRepository.Table
                .FirstOrDefaultAsync(jrt => jrt.Token == tokenGuid);
        });
    }

    public async Task<IList<JwtRefreshToken>> GetJwtRefreshTokensByCustomer(int customerId, bool excludeInactive = true)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException(nameof(customerId));
        }

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(JwtRefreshTokenCacheKeys.GetJwtRefreshTokensByCustomerIdCacheKey,
            customerId, excludeInactive);

        var query = _jwtRefreshTokenRepository.Table
            .Where(jrt => jrt.CustomerId == customerId);

        if (excludeInactive)
        {
            query = query.Where(jrt => jrt.Active);
        }

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await query.OrderByDescending(jrt => jrt.Id)
                .ToListAsync();
        });
    }

    public async Task InsertJwtRefreshTokenAsync(JwtRefreshToken entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _jwtRefreshTokenRepository.InsertAsync(entity);
    }

    public async Task UpdateJwtRefreshTokenAsync(JwtRefreshToken entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _jwtRefreshTokenRepository.UpdateAsync(entity);
    }

    public async Task DeleteJwtRefreshTokenAsync(JwtRefreshToken entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _jwtRefreshTokenRepository.DeleteAsync(entity);
    }
}
