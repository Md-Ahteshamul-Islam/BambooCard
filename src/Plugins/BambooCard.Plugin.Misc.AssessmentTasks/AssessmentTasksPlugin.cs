using System.Text;
using BambooCard.Plugin.Misc.AssessmentTasks.Settings;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Menu;

namespace BambooCard.Plugin.Misc.AssessmentTasks;

/// <summary>
/// Represents the BambooCard plugin containing custom assessment features:
/// - Custom discounts
/// - Checkout enhancements
/// - Admin product attribute search
/// - Order retrieval API
/// - Docker deployment
/// </summary>
public class AssessmentTasksPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin, IDiscountRequirementRule
{
    #region Fields

    private readonly IPermissionService _permissionService;
    private readonly IWebHelper _webHelper;
    private readonly ILanguageService _languageService;
    private readonly ILogger _logger;
    private readonly ILocalizationService _localizationService;
    private readonly IAdminMenu _adminMenu;
    private readonly ISettingService _settingService;
    private readonly IDiscountService _discountService;
    private readonly IOrderService _orderService;
    private readonly IAttributeService<CheckoutAttribute, CheckoutAttributeValue> _checkoutAttributeService;

    #endregion

    #region Ctor

    public AssessmentTasksPlugin(
        IPermissionService permissionService,
        IWebHelper webHelper,
        ILanguageService languageService,
        ILogger logger,
        ILocalizationService localizationService,
        IAdminMenu adminMenu,
        ISettingService settingService,
        IDiscountService discountService,
    IOrderService orderService,
        IAttributeService<CheckoutAttribute, CheckoutAttributeValue> checkoutAttributeService)
    {
        _permissionService = permissionService;
        _webHelper = webHelper;
        _languageService = languageService;
        _logger = logger;
        _localizationService = localizationService;
        _adminMenu = adminMenu;
        _settingService = settingService;
        _discountService = discountService;
        _orderService = orderService;
        _checkoutAttributeService = checkoutAttributeService;
    }

    #endregion

