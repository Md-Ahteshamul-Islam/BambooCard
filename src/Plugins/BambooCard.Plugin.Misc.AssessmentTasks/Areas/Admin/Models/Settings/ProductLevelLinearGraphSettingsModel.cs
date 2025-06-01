using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings;
public record ProductLevelLinearGraphSettingsModel : BaseNopModel, ISettingsModel
{
    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.DefaultMaxValueInYAxis")]
    public int DefaultMaxValueInYAxis { get; init; }
    public bool DefaultMaxValueInYAxis_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.DefaultMinValueInYAxis")]
    public int DefaultMinValueInYAxis { get; init; }
    public bool DefaultMinValueInYAxis_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.CapacityUOM")]
    public string CapacityUOM { get; init; }
    public bool CapacityUOM_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.NoOfLevelsInYAxis")]
    public int NoOfLevelsInYAxis { get; init; }
    public bool NoOfLevelsInYAxis_OverrideForStore { get; set; }

    [NopResourceDisplayName("BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.Settings.MarkersInXAxis")]
    public int MarkersInXAxis { get; init; }
    public bool MarkersInXAxis_OverrideForStore { get; set; }

    public int ActiveStoreScopeConfiguration { get; set; }
}
