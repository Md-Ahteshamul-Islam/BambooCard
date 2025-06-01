using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
public class ConfigurationModelFactory : IConfigurationModelFactory
{
    #region Fields
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public ConfigurationModelFactory(ISettingService settingService,
        IStoreContext storeContext)
    {
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Utilities

    protected virtual async Task<BambooCardDiscountSettingsModel> PrepareBambooCardDiscountSettingsModelAsync()
    {
        //load settings for a chosen store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var bambooCardDiscountSettings = await _settingService.LoadSettingAsync<BambooCardDiscountSettings>(storeId);

        //fill in model values from the entity
        var model = bambooCardDiscountSettings.ToSettingsModel<BambooCardDiscountSettingsModel>();

        if (storeId <= 0)
            return model;

        //fill in overridden values
        model.EnableCustomDiscount_OverrideForStore = await _settingService.SettingExistsAsync(bambooCardDiscountSettings, x => x.EnableCustomDiscount, storeId);
        model.MinimumOrderCount_OverrideForStore = await _settingService.SettingExistsAsync(bambooCardDiscountSettings, x => x.MinimumOrderCount, storeId);
        model.DiscountPercentage_OverrideForStore = await _settingService.SettingExistsAsync(bambooCardDiscountSettings, x => x.DiscountPercentage, storeId);

        return model;
    }
    protected virtual async Task<BCAPISettingsModel> PrepareBCAPISettingsModelAsync()
    {
        // Load settings for the selected store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var bcApiSettings = await _settingService.LoadSettingAsync<BCAPISettings>(storeId);

        // Fill model from settings
        var model = bcApiSettings.ToSettingsModel<BCAPISettingsModel>();

        if (storeId <= 0)
            return model;

        // Set overridden flags
        model.SecretKey_OverrideForStore = await _settingService.SettingExistsAsync(bcApiSettings, x => x.SecretKey, storeId);
        model.TokenValidityInMinute_OverrideForStore = await _settingService.SettingExistsAsync(bcApiSettings, x => x.TokenValidityInMinute, storeId);
        model.JwtRefreshTokenLifetimeInHours_OverrideForStore = await _settingService.SettingExistsAsync(bcApiSettings, x => x.JwtRefreshTokenLifetimeInHours, storeId);

        return model;
    }

    #endregion

    #region Methods

    public virtual async Task<ConfigurationModel> PrepareConfigurationModelAsync()
    {
        var model = new ConfigurationModel
        {
            BambooCardDiscountSettings = await PrepareBambooCardDiscountSettingsModelAsync(),
            BCAPISettings = await PrepareBCAPISettingsModelAsync()
        };

        return model;
    }
    #endregion
}
