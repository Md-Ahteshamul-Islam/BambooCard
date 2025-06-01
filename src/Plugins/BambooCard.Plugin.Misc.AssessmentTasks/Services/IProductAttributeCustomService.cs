using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;
public interface IProductAttributeCustomService
{
    Task<IPagedList<ProductAttribute>> GetAllProductAttributesAsync(string name = null, int pageIndex = 0, int pageSize = int.MaxValue);
}