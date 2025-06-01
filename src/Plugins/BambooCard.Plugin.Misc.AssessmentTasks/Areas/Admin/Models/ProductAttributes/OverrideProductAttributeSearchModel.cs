using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.ProductAttributes;
/// <summary>
/// Extends a Product Attribute search model
/// </summary>
public record OverrideProductAttributeSearchModel : ProductAttributeSearchModel
{
    #region Properties

    [NopResourceDisplayName("BambooCard.Admin.Models.ProductAttributes.List.SearchName")]
    public string SearchName { get; set; }

    #endregion
}
