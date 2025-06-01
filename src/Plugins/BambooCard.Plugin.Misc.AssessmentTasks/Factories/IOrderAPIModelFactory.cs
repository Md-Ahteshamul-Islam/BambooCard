using BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Factories;
public interface IOrderAPIModelFactory
{
    Task<OrderLookupResponseModel> PrepareOrderLookupResponseAsync(string email);
}