    #region Utilities
    private Language GetDefaultEnglishLanguage()
    {
        return _languageService.GetAllLanguages().Where(x => x.UniqueSeoCode.Equals("en", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
    }

    private async Task InstalLocalResourseStringFromXmlFileAsync()
    {
        var language = GetDefaultEnglishLanguage();

        if (language == null)
        {
            _logger.Error("Can't Add Resource string. Couldn't Find The Requered Language!");
            return;
        }

        try
        {
            var fileProvider = EngineContext.Current.Resolve<INopFileProvider>();
            var path = fileProvider.MapPath(AssessmentTasksDefaults.XmlResourceStringFilePath);
            using var sr = new StreamReader(path, Encoding.UTF8);
            await _localizationService.ImportResourcesFromXmlAsync(language, sr);
        }
        catch (Exception ex)
        {
            _logger.Error("GeneratorShop: Can't Add Resource string!", ex);
        }
    }
    private async Task CreateLoyalCustomerDiscountAsync()
    {
        var discountName = "Loyal Customer Discount";

        var existingDiscounts = await _discountService.GetAllDiscountsAsync(discountName: discountName);
        if (existingDiscounts.Any())
            return; // Already exists

        // Load settings to determine default discount percentage
        var discountSettings = await _settingService.LoadSettingAsync<BambooCardDiscountSettings>();

        // Create the discount
        var discount = new Discount
        {
            Name = discountName,
            DiscountType = DiscountType.AssignedToOrderSubTotal,
            UsePercentage = true,
            DiscountPercentage = discountSettings.DiscountPercentage,
            RequiresCouponCode = false,
            IsCumulative = false,
            LimitationTimes = 0,
            DiscountLimitationId = (int)DiscountLimitationType.Unlimited,
            StartDateUtc = null,
            EndDateUtc = null,
            AppliedToSubCategories = false
        };

        await _discountService.InsertDiscountAsync(discount);

        // Attach custom discount requirement rule
        var discountRequirement = new DiscountRequirement
        {
            DiscountId = discount.Id,
            DiscountRequirementRuleSystemName = AssessmentTasksDefaults.SystemName
        };

        await _discountService.InsertDiscountRequirementAsync(discountRequirement);

        // Add a localization string for better UX
        await _localizationService.AddOrUpdateLocaleResourceAsync("DiscountRequirements.BambooCardDiscountRequirement", "Applies to loyal customers with minimum order history");
    }
    protected virtual async Task EnsureGiftMessageCheckoutAttributeExistsAsync()
    {
        var attributes = await _checkoutAttributeService.GetAllAttributesAsync();
        var existing = attributes.FirstOrDefault(a =>
            a.Name.Equals("GiftMessage", StringComparison.InvariantCultureIgnoreCase));

        if (existing != null)
            return;

        var attribute = new CheckoutAttribute
        {
            Name = "GiftMessage",
            TextPrompt = "Enter your gift message",
            IsRequired = true,
            AttributeControlType = AttributeControlType.MultilineTextbox,
            DisplayOrder = 1
        };

        await _checkoutAttributeService.InsertAttributeAsync(attribute);

        await _localizationService.AddOrUpdateLocaleResourceAsync("CheckoutAttribute.GiftMessage", "Gift Message");
    }


    #endregion

    #region Methods

    /// <summary>
    /// Gets the plugin's configuration page URL.
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/AssessmentTasks/Configure";
    }


    private async void PreConfigureSettings()
    {
        var bambooCardDiscountSettings = await _settingService.LoadSettingAsync<BambooCardDiscountSettings>();
        bambooCardDiscountSettings.EnableCustomDiscount = true;
        bambooCardDiscountSettings.MinimumOrderCount = 3;
        bambooCardDiscountSettings.DiscountPercentage = 10;

        // Save the settings
        await _settingService.SaveSettingAsync(bambooCardDiscountSettings);
        var bcAPISettings = await _settingService.LoadSettingAsync<BCAPISettings>();
        bcAPISettings.TokenValidityInMinute = 60;
        bcAPISettings.JwtRefreshTokenLifetimeInHours = 1;

        // Save the settings
        await _settingService.SaveSettingAsync(bambooCardDiscountSettings);

        // clear cache if needed
        await _settingService.ClearCacheAsync();
    }


    public async override Task InstallAsync()
    {
        await base.InstallAsync();

        PreConfigureSettings();
        await CreateLoyalCustomerDiscountAsync();
        await InstalLocalResourseStringFromXmlFileAsync();
        await EnsureGiftMessageCheckoutAttributeExistsAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        if (targetVersion != currentVersion)
        {
            await InstalLocalResourseStringFromXmlFileAsync();
            await CreateLoyalCustomerDiscountAsync();
            await EnsureGiftMessageCheckoutAttributeExistsAsync();
        }

        await base.UpdateAsync(currentVersion, targetVersion);
    }

    public async override Task UninstallAsync()
    {
        await base.UninstallAsync();
    }

    public async Task ManageSiteMapAsync(AdminMenuItem rootNode)
    {
        var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
        if (pluginNode != null)
        {
            pluginNode.Visible = true;
            pluginNode.SystemName = "BambooCard";
            pluginNode.Title = "Bamboo Card";
            pluginNode.IconClass = "far fa-dot-circle";

            pluginNode.ChildNodes.Add(new AdminMenuItem()
            {
                Visible = true,
                SystemName = "BambooCard.Configuration",
                Title = "Configure",
                Url = _adminMenu.GetMenuItemUrl("BambooCardConfigure", "Configure"),
                IconClass = "far fa-dot-circle"
            });
        }
    }


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

    public string GetConfigurationUrl(int discountId, int? discountRequirementId)
    {
        return $"{_webHelper.GetStoreLocation()}Admin/AssessmentTasks/Configure";

    }

    #endregion
}
