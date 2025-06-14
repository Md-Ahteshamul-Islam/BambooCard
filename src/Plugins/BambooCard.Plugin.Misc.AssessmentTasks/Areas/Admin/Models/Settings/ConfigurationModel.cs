﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
public record ConfigurationModel : BaseNopModel, ISettingsModel
{
    #region CTOR
    public ConfigurationModel()
    {
        BambooCardDiscountSettings = new BambooCardDiscountSettingsModel();
        BCAPISettings = new BCAPISettingsModel();
    }

    #endregion
    #region Properties

    public BambooCardDiscountSettingsModel BambooCardDiscountSettings { get; set; }
    public BCAPISettingsModel BCAPISettings { get; set; }


    public int ActiveStoreScopeConfiguration { get; set; }

    #endregion
}
