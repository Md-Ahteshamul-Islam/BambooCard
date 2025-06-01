using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;

/// <summary>
/// Represents the settings model for API-related JWT configuration
/// </summary>
public record BCAPISettingsModel : BaseNopModel, ISettingsModel
{
    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BCAPISettings.SecretKey")]
    public string SecretKey { get; set; }
    public bool SecretKey_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BCAPISettings.TokenValidityInMinute")]
    public int TokenValidityInMinute { get; set; }
    public bool TokenValidityInMinute_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BCAPISettings.JwtRefreshTokenLifetimeInHours")]
    public int JwtRefreshTokenLifetimeInHours { get; set; }
    public bool JwtRefreshTokenLifetimeInHours_OverrideForStore { get; set; }

    public int ActiveStoreScopeConfiguration { get; set; }
}
