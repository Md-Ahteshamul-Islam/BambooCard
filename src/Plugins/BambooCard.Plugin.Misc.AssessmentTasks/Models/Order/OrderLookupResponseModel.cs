using Newtonsoft.Json;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;
public record OrderLookupResponseModel
{
    public OrderLookupResponseModel()
    {
        Orders = new List<OrderDetailsModel>();
    }
    [JsonProperty("orders")]
    public IList<OrderDetailsModel> Orders { get; set; }
}
