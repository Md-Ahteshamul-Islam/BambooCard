using Newtonsoft.Json;

namespace BambooCard.Plugin.Misc.AssessmentTasks.Models.Order;
public record OrderDetailsModel
{
    [JsonProperty("order_id")]
    public int OrderId { get; set; }

    [JsonProperty("total_amount")]
    public decimal TotalAmount { get; set; }

    [JsonProperty("order_date")]
    public DateTime OrderDate { get; set; }
}
