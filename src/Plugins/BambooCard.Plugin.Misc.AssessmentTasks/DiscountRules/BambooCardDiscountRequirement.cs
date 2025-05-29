using Nop.Core.Domain.Discounts;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Orders;
using Nop.Services.Configuration;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Nop.Services.Plugins;

namespace BambooCard.Plugin.Misc.AssessmentTasks.DiscountRules;

public class BambooCardDiscountRequirement : IDiscountRequirementRule
{
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly ISettingService _settingService;

    public BambooCardDiscountRequirement(
        ICustomerService customerService,
        IOrderService orderService,
        ISettingService settingService)
    {
        _customerService = customerService;
        _orderService = orderService;
        _settingService = settingService;
    }

    /// <summary>
    /// Gets or sets the plugin descriptor
    /// </summary>
    public PluginDescriptor PluginDescriptor { get; set; }

    /// <summary>
    /// Validates whether the discount requirement is met
    /// </summary>
    public async Task<DiscountRequirementValidationResult> CheckRequirementAsync(DiscountRequirementValidationRequest request)
    {
        var result = new DiscountRequirementValidationResult();

        if (request.Customer == null)
            return result;

        var settings = await _settingService.LoadSettingAsync<BambooCardDiscountSettings>();

        if (!settings.EnableCustomDiscount)
            return result;

        var orders = await _orderService.SearchOrdersAsync(customerId: request.Customer.Id);

        if (orders.TotalCount >= settings.MinimumOrderCount)
            result.IsValid = true;

        return result;
    }

    /// <summary>
    /// Gets the legacy configuration page URL (not used)
    /// </summary>
    public string GetConfigurationPageUrl()
    {
        return string.Empty; // No UI configuration page
    }

    /// <summary>
    /// Gets the legacy configuration URL (not used)
    /// </summary>
    public string GetConfigurationUrl(int discountId, int? discountRequirementId)
    {
        return string.Empty; // No UI configuration page
    }

    /// <summary>
    /// Gets the requirement configuration URL (used in admin, optional)
    /// </summary>
    public Task<string> GetRequirementConfigurationUrlAsync(string discountId, string discountRequirementId)
    {
        return Task.FromResult<string>(null); // No config view for this rule
    }

    /// <summary>
    /// Plugin install logic
    /// </summary>
    public Task InstallAsync()
    {
        // Add permissions, settings, or localization here if needed
        return Task.CompletedTask;
    }

    /// <summary>
    /// Prepare plugin for uninstall (cleanup)
    /// </summary>
    public Task PreparePluginToUninstallAsync()
    {
        // Remove any persistent data if needed
        return Task.CompletedTask;
    }

    /// <summary>
    /// Plugin uninstall logic
    /// </summary>
    public Task UninstallAsync()
    {
        // Deregister anything if required
        return Task.CompletedTask;
    }

    /// <summary>
    /// Update logic if you version the plugin in future
    /// </summary>
    public Task UpdateAsync(string currentVersion, string targetVersion)
    {
        // Apply upgrade logic between versions if needed
        return Task.CompletedTask;
    }
}
