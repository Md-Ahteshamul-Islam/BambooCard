using BambooCard.Plugin.Misc.AssessmentTasks.Factories;
using BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Services.Localization;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Controllers.API;
[Route("api/order")]
public class OrderApiController : BaseApiController
{
    private readonly IWorkContext _workContext;
    private readonly IOrderAPIModelFactory _orderAPIModelFactory;

    public OrderApiController(ILocalizationService localizationService,
        ICustomerService customerService,
        IWorkContext workContext,
        IOrderAPIModelFactory orderAPIModelFactory)
        : base(localizationService, customerService)
    {
        _workContext = workContext;
        _orderAPIModelFactory = orderAPIModelFactory;
    }

    [HttpPost("details")]
    public async Task<IActionResult> Details(OrderLookupRequestModel searchModel)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        if (await IsGuestCustomerAsync(customer))
            return await Unauthorized();

        var model = await _orderAPIModelFactory.PrepareOrderLookupResponseAsync(searchModel.Email);

        return Ok(model);
    }

}
