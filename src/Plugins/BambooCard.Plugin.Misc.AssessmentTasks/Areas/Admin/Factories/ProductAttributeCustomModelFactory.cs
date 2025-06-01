using BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Models.ProductAttributes;
using BambooCard.Plugin.Misc.AssessmentTasks.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Areas.Admin.Factories;
public class ProductAttributeCustomModelFactory : IProductAttributeCustomModelFactory
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly IProductService _productService;
    private readonly IProductAttributeCustomService _productAttributeCustomService;

    #endregion

    #region Ctor

    public ProductAttributeCustomModelFactory(ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IProductAttributeService productAttributeService,
        IProductService productService,
        IProductAttributeCustomService productAttributeCustomService)
    {
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _productAttributeService = productAttributeService;
        _productService = productService;
        _productAttributeCustomService = productAttributeCustomService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare product attribute search model
    /// </summary>
    /// <param name="searchModel">Product attribute search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product attribute search model
    /// </returns>
    public virtual Task<OverrideProductAttributeSearchModel> PrepareOverrideProductAttributeSearchModelAsync(OverrideProductAttributeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged product attribute list model
    /// </summary>
    /// <param name="searchModel">Product attribute search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product attribute list model
    /// </returns>
    public virtual async Task<ProductAttributeListModel> PrepareProductAttributeListModelAsync(OverrideProductAttributeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get product attributes
        var productAttributes = await _productAttributeCustomService
            .GetAllProductAttributesAsync(name:searchModel.SearchName, pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        //prepare list model
        var model = new ProductAttributeListModel().PrepareToGrid(searchModel, productAttributes, () =>
        {
            //fill in model values from the entity
            return productAttributes.Select(attribute => attribute.ToModel<ProductAttributeModel>());

        });

        return model;
    }

    #endregion
}
