using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.ProductAttributes;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
public interface IProductAttributeCustomModelFactory
{
    Task<OverrideProductAttributeSearchModel> PrepareOverrideProductAttributeSearchModelAsync(OverrideProductAttributeSearchModel searchModel);
    Task<ProductAttributeListModel> PrepareProductAttributeListModelAsync(OverrideProductAttributeSearchModel searchModel);
}