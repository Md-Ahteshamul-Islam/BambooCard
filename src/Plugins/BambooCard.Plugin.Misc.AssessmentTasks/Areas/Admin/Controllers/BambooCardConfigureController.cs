using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Controllers;
public class BambooCardConfigureController : BaseAdminController
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly IConfigurationModelFactory _configurationModelFactory;

    #endregion

    #region Ctor

    public BambooCardConfigureController(IConfigurationModelFactory configurationModelFactory,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _configurationModelFactory = configurationModelFactory;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Security.ACCESS_ADMIN_PANEL)]
    public async Task<IActionResult> Configure()
    {
        var model = await _configurationModelFactory.PrepareConfigurationModelAsync();
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Security.ACCESS_ADMIN_PANEL)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();

        // Save BambooCardDiscountSettings
        var bambooCardDiscountSettings = await _settingService.LoadSettingAsync<BambooCardDiscountSettings>(storeScope);
        bambooCardDiscountSettings = model.BambooCardDiscountSettings.ToSettings(bambooCardDiscountSettings);

        await _settingService.SaveSettingOverridablePerStoreAsync(bambooCardDiscountSettings, x => x.EnableCustomDiscount, model.BambooCardDiscountSettings.EnableCustomDiscount_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(bambooCardDiscountSettings, x => x.MinimumOrderCount, model.BambooCardDiscountSettings.MinimumOrderCount_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(bambooCardDiscountSettings, x => x.DiscountPercentage, model.BambooCardDiscountSettings.DiscountPercentage_OverrideForStore, storeScope, false);

        // Save BCAPISettings
        var bcApiSettings = await _settingService.LoadSettingAsync<BCAPISettings>(storeScope);
        bcApiSettings.SecretKey = model.BCAPISettings.SecretKey;
        bcApiSettings.TokenValidityInMinute = model.BCAPISettings.TokenValidityInMinute;
        bcApiSettings.JwtRefreshTokenLifetimeInHours = model.BCAPISettings.JwtRefreshTokenLifetimeInHours;

        await _settingService.SaveSettingOverridablePerStoreAsync(bcApiSettings, x => x.SecretKey, model.BCAPISettings.SecretKey_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(bcApiSettings, x => x.TokenValidityInMinute, model.BCAPISettings.TokenValidityInMinute_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(bcApiSettings, x => x.JwtRefreshTokenLifetimeInHours, model.BCAPISettings.JwtRefreshTokenLifetimeInHours_OverrideForStore, storeScope, false);

        // Clear settings cache
        await _settingService.ClearCacheAsync();

        return RedirectToAction("Configure");
    }


    #endregion
}
