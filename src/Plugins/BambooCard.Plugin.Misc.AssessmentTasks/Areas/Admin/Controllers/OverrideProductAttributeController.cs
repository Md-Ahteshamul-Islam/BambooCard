using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Mvc.Filters;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Controllers;
public class OverrideProductAttributeController : ProductAttributeController
{

    #region Ctor

    public OverrideProductAttributeController(ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IPermissionService permissionService,
        IProductAttributeModelFactory productAttributeModelFactory,
        IProductAttributeService productAttributeService) : base(
         customerActivityService,
         localizationService,
         localizedEntityService,
         notificationService,
         permissionService,
         productAttributeModelFactory,
         productAttributeService)
    { }

    #endregion

    #region Attribute list / create / edit / delete

    [CheckPermission(StandardPermission.Catalog.PRODUCT_ATTRIBUTES_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _productAttributeModelFactory.PrepareProductAttributeSearchModelAsync(new ProductAttributeSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Catalog.PRODUCT_ATTRIBUTES_VIEW)]
    public virtual async Task<IActionResult> List(ProductAttributeSearchModel searchModel)
    {
        //prepare model
        var model = await _productAttributeModelFactory.PrepareProductAttributeListModelAsync(searchModel);

        return Json(model);
    }
    #endregion
}
