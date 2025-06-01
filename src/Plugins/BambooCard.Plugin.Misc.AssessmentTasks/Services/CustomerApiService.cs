using System.Text;
using BambooCard.Plugin.Misc.AssessmentTasks.Domains;
using BambooCard.Plugin.Misc.AssessmentTasks.Extensions;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;

public class CustomerApiService : ICustomerApiService
{
    private readonly ICustomerService _customerService;
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    private readonly IJwtRefreshTokenService _jwtRefreshTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BCAPISettings _bcAPISettings;

    public CustomerApiService(ICustomerService customerService,
        ILocalizationService localizationService,
        ISettingService settingService,
        IJwtRefreshTokenService jwtRefreshTokenService,
        IHttpContextAccessor httpContextAccessor,
        BCAPISettings bcAPISettings)
    {
        _customerService = customerService;
        _localizationService = localizationService;
        _httpContextAccessor = httpContextAccessor;
        _bcAPISettings = bcAPISettings;
        _settingService = settingService;
        _jwtRefreshTokenService = jwtRefreshTokenService;
    }

    public async Task<string> GetTokenAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var now = Math.Round((DateTime.UtcNow.AddMinutes(_bcAPISettings.TokenValidityInMinute) - unixEpoch).TotalSeconds);

        var payload = new Dictionary<string, object>()
            {
                { AssessmentTasksDefaults.CustomerId, customer.Id },
                { "exp", now },
            };

        string encodedPayload;

        if (string.IsNullOrWhiteSpace(_bcAPISettings.SecretKey))
        {
            var secretKey = HelperExtension.RandomString(48);
            await _settingService.SaveSettingAsync(new BCAPISettings
            {
                SecretKey = secretKey
            });
            encodedPayload = JwtHelper.JwtEncoder.Encode(payload, secretKey);
        }
        else
        {
            encodedPayload = JwtHelper.JwtEncoder.Encode(payload, _bcAPISettings.SecretKey);
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Append(AssessmentTasksDefaults.CustomerToken,
            encodedPayload, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

        return encodedPayload;

    }

    public async Task<string> GenerateRefreshTokenAsync(Customer customer, bool isPersistent = false)
    {
        ArgumentNullException.ThrowIfNull(customer);

        var jwtRefreshTokenLifetimeInHours = _bcAPISettings.JwtRefreshTokenLifetimeInHours;

        var jwtRefreshToken = new JwtRefreshToken
        {
            Active = true,
            CustomerId = customer.Id,
            Token = Guid.NewGuid(),
            ValidTill = DateTime.UtcNow.AddHours(jwtRefreshTokenLifetimeInHours)
        };

        await _jwtRefreshTokenService.InsertJwtRefreshTokenAsync(jwtRefreshToken);

        _httpContextAccessor.HttpContext.Response.Cookies.Append(AssessmentTasksDefaults.JwtRefreshToken,
            jwtRefreshToken.Token.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = isPersistent ? DateTime.UtcNow.AddDays(30) : null
            });

        return jwtRefreshToken.Token.ToString();
    }
}
