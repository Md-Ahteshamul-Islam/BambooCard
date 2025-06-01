using Nop.Core.Configuration;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Settings;

/// <summary>
/// Represents settings for the BambooCard custom discount feature
/// </summary>
public class BambooCardDiscountSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the discount is enabled
    /// </summary>
    public bool EnableCustomDiscount { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of past orders required to qualify for the discount
    /// </summary>
    public int MinimumOrderCount { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage to be applied
    /// </summary>
    public decimal DiscountPercentage { get; set; }
}
