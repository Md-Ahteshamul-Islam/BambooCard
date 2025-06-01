using BambooCard.Plugin.Misc.AssessmentTasks.Extensions;
using BambooCard.Plugin.Misc.AssessmentTasks.Services;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using JWT;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;
using Nop.Services.Logging;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Infrastructure;

public class JwtAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly BCAPISettings _bcAPISettings;
    private readonly ILogger _logger;

    public JwtAuthMiddleware(RequestDelegate next,
        BCAPISettings bcAPISettings,
        ILogger logger)
    {
        _next = next;
        _bcAPISettings = bcAPISettings;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IWorkContext workContext, IWebHelper webHelper,
        ICustomerService customerService, ICustomerApiService customerApiService, BCAPISettings bcAPISettings)
    {
        string token;
        string refreshToken = "";
        if (context.Request.Headers.TryGetValue(AssessmentTasksDefaults.Token, out var tokenKey))
        {
            token = tokenKey.FirstOrDefault();
        }
        else
        {
            var cookieName = $".Nop.Customer.BC-Token";
            token = context.Request?.Cookies[cookieName];

            if (string.IsNullOrWhiteSpace(token))
                token = webHelper.QueryString<string>("customerToken");
        }

        if (context.Request.Cookies.TryGetValue(AssessmentTasksDefaults.JwtRefreshToken, out var jwtRefreshTokenKey))
        {
            refreshToken = jwtRefreshTokenKey ?? "";
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            SetCustomerTokenCookie(context, token);
            try
            {
                var load = JwtHelper.JwtDecoder.DecodeToObject(token, bcAPISettings.SecretKey, true);
                if (load != null)
                {
                    var customerId = Convert.ToInt32(load[AssessmentTasksDefaults.CustomerId]);
                    var customer = await customerService.GetCustomerByIdAsync(customerId);
                    await workContext.SetCurrentCustomerAsync(customer);
                }
            }
            catch (TokenExpiredException)
            {
                if (!string.IsNullOrWhiteSpace(refreshToken))
                {
                    var jwtRefreshTokenService = EngineContext.Current.Resolve<IJwtRefreshTokenService>();
                    var jwtRefreshToken = await jwtRefreshTokenService.GetJwtRefreshTokenByTokenAsync(refreshToken);
                    double jwtRefreshTokenValidDays = 0;

                    if (jwtRefreshToken is not null && jwtRefreshToken.Active &&
                        (jwtRefreshTokenValidDays = (jwtRefreshToken.ValidTill - DateTime.UtcNow).TotalDays) > 0)
                    {
                        var customer = await customerService.GetCustomerByIdAsync(jwtRefreshToken.CustomerId);
                        await customerApiService.GetTokenAsync(customer);
                        if (jwtRefreshTokenValidDays < 1)
                        {
                            await customerApiService.GenerateRefreshTokenAsync(customer);
                        }

                        await workContext.SetCurrentCustomerAsync(customer);
                    }
                }
            }
        }
        await _next(context);
    }

    protected virtual void SetCustomerTokenCookie(HttpContext context, string token)
    {
        //delete current cookie value
        var cookieName = $".Nop.Customer.BC-Token";
        context.Response.Cookies.Delete(cookieName);

        //get date of cookie expiration
        var cookieExpires = _bcAPISettings.TokenValidityInMinute;
        var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

        //if passed guid is empty set cookie as expired
        if (string.IsNullOrWhiteSpace(token))
            cookieExpiresDate = DateTime.Now.AddMonths(-1);

        //set new cookie value
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = cookieExpiresDate
        };
        context.Response.Cookies.Append(cookieName, token, options);
    }
}