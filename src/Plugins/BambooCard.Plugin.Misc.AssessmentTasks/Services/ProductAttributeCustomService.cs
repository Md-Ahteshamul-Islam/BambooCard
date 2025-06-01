using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Services;
public class ProductAttributeCustomService : IProductAttributeCustomService
{
    #region Fields

    protected readonly IRepository<ProductAttribute> _productAttributeRepository;

    #endregion

    #region Ctor

    public ProductAttributeCustomService(IRepository<ProductAttribute> productAttributeRepository)
    {
        _productAttributeRepository = productAttributeRepository;
    }

    #endregion


    #region Methods
    /// <summary>
    /// Gets all product attributes
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product attributes
    /// </returns>
    public virtual async Task<IPagedList<ProductAttribute>> GetAllProductAttributesAsync(string name = null, int pageIndex = 0,
    int pageSize = int.MaxValue)
    {
        var productAttributes = await _productAttributeRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrEmpty(name))
                query = query.Where(pa => pa.Name.Contains(name));

            return query.OrderBy(pa => pa.Name);
        }, pageIndex, pageSize);

        return productAttributes;
    }


    #endregion
}
