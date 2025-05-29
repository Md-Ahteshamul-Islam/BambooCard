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

    #endregion

    #region Methods

    public virtual async Task<ConfigurationModel> PrepareConfigurationModelAsync()
    {
        var model = new ConfigurationModel();
        model.BambooCardDiscountSettings = await PrepareBambooCardDiscountSettingsModelAsync();

        return model;
    }

    #endregion
}
