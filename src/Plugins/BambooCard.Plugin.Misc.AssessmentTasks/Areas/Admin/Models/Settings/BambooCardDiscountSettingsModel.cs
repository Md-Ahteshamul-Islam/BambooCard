using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
public record BambooCardDiscountSettingsModel : BaseNopModel, ISettingsModel
{

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BambooCardDiscountSettings.EnableCustomDiscount")]
    public bool EnableCustomDiscount { get; set; }
    public bool EnableCustomDiscount_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BambooCardDiscountSettings.MinimumOrderCount")]
    public int MinimumOrderCount { get; set; }
    public bool MinimumOrderCount_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.BambooCardDiscountSettings.DiscountPercentage")]
    public decimal DiscountPercentage { get; set; }
    public bool DiscountPercentage_OverrideForStore { get; set; }

    public int ActiveStoreScopeConfiguration { get; set; }
}
