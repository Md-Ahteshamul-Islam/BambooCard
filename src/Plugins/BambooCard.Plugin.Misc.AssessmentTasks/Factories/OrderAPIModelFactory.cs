using BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;
using Nop.Services.Customers;
using Nop.Services.Orders;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Factories
{
    public class OrderAPIModelFactory : IOrderAPIModelFactory
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public OrderAPIModelFactory(
            ICustomerService customerService,
            IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        public async Task<OrderLookupResponseModel> PrepareOrderLookupResponseAsync(string email)
        {
            var response = new OrderLookupResponseModel();

            if (string.IsNullOrWhiteSpace(email))
                return response;

            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
                return response;

            var orders = await _orderService.SearchOrdersAsync(customerId: customer.Id);

            response.Orders = orders.Select(o => new OrderDetailsModel
            {
                OrderId = o.Id,
                TotalAmount = o.OrderTotal,
                OrderDate = o.CreatedOnUtc
            }).ToList();

            return response;
        }
    }
}
